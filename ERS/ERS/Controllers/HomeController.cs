using ERS.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using ERSDAL;
using ERSBLL;
using ERSEntity;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace ERS.Controllers
{
    public class HomeController : SecurityController
    {
        RequestDetailsDAL objReqDetailsDAL = null;
        RequestFacilityDAL objReqFacilityDAL = null;
        FacilityBLL objFacilityBLL = null;
        EquipmentCategoryReportBLL objEquipCategoryReportBLL = null;
        EquipmentCategoryBLL objEquipCategoryBLL = null;

        public ActionResult Index()
        {
            if (Session["SystemEmailRequestActionGUID"] != null)
            {
                return Redirect("~/SystemEmail/Execute?id=" + Session["SystemEmailRequestActionGUID"].ToString());
            }

            objReqDetailsDAL = new RequestDetailsDAL();

            // For Claimed Item Quantity
            var claimedQuantities = objReqDetailsDAL.GetClaimedEquipmentsQuantity();
            var resultClaimed = claimedQuantities.Select(y => new
            {
                TotalSumQuantity = claimedQuantities.Sum(x => x.Quantity)
            });

            var claimedItemCount = 0;

            if (resultClaimed.Count() > 0)
                claimedItemCount = resultClaimed.FirstOrDefault().TotalSumQuantity;
            else if (resultClaimed.Count() == 0)
                claimedItemCount = 0;

            // For Unclaimed Item Quantity
            var unclaimedQuantities = objReqDetailsDAL.GetUnclaimedEquipmentsQuantity();
            var resultUnclaimed = unclaimedQuantities.Select(y => new
            {
                TotalSumQuantity = unclaimedQuantities.Sum(x => x.Quantity)
            });

            var unclaimedItemCount = 0;

            if (resultUnclaimed.Count() > 0)
                unclaimedItemCount = resultUnclaimed.FirstOrDefault().TotalSumQuantity;
            else if (resultClaimed.Count() == 0)
                unclaimedItemCount = 0;

            // For Occupied Rooms List
            objReqFacilityDAL = new RequestFacilityDAL();
            var occupiedFacilities = objReqFacilityDAL.GetClaimedFacilityByCurrentDate().ToList()
                .Select(x => x.RoomNumber).ToArray();

            // For Vacant Rooms List
            objFacilityBLL = new FacilityBLL();
            var facilityList = objFacilityBLL.GetListOnlyAvailable().ToList()
                .Select(y => y.RoomNumber).ToArray();

            var vacantFacilities = facilityList.Except(occupiedFacilities).ToArray();

            // For Remaining Equipment list
            objEquipCategoryBLL = new EquipmentCategoryBLL();
            var availableEquipmentList = objEquipCategoryBLL.GetListOnlyAvailable().ToList();

            RequestDetailsEntity objRequestDetailsEntity = new RequestDetailsEntity();
            RequestDetailsDAL objRequestDetailsDAL = new RequestDetailsDAL();
            var dateToday = DateTime.Now.ToString("M/d/yyyy hh:mm tt");
            var borrowedEquipments = objRequestDetailsDAL.SelectRequestDetailsByDate(dateToday, ""); // Get all borrowed item count

            var resultsBorrowedEquipments = borrowedEquipments.GroupBy(x => new { x.EquipmentCategoryID })
                .Select(y => new
                {
                    EquipmentCategoryID = y.Key.EquipmentCategoryID.ToString(),
                    TotalSumBorrowed = borrowedEquipments.Where(item => item.EquipmentCategoryID == y.Key.EquipmentCategoryID).Sum(item => item.Quantity)
                }); // Add all borrowed quantity with respect to EquipCategoryID

            var totalBorrowedEquipmentList = (from a in availableEquipmentList
                                            join b in resultsBorrowedEquipments
                                            on a.EquipmentCategoryID equals Convert.ToInt32(b.EquipmentCategoryID)
                                            select new
                                            { b.EquipmentCategoryID, b.TotalSumBorrowed }).AsEnumerable(); // Join borrowed quantity sum to the default equip list and instantiate new list

            var remainingEquipmentList = availableEquipmentList.Select(x => new EquipmentCategoryEntity()
            {
                Category = x.Category,
                EquipmentCategoryID = x.EquipmentCategoryID,
                QuantityUsable = ((x.QuantityUsable - totalBorrowedEquipmentList.Where(y => y.EquipmentCategoryID == x.EquipmentCategoryID.ToString())
                                                                             .Select(quantity => quantity.TotalSumBorrowed)
                                                                             .DefaultIfEmpty(0)
                                                                             .FirstOrDefault()) < 0 ) ? 0 : x.QuantityUsable - totalBorrowedEquipmentList.Where(y => y.EquipmentCategoryID == x.EquipmentCategoryID.ToString())
                                                                                                             .Select(quantity => quantity.TotalSumBorrowed)
                                                                                                             .DefaultIfEmpty(0)
                                                                                                             .FirstOrDefault()
                                                                             // Deduct borrowed equipments from default quantities
            }).ToList();

            // My Ongoing Equipment List
            var ongoingEquipmentUnclaimed = objReqDetailsDAL.SelectRequestDetailsByStatusAndSystemUserID("Unclaimed", _objSystemUser.ID).ToList();
            var resultsOngoingUnclaimedEquipments = ongoingEquipmentUnclaimed.GroupBy(x => new { x.Category, x.Status})
                .Select(y => new RequestDetailsEntity()
                {
                    Category = y.Key.Category,
                    Status = y.Key.Status,
                    Quantity = ongoingEquipmentUnclaimed.Where(item => item.Category == y.Key.Category).Sum(item => item.Quantity)
                }).ToList();
            var ongoingEquipmentClaimed = objReqDetailsDAL.SelectRequestDetailsByStatusAndSystemUserID("Claimed", _objSystemUser.ID).ToList();
            var resultsOngoingClaimedEquipments = ongoingEquipmentClaimed.GroupBy(x => new { x.Category, x.Status })
                .Select(y => new RequestDetailsEntity()
                {
                    Category = y.Key.Category,
                    Status = y.Key.Status,
                    Quantity = ongoingEquipmentClaimed.Where(item => item.Category == y.Key.Category).Sum(item => item.Quantity)
                }).ToList();

            // My Ongoing Facility List
            var ongoingFacilityUnclaimed = objReqFacilityDAL.SelectRequestFacilityGetByStatusAndSystemUserID("Unclaimed", _objSystemUser.ID).ToList();
            var ongoingFacilityClaimed = objReqFacilityDAL.SelectRequestFacilityGetByStatusAndSystemUserID("Claimed", _objSystemUser.ID).ToList();

            // Load all Facility for dropdown
            var FacilityDAL = new FacilityDAL();
            var facilityListDropdown = FacilityDAL.GetListOnlyAvailable().ToList();

            // Viewbags
            ViewBag.FacilityListDropdown = facilityListDropdown;
            ViewBag.OngoingUnclaimedFacility = ongoingFacilityUnclaimed;
            ViewBag.OngoingClaimedFacility = ongoingFacilityClaimed;
            ViewBag.OngoingClaimedEquipment = resultsOngoingClaimedEquipments;
            ViewBag.OngoingUnclaimedEquipment = resultsOngoingUnclaimedEquipments;
            ViewBag.RemainingEquipmentList = remainingEquipmentList;
            ViewBag.VacantFacilityList = vacantFacilities;
            ViewBag.OccupiedFacilityList = occupiedFacilities;
            ViewBag.TotalClaimedItemQuantity = claimedItemCount;
            ViewBag.TotalUnclaimedItemQuantity = unclaimedItemCount;

            return View();
        }

        [HttpGet]
        public ActionResult GetEquipmentUsageByDate(string DateFrom, string DateTo)
        {
            objEquipCategoryReportBLL = new EquipmentCategoryReportBLL();
            EquipmentCategoryReportEntity objEntity = new EquipmentCategoryReportEntity();
            objEntity.DateFrom = DateFrom;
            objEntity.DateTo = DateTo;

            var result = objEquipCategoryReportBLL.GetFiltered(objEntity);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetClassScheduleByFacilityID(int FacilityID)
        {
            var FacilityDAL = new FacilityDAL();
            var FacilityListEntity = new FacilityListEntity()
            {
                FacilityID = FacilityID,
                RowStart = 1,
                NoOfRecord = 100,
                SortColumn = 0,
                SortDirection = ""
            };
            var ScheduleListEntity = new List<FacilityListEntity>();
            var scheduleList = FacilityDAL.GetScheduleList(FacilityListEntity).ToList();
            if (scheduleList.Count > 0)
            {
                foreach(var item in scheduleList)
                {
                    string DayOfTheWeek = string.Empty;
                    switch (item.ScheduleDay)
                    {
                        case 0:
                            DayOfTheWeek = "Monday";
                            break;
                        case 1:
                            DayOfTheWeek = "Tuesday";
                            break;
                        case 2:
                            DayOfTheWeek = "Wednesday";
                            break;
                        case 3:
                            DayOfTheWeek = "Thursday";
                            break;
                        case 4:
                            DayOfTheWeek = "Friday";
                            break;
                        case 5:
                            DayOfTheWeek = "Saturday";
                            break;
                    }
                    ScheduleListEntity.Add(new FacilityListEntity()
                    {
                        CourseName = item.CourseName,
                        DayOfTheWeek = DayOfTheWeek,
                        TimeIn = DateTime.Now.ToString("M/d/yyyy") + " " + item.TimeIn,
                        TimeOut = DateTime.Now.ToString("M/d/yyyy") + " " + item.TimeOut
                    });
                }
            }

            return Json(ScheduleListEntity, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ErrorPage()
        {
            return View();
        }

        public ActionResult ErrorPagePartial()
        {
            return View();
        }

        public ActionResult SessionExpired()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}