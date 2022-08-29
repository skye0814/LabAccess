using ERSBLL;
using ERSEntity;
using ERSEntity.Entity;
using ERSUtil;
using ERSDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERS.Controllers
{
    public class RequestFacilityController : CommonController
    {
        FacilityBLL _facilityBLL = null;
        RequestFacilityBLL _requestFacilityBLL = null;
        RequestFacilityEntity _requestFacilityEntity = null;

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RequestFacilityAdd()
        {
            PopulateDropdown();
            return View();
        }

        [HttpGet]
        public ActionResult RequestFacilityEdit(int FacilityRequestID = 0)
        {
            try
            {
                _exception = null;
                var FacilityDAL = new FacilityDAL();
                _requestFacilityBLL = new RequestFacilityBLL();
                _requestFacilityEntity = new RequestFacilityEntity();
                _requestFacilityEntity.FacilityRequestID = FacilityRequestID;
                _requestFacilityEntity = _requestFacilityBLL.Get(_requestFacilityEntity);
                if (_requestFacilityEntity.IsSuccess)
                {
                    PopulateDropdown();

                    if(_requestFacilityEntity.Schedule == "vacant")
                    {
                        ViewBag.Schedule = "Vacant time outside class schedules";
                    }
                    else
                    {
                        int scheduleID = Convert.ToInt32(_requestFacilityEntity.Schedule);
                        var FacilityListEntity = new FacilityListEntity()
                        {
                            FacilityID = Convert.ToInt32(_requestFacilityEntity.FacilityID),
                            RowStart = 1,
                            NoOfRecord = 100,
                            SortColumn = 0,
                            SortDirection = ""
                        };
                        ViewBag.Schedule = FacilityDAL.GetScheduleList(FacilityListEntity).Where(x => x.ScheduleID == scheduleID).FirstOrDefault().CourseName;
                    }
                }
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
            _requestFacilityBLL = null;

            if (_exception != null)
            {
                _resultEntity.IsListResult = false;
                _resultEntity.Result = _exception.Message;

                return Json(_resultEntity, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (_requestFacilityEntity.IsSuccess)
                {
                    return PartialView(_requestFacilityEntity);
                }
                else
                {
                    _resultEntity.IsSuccess = _requestFacilityEntity.IsSuccess;
                    _resultEntity.Result = _requestFacilityEntity.MessageList;
                    _resultEntity.IsListResult = false;

                    return Json(_resultEntity, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult RequestFacilityList()
        {
            if (_objSystemUser.isLabPersonnel && !_objSystemUser.isStudent)
            {
                return new HttpStatusCodeResult(401);
            }
            else
            {
                RequestFacilityDAL objDAL = new RequestFacilityDAL();
                var penalties = objDAL.GetAllActiveFacilityRequestPenaltyBySystemUserID(_objSystemUser.ID);


                ViewBag.Penalties = penalties;
            }
            return View();
        }

        private void PopulateDropdown()
        {
            List<FacilityEntity> lstFacilityEntity = new List<FacilityEntity>();
            SelectListItem FacilitySelection = new SelectListItem();
            List<SelectListItem> lstFacilitySelection = new List<SelectListItem>();

            _facilityBLL = new FacilityBLL();
            lstFacilityEntity = _facilityBLL.GetListOnlyAvailable().ToList();

            if (lstFacilityEntity.ToList().Count > 0)
            {
                foreach (FacilityEntity item in lstFacilityEntity)
                {
                    FacilitySelection = new SelectListItem();
                    FacilitySelection.Value = "" + item.FacilityID.ToString();
                    FacilitySelection.Text = "" + item.RoomNumber.ToString();
                    lstFacilitySelection.Add(FacilitySelection);
                    FacilitySelection = null;
                }
            }

            ViewBag.Facility = lstFacilitySelection;
            TempData["FacilityPopulateDropDown"] = lstFacilitySelection;
        }
        
        
        public ActionResult GetRequestFacilityByRequestDate(string StartTime, string EndTime)
        {
            RequestFacilityEntity objFacilityEntity = new RequestFacilityEntity();
            RequestFacilityDAL objFacilityDAL = new RequestFacilityDAL();
            var result = objFacilityDAL.SelectRequestFacilityGetByRequestDate(StartTime, EndTime);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult GetSchedulesByFacilityAndBorrowDate(int FacilityID, string BorrowDate)
        {
            var objFacilityDAL = new FacilityDAL();
            var objRequestFacilityDAL = new RequestFacilityDAL();

            // Convert BorrowDate to Day of the Week
            int ScheduleDay = 0;
            switch (Convert.ToDateTime(BorrowDate).ToString("dddd"))
            {
                case "Monday":
                    ScheduleDay = 0;
                    break;
                case "Tuesday":
                    ScheduleDay = 1;
                    break;
                case "Wednesday":
                    ScheduleDay = 2;
                    break;
                case "Thursday":
                    ScheduleDay = 3;
                    break;
                case "Friday":
                    ScheduleDay = 4;
                    break;
                case "Saturday":
                    ScheduleDay = 5;
                    break;
            }

            var FacilityListEntity = new FacilityListEntity()
            {
                FacilityID = FacilityID,
                RowStart = 1,
                NoOfRecord = 100,
                SortColumn = 0,
                SortDirection = ""
            };
            var ScheduleList = objFacilityDAL.GetScheduleList(FacilityListEntity).ToList();
            var ScheduleListByScheduleDay = ScheduleList.Where(x => x.ScheduleDay == ScheduleDay).ToList();

            return Json(ScheduleListByScheduleDay, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetConflictingRequestsforVacantTime(string StartTime, string EndTime, int FacilityID)
        {
            var FacilityDAL = new FacilityDAL();
            var RequestFacilityDAL = new RequestFacilityDAL();

            // Entities
            var FacilityListEntity = new FacilityListEntity()
            {
                FacilityID = FacilityID,
                RowStart = 1,
                NoOfRecord = 100,
                SortColumn = 0,
                SortDirection = ""
            };
            var ConflictingSchedules = new List<FacilityListEntity>(); // Add all conflicting schedule here

            // Get all class schedules by room first
            var getSchedulesByRoom = FacilityDAL.GetScheduleList(FacilityListEntity).ToList();

            // Get all class schedules by day
            int ScheduleDay = 0;
            switch (Convert.ToDateTime(StartTime).ToString("dddd"))
            {
                case "Monday":
                    ScheduleDay = 0;
                    break;
                case "Tuesday":
                    ScheduleDay = 1;
                    break;
                case "Wednesday":
                    ScheduleDay = 2;
                    break;
                case "Thursday":
                    ScheduleDay = 3;
                    break;
                case "Friday":
                    ScheduleDay = 4;
                    break;
                case "Saturday":
                    ScheduleDay = 5;
                    break;
            }
            var getSchedulesByDay = getSchedulesByRoom.Where(x => x.ScheduleDay == ScheduleDay).ToList();

            if (getSchedulesByDay.Count > 0)
            {
                foreach(var schedules in getSchedulesByDay)
                {
                    ConflictingSchedules.Add(schedules); // Automatically adds all class schedules to conflicting sched list
                }
            }

            // Get all existing request but only vacant schedules
            var getReservedVacantSchedules = RequestFacilityDAL.SelectRequestFacilityGetReservedVacantSchedulesByDate(StartTime, EndTime)
                .Where(x => x.FacilityID == FacilityID).ToList();
            if (getReservedVacantSchedules.Count > 0)
            {
                foreach(var reserved in getReservedVacantSchedules)
                {
                    ConflictingSchedules.Add(new FacilityListEntity()
                    {
                        CourseName = "Reserved Vacant Time",
                        TimeIn = Convert.ToDateTime(reserved.StartTime).ToString("hh:mm tt"),
                        TimeOut = Convert.ToDateTime(reserved.EndTime).ToString("hh:mm tt")
                    });
                }
            }

            return Json(ConflictingSchedules, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetReservedClassSchedulesByDate(string StartTime)
        {
            var RequestFacilityDAL = new RequestFacilityDAL();
            var result = RequestFacilityDAL.SelectRequestFacilityGetReservedClassSchedulesByDate(StartTime).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRequestFacilityList(string sidx = "", string sord = "", int page = 1, int rows = 10
          , RequestFacilityListEntity objListEntityFilter = null)
        {
            IEnumerable<RequestFacilityListEntity> objListTableView = null;
            RequestFacilityListEntity objListTableFilter = null;

            int totalRecords = 0;
            int noOfPages = 1;
            int rowStart = 1;

            try
            {
                rowStart = page > 1 ? page * rows - rows + 1 : rowStart;

                objListTableFilter = new RequestFacilityListEntity();
                objListTableFilter.RowStart = rowStart;
                objListTableFilter.NoOfRecord = rows;
                objListTableFilter.SortColumn = sidx == "" ? 0 : Convert.ToInt32(sidx);
                objListTableFilter.SortDirection = sord;
                objListTableFilter.RequestDate = objListEntityFilter.RequestDate;
                objListTableFilter.StartTime = objListEntityFilter.StartTime;
                objListTableFilter.Status = objListEntityFilter.Status;
                objListTableFilter.Remarks = objListEntityFilter.Remarks;
                objListTableFilter.FacilityID = objListEntityFilter.FacilityID;
                objListTableFilter.RequestFacilityGUID = objListEntityFilter.RequestFacilityGUID;
                objListTableFilter.EndTime = objListEntityFilter.EndTime;
                objListTableFilter.FacilityRequestorID = objListEntityFilter.FacilityRequestorID;
                objListTableFilter.StatusMode = objListEntityFilter.StatusMode;

                _requestFacilityBLL = new RequestFacilityBLL();
                objListTableView = _requestFacilityBLL.GetFiltered(objListTableFilter).ToList();

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

            _requestFacilityBLL = null;

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
        public JsonResult SaveRequestFacility(RequestFacilityEntity objRequestFacilityEntity)
        {
            try
            {
                var objReqFacilityDAL = new RequestFacilityDAL();
                var FacilityDAL = new FacilityDAL();
                _requestFacilityBLL = new RequestFacilityBLL();
                _requestFacilityEntity = new RequestFacilityEntity();
                List<string> errorList = new List<string>();

                if(objRequestFacilityEntity.FacilityRequestID == 0)
                {
                    if(objRequestFacilityEntity.Schedule == "vacant") // Dito papasok pag vacant room
                    {
                        // Check first if selected time falls between class schedules, dapat HINDI!
                        var _FacilityListEntity = new FacilityListEntity()
                        {
                            FacilityID = Convert.ToInt32(objRequestFacilityEntity.FacilityID),
                            RowStart = 1,
                            NoOfRecord = 100,
                            SortColumn = 0,
                            SortDirection = ""
                        };
                        var getScheduleByRoom = FacilityDAL.GetScheduleList(_FacilityListEntity).ToList();

                        // Get schedule of room by Day
                        int ScheduleDay = 0;
                        switch (Convert.ToDateTime(objRequestFacilityEntity.StartTime).ToString("dddd"))
                        {
                            case "Monday":
                                ScheduleDay = 0;
                                break;
                            case "Tuesday":
                                ScheduleDay = 1;
                                break;
                            case "Wednesday":
                                ScheduleDay = 2;
                                break;
                            case "Thursday":
                                ScheduleDay = 3;
                                break;
                            case "Friday":
                                ScheduleDay = 4;
                                break;
                            case "Saturday":
                                ScheduleDay = 5;
                                break;
                        }
                        DateTime vacantStart = Convert.ToDateTime(objRequestFacilityEntity.VacantStart);
                        DateTime vacantEnd = Convert.ToDateTime(objRequestFacilityEntity.VacantEnd);

                        var getScheduleByDay = getScheduleByRoom.Where(x => x.ScheduleDay == ScheduleDay)
                            .Where(y => (vacantStart >= Convert.ToDateTime(y.TimeIn) && vacantStart <= Convert.ToDateTime(y.TimeOut))
                            || (vacantEnd >= Convert.ToDateTime(y.TimeIn) && vacantEnd <= Convert.ToDateTime(y.TimeOut))).ToList();

                        objRequestFacilityEntity.EndTime = objRequestFacilityEntity.StartTime + " " + objRequestFacilityEntity.VacantEnd;
                        objRequestFacilityEntity.StartTime = objRequestFacilityEntity.StartTime + " " + objRequestFacilityEntity.VacantStart;

                        // Now check if selected time falls between another reserved vacant time
                        var getReservedVacantTime = objReqFacilityDAL.SelectRequestFacilityGetReservedVacantSchedulesByDate(objRequestFacilityEntity.StartTime, objRequestFacilityEntity.EndTime)
                            .Where(x => x.FacilityID == Convert.ToInt32(objRequestFacilityEntity.FacilityID)).ToList();

                        if (getScheduleByDay.Count != 0 || getReservedVacantTime.Count != 0)
                        {
                            _resultEntity.IsSuccess = false;
                            errorList.Add("Selected time falls between class schedules or reserved vacant time");
                        }
                        else
                        {
                            objRequestFacilityEntity.CreatedBy = _objSystemUser.ID;
                            objRequestFacilityEntity.FacilityRequestor = _objSystemUser.FirstName + " " + _objSystemUser.LastName;
                            objRequestFacilityEntity.FacilityRequestorID = _objSystemUser.ID;
                            
                            _requestFacilityEntity = _requestFacilityBLL.Insert(objRequestFacilityEntity);
                        }
                        
                    }
                    else // Dito papasok kapag yung may class ang pinili
                    {
                        var reservedClassSchedByDate = objReqFacilityDAL.SelectRequestFacilityGetReservedClassSchedulesByDate(objRequestFacilityEntity.StartTime)
                            .Where(x => x.Schedule == objRequestFacilityEntity.Schedule).ToList();

                        if (reservedClassSchedByDate.Count != 0)
                        {
                            _resultEntity.IsSuccess = false;
                            errorList.Add("Selected schedule falls between a reserved class schedule. Please reload the page and submit a new request again.");
                        }
                        else
                        {
                            var _FacilityListEntity = new FacilityListEntity()
                            {
                                FacilityID = 0,
                                RowStart = 1,
                                NoOfRecord = 100,
                                SortColumn = 0,
                                SortDirection = ""
                            };
                            var getScheduleByID = FacilityDAL.GetScheduleList(_FacilityListEntity).Where(x => x.ScheduleID == Convert.ToInt32(objRequestFacilityEntity.Schedule));
                            objRequestFacilityEntity.EndTime = objRequestFacilityEntity.StartTime + " " + getScheduleByID.FirstOrDefault().TimeOut;
                            objRequestFacilityEntity.StartTime = objRequestFacilityEntity.StartTime + " " + getScheduleByID.FirstOrDefault().TimeIn;

                            objRequestFacilityEntity.CreatedBy = _objSystemUser.ID;
                            objRequestFacilityEntity.FacilityRequestor = _objSystemUser.FirstName + " " + _objSystemUser.LastName;
                            objRequestFacilityEntity.FacilityRequestorID = _objSystemUser.ID;
                            _requestFacilityEntity = _requestFacilityBLL.Insert(objRequestFacilityEntity);
                        }
                    }
                }

                //if (objRequestFacilityEntity.FacilityRequestID == 0)
                //{
                //    var existingRequests = objReqFacilityDAL.SelectRequestFacilityGetByRequestDate(objRequestFacilityEntity.StartTime, objRequestFacilityEntity.EndTime);

                //    var conflictSchedule = existingRequests.Where(x => x.FacilityID == objRequestFacilityEntity.FacilityID).ToList();

                //    if (conflictSchedule.Count == 0)
                //    {
                //        objRequestFacilityEntity.CreatedBy = _objSystemUser.ID;
                //        objRequestFacilityEntity.FacilityRequestor = _objSystemUser.FirstName + " " + _objSystemUser.LastName;
                //        objRequestFacilityEntity.FacilityRequestorID = _objSystemUser.ID;
                //        _requestFacilityEntity = _requestFacilityBLL.Insert(objRequestFacilityEntity);
                //    }
                //    else
                //    {
                //        var roomLists = (List<SelectListItem>)TempData["FacilityPopulateDropDown"];
                //        var existingRoomName = roomLists.Where(x => x.Value == conflictSchedule.FirstOrDefault().FacilityID.ToString()).FirstOrDefault().Text;
                //        errorList.Add("The facility <strong>"+ existingRoomName + "</strong> has already reserved by another user.");
                //        errorList.Add("<br/><strong>This is due to a user who submitted the reservation first. Please submit a new request again, thank you.</strong>");
                //        _requestFacilityEntity.IsSuccess = false;
                //    }
                //}
                //else
                //{
                //    _requestFacilityEntity = _requestFacilityBLL.Update(objRequestFacilityEntity);
                //}

                if (_requestFacilityEntity.IsSuccess)
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

            _requestFacilityBLL = null;

            return Json(_resultEntity);
        }

        public JsonResult GenerateRequestFacilityQRCode(string RequestFacilityGUID)
        {
            _resultEntity = new ResultEntity();
            EncryptionUTIL encrypt = new EncryptionUTIL();
            string errorMessage = String.Empty;

            try
            {
                _resultEntity.Result = QRGenerate(encrypt.Encode(RequestFacilityGUID, ref errorMessage));

                if (String.IsNullOrEmpty(errorMessage) || string.IsNullOrWhiteSpace(errorMessage))
                {
                    _resultEntity.IsSuccess = true;
                }
                else
                {
                    _resultEntity.IsSuccess=false;
                    _resultEntity.Result=errorMessage;
                    _resultEntity.IsListResult = false;
                }

            }
            catch(Exception ex)
            {
                _resultEntity.IsSuccess = false;
                _resultEntity.Result = errorMessage;
                _resultEntity.IsListResult = false;
            }

            return Json(_resultEntity);
        }

        [HttpPost]
        public JsonResult CancelRequestFacility(int FacilityRequestID)
        {
            try
            {
                RequestFacilityDAL objRequestsDAL = new RequestFacilityDAL();
                RequestFacilityEntity objRequestEntity = new RequestFacilityEntity();
                objRequestEntity.FacilityRequestID = FacilityRequestID;

                objRequestEntity = objRequestsDAL.CancelRequestFacility(objRequestEntity);

                return Json(objRequestEntity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}