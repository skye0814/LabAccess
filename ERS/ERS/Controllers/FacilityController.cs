using ERSBLL;
using ERSDAL;
using ERSEntity;
using ERSEntity.Entity;
using ERSUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERS.Controllers
{
    public class FacilityController : CommonController
    {
        FacilityBLL _FacilityBLL = null;
        FacilityEntity _FacilityEntity = null;

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FacilityList()
        {
            if (!_objSystemUser.isLabPersonnel)
            {
                return new HttpStatusCodeResult(401);
            }

            return View();
        }
        public ActionResult FacilityAdd()
        {
            return View();
        }
        public ActionResult FacilityEdit(int FacilityID = 0)
        {
            PopulateDropdown();
            try
            {
                _exception = null;

                _FacilityBLL = new FacilityBLL();
                _FacilityEntity = new FacilityEntity();
                _FacilityEntity.FacilityID = FacilityID;
                _FacilityEntity = _FacilityBLL.Get(_FacilityEntity);
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
            _FacilityBLL = null;

            if (_exception != null)
            {
                _resultEntity.IsListResult = false;
                _resultEntity.Result = _exception.Message;

                return Json(_resultEntity, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (_FacilityEntity.IsSuccess)
                {
                    return PartialView(_FacilityEntity);
                }
                else
                {
                    _resultEntity.IsSuccess = _FacilityEntity.IsSuccess;
                    _resultEntity.Result = _FacilityEntity.MessageList;
                    _resultEntity.IsListResult = false;

                    return Json(_resultEntity, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult GetFacilityList(string sidx = "", string sord = "", int page = 1, int rows = 10
          , FacilityListEntity objFacilityListEntityFilter = null)
        {
            IEnumerable<FacilityListEntity> objFacilityListTableView = null;
            FacilityListEntity objFacilityListTableFilter = null;

            int totalRecords = 0;
            int noOfPages = 1;
            int rowStart = 1;

            try
            {
                rowStart = page > 1 ? page * rows - rows + 1 : rowStart;

                objFacilityListTableFilter = new FacilityListEntity();
                objFacilityListTableFilter.RowStart = rowStart;
                objFacilityListTableFilter.NoOfRecord = rows;
                objFacilityListTableFilter.SortColumn = sidx == "" ? 0 : Convert.ToInt32(sidx);
                objFacilityListTableFilter.SortDirection = sord;
                objFacilityListTableFilter.RoomNumber = objFacilityListEntityFilter.RoomNumber;
                objFacilityListTableFilter.RoomType = objFacilityListEntityFilter.RoomType;

                _FacilityBLL = new FacilityBLL();
                objFacilityListTableView = _FacilityBLL.GetFiltered(objFacilityListTableFilter).ToList();

                if (objFacilityListTableView != null && objFacilityListTableView.Count() > 0)
                {
                    totalRecords = objFacilityListTableView.FirstOrDefault().TotalRecord;
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

            _FacilityBLL = null;

            objFacilityListTableFilter = null;


            var jsonData = new
            {
                total = noOfPages,
                page,
                sort = sidx,
                records = totalRecords,
                rows = objFacilityListTableView
            };

            objFacilityListTableView = null;

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveFacility(FacilityEntity objFacilityEntity)
        {
            try
            {
                _FacilityBLL = new FacilityBLL();
                _FacilityEntity = new FacilityEntity();

                if (objFacilityEntity.FacilityID == 0)
                {
                    objFacilityEntity.CreatedBy = _objSystemUser.ID;
                    _FacilityEntity = _FacilityBLL.Insert(objFacilityEntity);
                }
                else
                {
                    objFacilityEntity.ModifiedBy = _objSystemUser.ID;
                    _FacilityEntity = _FacilityBLL.Update(objFacilityEntity);
                }

                if (_FacilityEntity.IsSuccess)
                {
                    _resultEntity.IsSuccess = true;
                }
                else
                {
                    _resultEntity.IsSuccess = false;
                    _resultEntity.IsListResult = true;
                    _resultEntity.Result = _FacilityEntity.MessageList;
                }
            }
            catch (Exception ex)
            {
                _resultEntity.IsSuccess = false;
                _resultEntity.Result = ex.Message;
            }

            _FacilityBLL = null;

            return Json(_resultEntity);
        }

        public JsonResult GenerateFacilityQRCode(string RoomNumber)
        {
            _resultEntity = new ResultEntity();
            EncryptionUTIL encrypt = new EncryptionUTIL();
            string errorMessage = String.Empty;
            try
            {
                _resultEntity.Result = QRGenerate(encrypt.Encode(RoomNumber.ToString(), ref errorMessage));

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
                _resultEntity.Result = ex;
                _resultEntity.IsListResult = false;
            }


            return Json(_resultEntity);
        }
        private void PopulateDropdown()
        {
            List<SelectListItem> Days = new List<SelectListItem>();
            SelectListItem Day = null;
            int value = 0;
            string[] arrDay = {
                "Monday"
                ,"Tuesday"
                ,"Wednesday"
                ,"Thursday"
                ,"Friday"
                ,"Saturday"
            };
            foreach (var i in arrDay)
            {
                Day = new SelectListItem();
                Day.Value = "" + value;
                Day.Text = "" + i;
                Days.Add(Day);
                Day = null;
                value++;
            }
            ViewBag.daysViewBag = Days;
        }
        public JsonResult AddSchedule(int FacilityID, int ScheduleDay, string TimeIn, string TimeOut, string SubjectCode, string CourseName)
        {
            try
            {
                var objFacilityDAL = new FacilityDAL();
                _FacilityBLL = new FacilityBLL();
                _FacilityEntity = new FacilityEntity();

                var _FacilityListEntity = new FacilityListEntity()
                {
                    FacilityID = FacilityID,
                    RowStart = 1,
                    NoOfRecord = 100,
                    SortColumn = 0,
                    SortDirection = ""
                };

                _FacilityEntity.FacilityID = FacilityID;
                _FacilityEntity.ScheduleDay = ScheduleDay;
                _FacilityEntity.TimeIn = TimeIn;
                _FacilityEntity.TimeOut = TimeOut;
                _FacilityEntity.SubjectCode = SubjectCode;
                _FacilityEntity.CourseName = CourseName;

                var date = DateTime.Now.ToString("");
                // Check if there is a conflicting schedule
                var scheduleLists = objFacilityDAL.GetScheduleList(_FacilityListEntity).ToList();
                var scheduleListsByDay = scheduleLists.Where(x => x.ScheduleDay == _FacilityEntity.ScheduleDay).ToList();
                var conflictingSchedule = scheduleListsByDay.Where(y => (Convert.ToDateTime(_FacilityEntity.TimeIn) >= Convert.ToDateTime(y.TimeIn) && Convert.ToDateTime(_FacilityEntity.TimeIn) <= Convert.ToDateTime(y.TimeOut))
                    || (Convert.ToDateTime(_FacilityEntity.TimeOut) >= Convert.ToDateTime(y.TimeIn) && Convert.ToDateTime(_FacilityEntity.TimeOut) <= Convert.ToDateTime(y.TimeOut))).ToList();

                if (conflictingSchedule.Count == 0)
                {
                    _FacilityEntity = _FacilityBLL.InsertSchedule(_FacilityEntity);
                }
                else{
                    _FacilityEntity.IsSuccess = false;
                    _FacilityEntity.MessageList = new List<string>();
                    _FacilityEntity.MessageList.Add("Failed to add schedule. Please adjust the timespan before or after the conflicting schedules.");
                }

                
                if (_FacilityEntity.IsSuccess)
                {
                    _resultEntity.IsSuccess = true;
                }
                else
                {
                    _resultEntity.IsSuccess = false;
                    _resultEntity.IsListResult = true;
                    _resultEntity.Result = _FacilityEntity.MessageList;
                }
            }
            catch (Exception ex)
            {
                _resultEntity.IsSuccess = false;
                _resultEntity.Result = ex.Message;
            }

            _FacilityBLL = null;

            return Json(_resultEntity);
        }
        public JsonResult DeleteSchedule(int ScheduleID)
        {
            try
            {
                _FacilityBLL = new FacilityBLL();
                _FacilityEntity = new FacilityEntity();
                _FacilityEntity.ScheduleID = ScheduleID;
                _FacilityEntity = _FacilityBLL.DeleteSchedule(_FacilityEntity);

                if (_FacilityEntity.IsSuccess)
                {
                    _resultEntity.IsSuccess = true;
                }
                else
                {
                    _resultEntity.IsSuccess = false;
                    _resultEntity.IsListResult = true;
                    _resultEntity.Result = _FacilityEntity.MessageList;
                }
            }
            catch (Exception ex)
            {
                _resultEntity.IsSuccess = false;
                _resultEntity.Result = ex.Message;
            }

            _FacilityBLL = null;

            return Json(_resultEntity);
        }
        public JsonResult GetScheduleList(string sidx = "", string sord = "", int page = 1, int rows = 10
          , FacilityListEntity objFacilityListEntityFilter = null)
        {
            IEnumerable<FacilityListEntity> objFacilityListTableView = null;
            FacilityListEntity objTableFilter = null;

            int totalRecords = 0;
            int noOfPages = 1;
            int rowStart = 1;

            try
            {
                rowStart = page > 1 ? page * rows - rows + 1 : rowStart;

                objTableFilter = new FacilityListEntity();
                objTableFilter.RowStart = rowStart;
                objTableFilter.NoOfRecord = rows;
                objTableFilter.SortColumn = sidx == "" ? 0 : Convert.ToInt32(sidx);
                objTableFilter.SortDirection = sord;
                objTableFilter.FacilityID = objFacilityListEntityFilter.FacilityID;

                _FacilityBLL = new FacilityBLL();
                objFacilityListTableView = _FacilityBLL.GetScheduleList(objTableFilter).ToList();

                if (objFacilityListTableView != null && objFacilityListTableView.Count() > 0)
                {
                    totalRecords = objFacilityListTableView.FirstOrDefault().TotalRecord;
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

            _FacilityBLL = null;

            objTableFilter = null;


            var jsonData = new
            {
                total = noOfPages,
                page,
                sort = sidx,
                records = totalRecords,
                rows = objFacilityListTableView
            };

            objFacilityListTableView = null;

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }
}
