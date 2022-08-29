using ERSBLL;
using ERSEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERS.Controllers
{
    public class EquipmentController : CommonController
    {
        EquipmentBLL _EquipmentBLL = null;
        EquipmentEntity _EquipmentEntity = null;
        EquipmentItemBLL _EquipmentItemBLL = null;

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult EquipmentList()
        {
            return View();
        }
        public ActionResult EquipmentAdd()
        {
            return View();
        }
        public ActionResult EquipmentEdit(int EquipmentID = 0)
        {
            try
            {
                _exception = null;

                _EquipmentBLL = new EquipmentBLL();
                _EquipmentEntity = new EquipmentEntity();
                _EquipmentEntity.EquipmentID = EquipmentID;
                _EquipmentEntity = _EquipmentBLL.Get(_EquipmentEntity);
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
            _EquipmentBLL = null;

            if (_exception != null)
            {
                _resultEntity.IsListResult = false;
                _resultEntity.Result = _exception.Message;

                return Json(_resultEntity, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (_EquipmentEntity.IsSuccess)
                {
                    return PartialView(_EquipmentEntity);
                }
                else
                {
                    _resultEntity.IsSuccess = _EquipmentEntity.IsSuccess;
                    _resultEntity.Result = _EquipmentEntity.MessageList;
                    _resultEntity.IsListResult = false;

                    return Json(_resultEntity, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public ActionResult EquipmentUpdate()
        {
            return View();
        }
        public JsonResult GetEquipmentList(string sidx = "", string sord = "", int page = 1, int rows = 10
          , EquipmentListEntity objEquipmentListEntityFilter = null)
        {
            IEnumerable<EquipmentListEntity> objEquipmentListTableView = null;
            EquipmentListEntity objEquipmentListTableFilter = null;

            int totalRecords = 0;
            int noOfPages = 1;
            int rowStart = 1;

            try
            {
                rowStart = page > 1 ? page * rows - rows + 1 : rowStart;

                objEquipmentListTableFilter = new EquipmentListEntity();
                objEquipmentListTableFilter.RowStart = rowStart;
                objEquipmentListTableFilter.NoOfRecord = rows;
                objEquipmentListTableFilter.SortColumn = sidx == "" ? 0 : Convert.ToInt32(sidx);
                objEquipmentListTableFilter.SortDirection = sord;
                objEquipmentListTableFilter.EquipmentCode = objEquipmentListEntityFilter.EquipmentCode;
                objEquipmentListTableFilter.Description = objEquipmentListEntityFilter.Description;

                _EquipmentBLL = new EquipmentBLL();
                objEquipmentListTableView = _EquipmentBLL.GetFiltered(objEquipmentListTableFilter).ToList();

                if (objEquipmentListTableView != null && objEquipmentListTableView.Count() > 0)
                {
                    totalRecords = objEquipmentListTableView.FirstOrDefault().TotalRecord;
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

            _EquipmentBLL = null;

            objEquipmentListTableFilter = null;


            var jsonData = new
            {
                total = noOfPages,
                page,
                sort = sidx,
                records = totalRecords,
                rows = objEquipmentListTableView
            };

            objEquipmentListTableView = null;

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SaveEquipment(EquipmentEntity objEquipmentEntity)
        {
            try
            {
                _EquipmentBLL = new EquipmentBLL();
                _EquipmentEntity = new EquipmentEntity();

                if (objEquipmentEntity.EquipmentID == 0)
                {
                    _EquipmentEntity = _EquipmentBLL.Insert(objEquipmentEntity);
                }
                else
                {
                    _EquipmentEntity = _EquipmentBLL.Update(objEquipmentEntity);
                }

                if (_EquipmentEntity.IsSuccess)
                {
                    _resultEntity.IsSuccess = true;
                }
                else
                {
                    _resultEntity.IsSuccess = false;
                    _resultEntity.IsListResult = true;
                    _resultEntity.Result = _EquipmentEntity.MessageList;
                }
            }
            catch (Exception ex)
            {
                _resultEntity.IsSuccess = false;
                _resultEntity.Result = ex.Message;
            }

            _EquipmentBLL = null;

            return Json(_resultEntity);
        }
        public JsonResult GenerateQRCode(int EquipmentID)
        {
            string QRCode = string.Empty;
            EquipmentEntity result = new EquipmentEntity();
            try
            {

             
                string encoded = "";
                string error = "";
                QRCode = QRGenerate(encoded);


                if (result.IsSuccess)
                {
                    _resultEntity.IsSuccess = true;
                    _resultEntity.Result = QRCode;
                }
                else
                {
                    _resultEntity.IsSuccess = false;
                    _resultEntity.IsListResult = true;
                    _resultEntity.Result = result.MessageList;
                }
            }
            catch (Exception ex)
            {
                _resultEntity.IsSuccess = false;
                _resultEntity.IsListResult = false;
                _resultEntity.Result = ex.Message;
            }

            _resultEntity = null;

            return Json(_resultEntity);
        }
        public JsonResult GetEquipmentItemList(string sidx = "", string sord = "", int page = 1, int rows = 10
          , int EquipmentID = 0)
        {
            IEnumerable<EquipmentItemListEntity> objEquipmentItemListTableView = null;
            EquipmentItemListEntity objEquipmentItemListTableFilter = null;

            int totalRecords = 0;
            int noOfPages = 1;
            int rowStart = 1;

            try
            {
                rowStart = page > 1 ? page * rows - rows + 1 : rowStart;

                objEquipmentItemListTableFilter = new EquipmentItemListEntity();
                objEquipmentItemListTableFilter.RowStart = rowStart;
                objEquipmentItemListTableFilter.NoOfRecord = rows;
                objEquipmentItemListTableFilter.SortColumn = sidx == "" ? 0 : Convert.ToInt32(sidx);
                objEquipmentItemListTableFilter.SortDirection = sord;
                objEquipmentItemListTableFilter.EquipmentID = EquipmentID;

                _EquipmentItemBLL = new EquipmentItemBLL();
                objEquipmentItemListTableView = _EquipmentItemBLL.GetFiltered(objEquipmentItemListTableFilter).ToList();

                if (objEquipmentItemListTableView != null && objEquipmentItemListTableView.Count() > 0)
                {
                    totalRecords = objEquipmentItemListTableView.FirstOrDefault().TotalRecord;
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

            _EquipmentItemBLL = null;

            objEquipmentItemListTableFilter = null;


            var jsonData = new
            {
                total = noOfPages,
                page,
                sort = sidx,
                records = totalRecords,
                rows = objEquipmentItemListTableView
            };

            objEquipmentItemListTableView = null;

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }
}
