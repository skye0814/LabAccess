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
    public class ClaimReturnEquipmentController : CommonController
    {
        ClaimReturnEquipmentBLL _ClaimReturnEquipmentBLL = null;
        ClaimReturnEquipmentEntity _ClaimReturnEquipmentEntity = null;
        ClaimReturnEquipmentDAL claimReturnEquipmentDAL = null;

        // GET: ClaimReturnEquipment
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ClaimReturnEquipmentList()
        {
            if (!_objSystemUser.isLabPersonnel)
            {
                return new HttpStatusCodeResult(401);
            }
            else
            {
                // Para ito sa auto cancel ng requests if lumagpas na sa end time
                claimReturnEquipmentDAL = new ClaimReturnEquipmentDAL();
                claimReturnEquipmentDAL.CancelUnclaimedEquipmentRequests();

                PopulateDropdown();
                return View();
            }
        }

        public ActionResult ClaimEquipment()
        {
            return View();
        }

        public ActionResult EquipmentQRCodeScanner()
        {
            return View();
        }
        public ActionResult ItemQRCodeScanner(int RequestEquipmentItemID = 0)
        {
            return View();
        }

        public ActionResult ReturnEquipment()
        {
            return View();

        }
        public ActionResult ViewEquipmentRequest(string requestGUID)
        {
            try
            {
                ClaimReturnEquipmentEntity duplicateCheck = new ClaimReturnEquipmentEntity();
                ClaimReturnEquipmentBLL duplicateCheckBLL = new ClaimReturnEquipmentBLL();
                RequestDetailsDAL objDAL = null;
                List<RequestDetailsEntity> objRequestDetailsList = null;
                _exception = null;

                _ClaimReturnEquipmentBLL = new ClaimReturnEquipmentBLL();
                _ClaimReturnEquipmentEntity = new ClaimReturnEquipmentEntity();
                duplicateCheck.RequestGUID = requestGUID;
                duplicateCheck = duplicateCheckBLL.Get(duplicateCheck);
                if (!duplicateCheck.IsSuccess)
                {
                    objDAL = new RequestDetailsDAL();
                    objRequestDetailsList = objDAL.SelectRequestDetailsByRequestGUID(requestGUID);
                    foreach (var objRequestDetails in objRequestDetailsList)
                    {
                        _ClaimReturnEquipmentEntity = _ClaimReturnEquipmentBLL.UpdateDatabase(objRequestDetails);
                    }
                }
                _ClaimReturnEquipmentEntity.RequestGUID = requestGUID;
                _ClaimReturnEquipmentEntity = _ClaimReturnEquipmentBLL.Get(_ClaimReturnEquipmentEntity);

            }
            catch (Exception ex)
            {
                _exception = ex;
            }
            _ClaimReturnEquipmentBLL = null;

            if (_exception != null)
            {
                _resultEntity.IsListResult = false;
                _resultEntity.Result = _exception.Message;

                return Json(_resultEntity, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (_ClaimReturnEquipmentEntity.IsSuccess)
                {
                    return PartialView(_ClaimReturnEquipmentEntity);
                }
                else
                {
                    _resultEntity.IsSuccess = _ClaimReturnEquipmentEntity.IsSuccess;
                    _resultEntity.Result = _ClaimReturnEquipmentEntity.MessageList;
                    _resultEntity.IsListResult = false;

                    return Json(_resultEntity, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public JsonResult GetRequestEquipmentItemList(string sidx = "", string sord = "", int page = 1, int rows = 10
          , ClaimReturnEquipmentListEntity objRequestEquipmentItemListEntityFilter = null)
        {
            IEnumerable<ClaimReturnEquipmentListEntity> objListTableView = null;
            ClaimReturnEquipmentListEntity objRequestEquipmentItemListTableFilter = null;

            int totalRecords = 0;
            int noOfPages = 1;
            int rowStart = 1;

            try
            {
                rowStart = page > 1 ? page * rows - rows + 1 : rowStart;

                objRequestEquipmentItemListTableFilter = new ClaimReturnEquipmentListEntity();
                objRequestEquipmentItemListTableFilter.RowStart = rowStart;
                objRequestEquipmentItemListTableFilter.NoOfRecord = rows;
                objRequestEquipmentItemListTableFilter.SortColumn = sidx == "" ? 0 : Convert.ToInt32(sidx);
                objRequestEquipmentItemListTableFilter.SortDirection = sord;
                objRequestEquipmentItemListTableFilter.RequestGUID = objRequestEquipmentItemListEntityFilter.RequestGUID;

                _ClaimReturnEquipmentBLL = new ClaimReturnEquipmentBLL();
                objListTableView = _ClaimReturnEquipmentBLL.GetFiltered(objRequestEquipmentItemListTableFilter).ToList();

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

            _ClaimReturnEquipmentBLL = null;

            objRequestEquipmentItemListTableFilter = null;


            var jsonData = new
            {
                total = noOfPages,
                page,
                sort = sidx,
                records = totalRecords,
                rows = objListTableView
            };

            objRequestEquipmentItemListTableFilter = null;

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult decodeQRCode(string QRCode, int requestEquipmentItemID = 0) //decode the QR Code then proceed to view or assigned claim/returned items
        {
            EncryptionUTIL encryptionUtil = new EncryptionUTIL();
            string errorMessage = String.Empty;
            string stringDecodedQR = String.Empty;
            try
            {
                stringDecodedQR = encryptionUtil.Decode(QRCode, ref errorMessage);
                if (stringDecodedQR == null || stringDecodedQR == "")
                {
                    throw new Exception();
                }
                _ClaimReturnEquipmentEntity = new ClaimReturnEquipmentEntity();
                _ClaimReturnEquipmentBLL = new ClaimReturnEquipmentBLL();
                if (requestEquipmentItemID == 0)
                {
                    _ClaimReturnEquipmentEntity.RequestGUID = stringDecodedQR;
                    _ClaimReturnEquipmentEntity = _ClaimReturnEquipmentBLL.CheckRequestGUID(_ClaimReturnEquipmentEntity);
                }
                else
                {
                    _ClaimReturnEquipmentEntity.RequestEquipmentItemID = requestEquipmentItemID;
                    _ClaimReturnEquipmentEntity = _ClaimReturnEquipmentBLL.GetItem(_ClaimReturnEquipmentEntity);
                    _ClaimReturnEquipmentEntity.EquipmentItemCode = stringDecodedQR;
                    _ClaimReturnEquipmentEntity = _ClaimReturnEquipmentBLL.RequestEquipmentItemUpdate(_ClaimReturnEquipmentEntity);
                    if (_ClaimReturnEquipmentEntity.IsSuccess) // Get Request to be displayed again
                    {
                        _ClaimReturnEquipmentEntity.RequestEquipmentItemID = requestEquipmentItemID;
                        _ClaimReturnEquipmentEntity = _ClaimReturnEquipmentBLL.Get(_ClaimReturnEquipmentEntity);
                    }
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
                if (_ClaimReturnEquipmentEntity.IsSuccess)
                {
                    return Json(_ClaimReturnEquipmentEntity);
                }
                else
                {
                    _resultEntity.IsSuccess = _ClaimReturnEquipmentEntity.IsSuccess;
                    _resultEntity.Result = _ClaimReturnEquipmentEntity.MessageList;
                    _resultEntity.IsListResult = false;

                    return Json(_resultEntity, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult UpdateClaimedItems (string requestGUID, string mode) // Mark Requsets Status as Claimed or reset assigned EquipmentItemID to the request.
        {
            try
            {
                _ClaimReturnEquipmentEntity = new ClaimReturnEquipmentEntity();
                _ClaimReturnEquipmentBLL = new ClaimReturnEquipmentBLL();
                _ClaimReturnEquipmentEntity.RequestGUID = requestGUID;
                _ClaimReturnEquipmentEntity.mode = mode;
                _ClaimReturnEquipmentEntity = _ClaimReturnEquipmentBLL.UpdateClaimedItems(_ClaimReturnEquipmentEntity);
                return Json(_ClaimReturnEquipmentEntity);
            }
            catch (Exception ex)
            {
                _resultEntity = new ResultEntity();
                _resultEntity.IsSuccess = false;
                _resultEntity.Result = ex.Message;
                return Json(_resultEntity);
            }
        }

        public JsonResult CheckClaimedItems(string requestGUID) // Mark Requsets Status as Claimed or reset assigned EquipmentItemID to the request.
        {
            try
            {
                _ClaimReturnEquipmentEntity = new ClaimReturnEquipmentEntity();
                _ClaimReturnEquipmentBLL = new ClaimReturnEquipmentBLL();
                _ClaimReturnEquipmentEntity.RequestGUID = requestGUID;
                _ClaimReturnEquipmentEntity = _ClaimReturnEquipmentBLL.CheckClaimedItems(_ClaimReturnEquipmentEntity);
                return Json(_ClaimReturnEquipmentEntity);
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