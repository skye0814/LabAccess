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
    public class ClaimReturnFacilityController : CommonController
    {
        ClaimReturnFacilityBLL _ClaimReturnFacilityBLL = null;
        RequestFacilityEntity _RequestFacilityEntity = null;
        ClaimReturnFacilityDAL claimReturnFacilityDAL = null;

        // GET: ClaimReturnFacility
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ClaimReturnFacilityList()
        {
            if (!_objSystemUser.isLabPersonnel)
            {
                return new HttpStatusCodeResult(401);
            }
            else
            {
                // Para ito sa auto cancel ng requests if lumagpas na sa end time
                claimReturnFacilityDAL = new ClaimReturnFacilityDAL();
                claimReturnFacilityDAL.CancelUnclaimedFacilityRequests();

                PopulateDropdown();
                return View();
            }
        }

        public ActionResult ClaimFacility()
        {
            return View();
        }

        public ActionResult FacilityQRCodeScanner()
        {
            return View();

        }
        public ActionResult ReturnFacility()
        {
            return View();

        }
        public ActionResult ViewFacilityRequest(string requestGUID)
        {
            try
            {
                RequestFacilityBLL _RequestFacilityBLL = new RequestFacilityBLL();
                _exception = null;
                _RequestFacilityEntity = new RequestFacilityEntity();
                _RequestFacilityEntity.RequestFacilityGUID = requestGUID;
                _RequestFacilityEntity = _RequestFacilityBLL.Get(_RequestFacilityEntity);
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
            _ClaimReturnFacilityBLL = null;

            if (_exception != null)
            {
                _resultEntity.IsListResult = false;
                _resultEntity.Result = _exception.Message;

                return Json(_resultEntity, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (_RequestFacilityEntity.IsSuccess)
                {

                    if (_RequestFacilityEntity.Schedule == "vacant")
                    {
                        ViewBag.Schedule = "Vacant time outside class schedules";
                        return PartialView(_RequestFacilityEntity);
                    }
                    else
                    {
                        var FacilityDAL = new FacilityDAL();
                        int scheduleID = Convert.ToInt32(_RequestFacilityEntity.Schedule);
                        var FacilityListEntity = new FacilityListEntity()
                        {
                            FacilityID = Convert.ToInt32(_RequestFacilityEntity.FacilityID),
                            RowStart = 1,
                            NoOfRecord = 100,
                            SortColumn = 0,
                            SortDirection = ""
                        };
                        ViewBag.Schedule = FacilityDAL.GetScheduleList(FacilityListEntity).Where(x => x.ScheduleID == scheduleID).FirstOrDefault().CourseName;
                        return PartialView(_RequestFacilityEntity);
                    }
                    
                }
                else
                {
                    _resultEntity.IsSuccess = _RequestFacilityEntity.IsSuccess;
                    _resultEntity.Result = _RequestFacilityEntity.MessageList;
                    _resultEntity.IsListResult = false;

                    return Json(_resultEntity, JsonRequestBehavior.AllowGet);
                }
            }
        }


        [HttpPost]
        public JsonResult SetRoom(string QRCode, int FacilityID = 0, string RoomNumber = "", string RequestFacilityGUID = "", string Status = "") //decode the QR Code then proceed to view or assigned claim/returned items
        {
            EncryptionUTIL encryptionUtil = new EncryptionUTIL();
            string errorMessage = String.Empty;
            string strRoomNumber = "";
            try
            {
                FacilityEntity _FacilityEntity = new FacilityEntity();
                FacilityBLL _FacilityBLL = new FacilityBLL();
                _RequestFacilityEntity = new RequestFacilityEntity();
                strRoomNumber = encryptionUtil.Decode(QRCode, ref errorMessage);
                if (strRoomNumber == "" || strRoomNumber == null)
                {
                    throw new Exception();
                }
                if (RoomNumber == strRoomNumber)
                {
                    _ClaimReturnFacilityBLL = new ClaimReturnFacilityBLL();
                    RequestFacilityBLL _RequestFacilityBLL = new RequestFacilityBLL();
                    _RequestFacilityEntity.FacilityID = FacilityID;
                    _RequestFacilityEntity.RoomNumber = RoomNumber;
                    _RequestFacilityEntity.RequestFacilityGUID = RequestFacilityGUID;
                    _RequestFacilityEntity.Status = Status;
                    _RequestFacilityEntity = _ClaimReturnFacilityBLL.RequestFacilityUpdate(_RequestFacilityEntity);
                    _RequestFacilityEntity.RequestFacilityGUID = RequestFacilityGUID;
                    _RequestFacilityEntity = _RequestFacilityBLL.Get(_RequestFacilityEntity);
                    if (_RequestFacilityEntity.ClaimedTime == "0")
                    {
                        _RequestFacilityEntity.Mode = "ClaimedTimedisableQR";
                    }
                    else if (_RequestFacilityEntity.ReturnedTime == "0")
                    {
                        _RequestFacilityEntity.Mode = "ReturnedTimedisableQR";
                    }
                }
                else
                {
                    _RequestFacilityEntity.IsSuccess = false;
                    _RequestFacilityEntity.MessageList = new List<string>();
                    _RequestFacilityEntity.MessageList.Add("Incorrect Room Key. You scanned Room number: " + strRoomNumber + ".");
                }

            }
            catch (Exception ex)
            {
                _exception = ex;
            }
            if (_exception != null)
            {
                _resultEntity.IsListResult = false;
                _resultEntity.Result = _exception.Message;

                return Json(_resultEntity, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (_RequestFacilityEntity.IsSuccess)
                {
                    return Json(_RequestFacilityEntity);
                }
                else
                {
                    _resultEntity.IsSuccess = _RequestFacilityEntity.IsSuccess;
                    _resultEntity.Result = _RequestFacilityEntity.MessageList;
                    _resultEntity.IsListResult = false;

                    return Json(_resultEntity, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public JsonResult decodeQRCode(string QRCode) //decode the QR Code then proceed to view or assigned claim/returned items
        {
            EncryptionUTIL encryptionUtil = new EncryptionUTIL();
            string errorMessage = String.Empty;
            string strRequestGUID = String.Empty;
            try
            {
                _RequestFacilityEntity = new RequestFacilityEntity();
                strRequestGUID = encryptionUtil.Decode(QRCode, ref errorMessage);
                if (strRequestGUID == "")
                {
                    throw new Exception();
                }
                RequestFacilityBLL _RequestFacilityBLL = new RequestFacilityBLL();
                _exception = null;
                _RequestFacilityEntity.RequestFacilityGUID = strRequestGUID;
                _RequestFacilityEntity = _RequestFacilityBLL.Get(_RequestFacilityEntity);

            }
            catch (Exception ex)
            {
                _exception = ex;
            }
            if (_exception != null)
            {
                _resultEntity.IsListResult = false;
                _resultEntity.Result = _exception.Message;

                return Json(_resultEntity, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (_RequestFacilityEntity.IsSuccess)
                {
                    return Json(_RequestFacilityEntity);
                }
                else
                {
                    _resultEntity.IsSuccess = _RequestFacilityEntity.IsSuccess;
                    _resultEntity.Result = _RequestFacilityEntity.MessageList;
                    _resultEntity.IsListResult = false;

                    return Json(_resultEntity, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public JsonResult UpdateClaimedRequest(string requestGUID, string mode) // Mark Requsets Status as Claimed or reset assigned FacilityItemID to the request.
        {
            try
            {
                _RequestFacilityEntity = new RequestFacilityEntity();
                _ClaimReturnFacilityBLL = new ClaimReturnFacilityBLL();
                _RequestFacilityEntity.RequestFacilityGUID = requestGUID;
                _RequestFacilityEntity.Mode = mode;
                _RequestFacilityEntity = _ClaimReturnFacilityBLL.UpdateClaimedRequest(_RequestFacilityEntity);
                return Json(_RequestFacilityEntity);
            }
            catch (Exception ex)
            {
                _resultEntity = new ResultEntity();
                _resultEntity.IsSuccess = false;
                _resultEntity.Result = ex.Message;
                return Json(_resultEntity);
            }
        }
        private void PopulateDropdown()
        {
            List<SelectListItem> lstCategoryItemSelection = new List<SelectListItem>();
            SelectListItem CategoryItemSelection = null;
            string[] statuses = {
                "Unclaimed"
                ,"Claimed"
                ,"Completed"
            };
            foreach (var i in statuses)
            {
                CategoryItemSelection = new SelectListItem();
                CategoryItemSelection.Value = "" + i;
                CategoryItemSelection.Text = "" + i;
                lstCategoryItemSelection.Add(CategoryItemSelection);
                CategoryItemSelection = null;
            }
            ViewBag.statusViewBag = lstCategoryItemSelection;
        }
    }
}