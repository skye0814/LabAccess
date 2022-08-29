using ERS;
using ERSBLL;
using ERSDAL;
using ERSEntity;
using ERSEntity.Entity;
using ERSUtil;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERS.Controllers
{
    public class RequestEquipmentController : CommonController
    {
        EquipmentCategoryBLL _equipmentCategoryBLL = null;
        RequestEquipmentBLL _requestEquipmentBLL = null;
        RequestsEntity _requestsEntity = null;
        RequestDetailsEntity _requestsDetailEntity = null;
        HomeController _homeController = null;

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RequestEquipmentAdd()
        {
            PopulateDropdown();

            // Load all RequestDetails
            RequestDetailsEntity objRequestDetailsEntity = new RequestDetailsEntity();
            RequestDetailsDAL objRequestDetailsDAL = new RequestDetailsDAL();
            var allRequestDetailsList = objRequestDetailsDAL.GetAllRequestDetails();
            ViewBag.AllRequestDetailsList = allRequestDetailsList;
            

            return View();
        }

        [HttpGet]
        public ActionResult GetAvailableItemsByRequestDate(string StartTime, string EndTime)
        {
            PopulateDropdown();
            RequestDetailsEntity objRequestDetailsEntity = new RequestDetailsEntity();
            RequestDetailsDAL objRequestDetailsDAL = new RequestDetailsDAL();

            // Get list of borrowed equipments by given date
            var borrowedEquipments = objRequestDetailsDAL.SelectRequestDetailsByDate(StartTime, EndTime);
            
            // Add all borrowed quantities for each equipment category
            var results = borrowedEquipments.GroupBy(x => new { x.EquipmentCategoryID })
                .Select(y => new
                {
                    EquipmentCategoryID = y.Key.EquipmentCategoryID.ToString(),
                    TotalSumBorrowed = borrowedEquipments.Where(item => item.EquipmentCategoryID == y.Key.EquipmentCategoryID).Sum(item => item.Quantity)
                });

            // Get only all available equipments and their borrowed quantities
            var query = (from a in (List<SelectListItem>)TempData["EquipmentCategory"]
                            join b in results
                            on a.Value equals b.EquipmentCategoryID
                            select new
                            { b.EquipmentCategoryID, b.TotalSumBorrowed});
   
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RequestEquipmentList()
        {
            if (_objSystemUser.isLabPersonnel && !_objSystemUser.isStudent)
            {
                return new HttpStatusCodeResult(401);
            }
            else
            {
                RequestEntityDAL objDAL = new RequestEntityDAL();
                var penalties = objDAL.GetAllActiveEquipmentRequestPenaltyBySystemUserID(_objSystemUser.ID);

                ViewBag.Penalties = penalties;
            }
            return View();
        }

        private void PopulateDropdown()
        {
            List<EquipmentCategoryEntity> lstEquipCategoryEntity = new List<EquipmentCategoryEntity>();
            SelectListItem EquipCategoryItemSelection = new SelectListItem();
            SelectListItem EquipCategoryQuantitySelection = new SelectListItem();
            List<SelectListItem> lstEquipCategoryItemSelection = new List<SelectListItem>();

            List<int> quantityAvailable = new List<int>();
            EquipmentCategoryQuantityAvailable quantityAvailableForEach = new EquipmentCategoryQuantityAvailable()
            {
                QuantityForEach = new List<List<int>>()
            };

            _equipmentCategoryBLL = new EquipmentCategoryBLL();
            lstEquipCategoryEntity = _equipmentCategoryBLL.GetListOnlyAvailable().ToList();

            if (lstEquipCategoryEntity.ToList().Count > 0)
            {
                foreach (EquipmentCategoryEntity item in lstEquipCategoryEntity)
                {
                    EquipCategoryItemSelection = new SelectListItem();
                    quantityAvailable = new List<int>();

                    EquipCategoryItemSelection.Value = "" + item.EquipmentCategoryID.ToString();
                    EquipCategoryItemSelection.Text = "(" + item.CategoryCode + ") " + item.Category;
                    lstEquipCategoryItemSelection.Add(EquipCategoryItemSelection);
                    EquipCategoryItemSelection = null;

                    for (int i = 1; i <= item.QuantityUsable; i++)
                    {
                        quantityAvailable.Add(i);
                    }
                    quantityAvailableForEach.QuantityForEach.Add(quantityAvailable);
                    
                }
                

            }
            ViewBag.EquipmentCategoryList = lstEquipCategoryEntity;
            ViewBag.QuantityAvailableForEachItem = quantityAvailableForEach;
            ViewBag.EquipmentCategory = lstEquipCategoryItemSelection;

            TempData["EquipmentCategoryList"] = lstEquipCategoryEntity;
            TempData["EquipmentCategory"] = lstEquipCategoryItemSelection;

        }

        public ActionResult RequestEquipmentEdit(string RequestGUID = "")
        {
            PopulateDropdown();
            RequestsEntity objRequestsEntity = new RequestsEntity();
            RequestDetailsEntity objEntity = new RequestDetailsEntity();

            RequestDetailsDAL objDAL = new RequestDetailsDAL();
            RequestEntityDAL objRequestsDAL = new RequestEntityDAL();

            ViewBag.Requests = objRequestsDAL.SelectRequestsByRequestGUID(RequestGUID);
            ViewBag.RequestDetailsList = objDAL.SelectRequestDetailsByRequestGUID(RequestGUID);
            return View();

        }

        public JsonResult GetRequestEquipmentList(string sidx = "", string sord = "", int page = 1, int rows = 10
          , RequestsListEntity objListEntityFilter = null)
        {
            IEnumerable<RequestsListEntity> objListTableView = null;
            RequestsListEntity objListTableFilter = null;

            int totalRecords = 0;
            int noOfPages = 1;
            int rowStart = 1;

            try
            {
                rowStart = page > 1 ? page * rows - rows + 1 : rowStart;

                objListTableFilter = new RequestsListEntity();
                objListTableFilter.RowStart = rowStart;
                objListTableFilter.NoOfRecord = rows;
                objListTableFilter.SortColumn = sidx == "" ? 0 : Convert.ToInt32(sidx);
                objListTableFilter.SortDirection = sord;
                objListTableFilter.Requestor = objListEntityFilter.Requestor;
                objListTableFilter.RequestDateTime = objListEntityFilter.RequestDateTime;
                objListTableFilter.StartTime = objListEntityFilter.StartTime;
                objListTableFilter.EndTime = objListEntityFilter.EndTime;
                objListTableFilter.isApproved = objListEntityFilter.isApproved;
                objListTableFilter.Remarks = objListEntityFilter.Remarks;
                objListTableFilter.RequestGUID = objListEntityFilter.RequestGUID;
                objListTableFilter.ClaimedTime = objListEntityFilter.ClaimedTime;
                objListTableFilter.ReturnedTime = objListEntityFilter.ReturnedTime;
                objListTableFilter.Status = objListEntityFilter.Status;
                objListTableFilter.RequestorID = objListEntityFilter.RequestorID;
                objListTableFilter.StatusMode = objListEntityFilter.StatusMode;

                _requestEquipmentBLL = new RequestEquipmentBLL();
                objListTableView = _requestEquipmentBLL.GetFiltered(objListTableFilter).ToList();

                if (objListTableView != null && objListTableView.Count() > 0)
                {
                    totalRecords = objListTableView.FirstOrDefault().TotalRecord;
                    noOfPages = (int)Math.Ceiling((float)totalRecords / (float)rows);

                    if (noOfPages == 0)
                        noOfPages = 1;
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                _resultEntity.IsListResult = false;
                _resultEntity.Result = ex.Message;

                return Json(_resultEntity, JsonRequestBehavior.AllowGet);
            }

            _requestEquipmentBLL = null;

            objListTableFilter = null;


            var jsonData = new
            {
                total = noOfPages,
                page,
                sort = sidx,
                records = totalRecords,
                rows = objListTableView
            };

            objListTableView = null;

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveRequestEquipment(RequestsEntity objRequestEquipmentEntity)
        {
            try
            {
                _resultEntity = new ResultEntity();
                var categoryList = (List<EquipmentCategoryEntity>)TempData["EquipmentCategoryList"];
                var objReqEquipmentDAL = new RequestDetailsDAL();
                _requestEquipmentBLL = new RequestEquipmentBLL();
                _requestsEntity = new RequestsEntity();
                _requestsDetailEntity = new RequestDetailsEntity();
                List<RequestDetailsEntity> requestDetails = new List<RequestDetailsEntity>();
                List<string> errorList = new List<string>();

                if (objRequestEquipmentEntity.RequestID == 0)
                {
                    // Ito yung request details
                    requestDetails = objRequestEquipmentEntity.RequestDetails;

                    var existingItems = objReqEquipmentDAL.SelectRequestDetailsByDate(objRequestEquipmentEntity.StartTime, objRequestEquipmentEntity.EndTime);
                    var existingItemsGroupedByID = existingItems.GroupBy(x => new { x.EquipmentCategoryID, x.Quantity })
                        .Select(y => new
                        {
                            EquipmentCategoryID = y.Key.EquipmentCategoryID.ToString(),
                            TotalSumBorrowed = existingItems.Where(item => item.EquipmentCategoryID == y.Key.EquipmentCategoryID).Sum(item => item.Quantity)
                        });

                    foreach(var item in requestDetails)
                    {
                        int? categoryID = item.EquipmentCategoryID;
                        int sumToBeReserved = item.Quantity;
                        int sumExisting = existingItemsGroupedByID.Where(x => x.EquipmentCategoryID == categoryID.ToString())
                                                                .Select(quantity => quantity.TotalSumBorrowed)
                                                                .DefaultIfEmpty(0)
                                                                .FirstOrDefault();
                        int quantityUsableByID = categoryList.Where(x => x.EquipmentCategoryID == categoryID).FirstOrDefault().QuantityUsable;
                        string categoryName = categoryList.Where(x => x.EquipmentCategoryID == categoryID).FirstOrDefault().Category;
                        if ((sumToBeReserved + sumExisting) > quantityUsableByID)
                        {
                            errorList.Add("Your selected quantity of <strong>" + categoryName + "</strong> has already reached its available quantity limit.");
                        }
                    }

                    if (errorList.Count != 0)
                    {
                        errorList.Add("<br/><strong>This is due to a user who submitted the reservation first. Please submit a new request again, thank you.</strong>");
                        _resultEntity.IsSuccess = false;
                        _resultEntity.IsListResult = false;
                    }
                    else
                    {
                        // Requests insert
                        objRequestEquipmentEntity.Requestor = _objSystemUser.FirstName + " " + _objSystemUser.LastName;
                        objRequestEquipmentEntity.RequestorID = _objSystemUser.ID;
                        _requestsEntity = _requestEquipmentBLL.InsertRequests(objRequestEquipmentEntity);

                        // Insert request details
                        foreach (var item in requestDetails)
                        {
                            item.RequestorID = _objSystemUser.ID;
                            _requestsDetailEntity = _requestEquipmentBLL.InsertRequestDetails(item);
                        }
                    }
                }

                if (_requestsEntity.IsSuccess)
                {
                    _resultEntity.IsSuccess = true;
                }
                else
                {
                    _resultEntity.IsSuccess = false;
                    _resultEntity.IsListResult = true;
                    _resultEntity.Result = errorList;
                }
            }
            catch (Exception ex)
            {
                _resultEntity.IsSuccess = false;
                _resultEntity.Result = ex.Message;
            }

            _requestEquipmentBLL = null;

            return Json(_resultEntity);
        }

        public JsonResult GenerateEquipmentItemQRCode(string RequestGUID)
        {
            _resultEntity = new ResultEntity();
            EncryptionUTIL encrypt = new EncryptionUTIL();
            string errorMessage = String.Empty;
            try
            {
                _resultEntity.Result = QRGenerate(encrypt.Encode(RequestGUID, ref errorMessage));

                if (String.IsNullOrEmpty(errorMessage) || string.IsNullOrWhiteSpace(errorMessage))
                {
                    _resultEntity.IsSuccess = true;
                }
                else
                {
                    _resultEntity.IsSuccess = false;
                    _resultEntity.Result = errorMessage;
                    _resultEntity.IsListResult = false;
                }

            }
            catch (Exception ex)
            {
                _resultEntity.IsSuccess = false;
                _resultEntity.Result = errorMessage;
                _resultEntity.IsListResult = false;
            }


            return Json(_resultEntity);
        }

        public JsonResult CancelRequestEquipment(string RequestGUID)
        {
            try
            {
                RequestEntityDAL objRequestsDAL = new RequestEntityDAL();
                RequestsEntity objRequestEntity = new RequestsEntity();
                objRequestEntity.RequestGUID = RequestGUID;

                objRequestEntity = objRequestsDAL.CancelRequestEquipment(objRequestEntity);

                return Json(objRequestEntity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}