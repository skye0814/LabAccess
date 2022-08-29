using ERSBLL;
using ERSEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERS.Controllers
{
    public class EquipmentCategoryController : CommonController
    {
        EquipmentCategoryBLL _EquipmentCategoryBLL = null;
        EquipmentCategoryEntity _EquipmentCategoryEntity = null;
        EquipmentItemBLL _EquipmentItemBLL = null;

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult EquipmentCategoryList()
        {
            if (!_objSystemUser.isLabPersonnel)
            {
                return new HttpStatusCodeResult(401);
            }

            return View();
        }
        public ActionResult EquipmentCategoryAdd()
        {
            return View();
        }
        public ActionResult EquipmentCategoryEdit(int EquipmentCategoryID = 0)
        {
            try
            {
                _exception = null;

                _EquipmentCategoryBLL = new EquipmentCategoryBLL();
                _EquipmentCategoryEntity = new EquipmentCategoryEntity();
                _EquipmentCategoryEntity.EquipmentCategoryID = EquipmentCategoryID;
                _EquipmentCategoryEntity = _EquipmentCategoryBLL.Get(_EquipmentCategoryEntity);
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
            _EquipmentCategoryBLL = null;

            if (_exception != null)
            {
                _resultEntity.IsListResult = false;
                _resultEntity.Result = _exception.Message;

                return Json(_resultEntity, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (_EquipmentCategoryEntity.IsSuccess)
                {
                    return PartialView(_EquipmentCategoryEntity);
                }
                else
                {
                    _resultEntity.IsSuccess = _EquipmentCategoryEntity.IsSuccess;
                    _resultEntity.Result = _EquipmentCategoryEntity.MessageList;
                    _resultEntity.IsListResult = false;

                    return Json(_resultEntity, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public ActionResult EquipmentUpdate()
        {
            return View();
        }

        public JsonResult GetEquipmentCategoryList(string sidx = "", string sord = "", int page = 1, int rows = 10
          , EquipmentCategoryListEntity objEquipmentCategoryListEntityFilter = null)
        {
            IEnumerable<EquipmentCategoryListEntity> objEquipmentListTableView = null;
            EquipmentCategoryListEntity objEquipmentCategoryListTableFilter = null;

            int totalRecords = 0;
            int noOfPages = 1;
            int rowStart = 1;

            try
            {
                rowStart = page > 1 ? page * rows - rows + 1 : rowStart;

                objEquipmentCategoryListTableFilter = new EquipmentCategoryListEntity();
                objEquipmentCategoryListTableFilter.RowStart = rowStart;
                objEquipmentCategoryListTableFilter.NoOfRecord = rows;
                objEquipmentCategoryListTableFilter.SortColumn = sidx == "" ? 0 : Convert.ToInt32(sidx);
                objEquipmentCategoryListTableFilter.SortDirection = sord;
                objEquipmentCategoryListTableFilter.Category = objEquipmentCategoryListEntityFilter.Category;
                objEquipmentCategoryListTableFilter.CategoryCode = objEquipmentCategoryListEntityFilter.CategoryCode;

                _EquipmentCategoryBLL = new EquipmentCategoryBLL();
                objEquipmentListTableView = _EquipmentCategoryBLL.GetFiltered(objEquipmentCategoryListTableFilter).ToList();

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

            _EquipmentCategoryBLL = null;

            objEquipmentCategoryListTableFilter = null;


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
        public JsonResult SaveEquipmentCategory(EquipmentCategoryEntity objEquipmentCategoryEntity)
        {
            try
            {
                _EquipmentCategoryBLL = new EquipmentCategoryBLL();
                _EquipmentCategoryEntity = new EquipmentCategoryEntity();

                if (objEquipmentCategoryEntity.EquipmentCategoryID == 0)
                {
                    objEquipmentCategoryEntity.CreatedBy = _objSystemUser.ID;
                    _EquipmentCategoryEntity = _EquipmentCategoryBLL.Insert(objEquipmentCategoryEntity);
                }
                else
                {
                    objEquipmentCategoryEntity.ModifiedBy = _objSystemUser.ID;
                    _EquipmentCategoryEntity = _EquipmentCategoryBLL.Update(objEquipmentCategoryEntity);
                }

                if (_EquipmentCategoryEntity.IsSuccess)
                {
                    _resultEntity.IsSuccess = true;
                }
                else
                {
                    _resultEntity.IsSuccess = false;
                    _resultEntity.IsListResult = true;
                    _resultEntity.Result = _EquipmentCategoryEntity.MessageList;
                }
            }
            catch (Exception ex)
            {
                _resultEntity.IsSuccess = false;
                _resultEntity.Result = ex.Message;
            }

            _EquipmentCategoryBLL = null;

            return Json(_resultEntity);
        }
        public JsonResult GenerateQRCode(int EquipmentID)
        {
            string QRCode = string.Empty;
            EquipmentCategoryEntity result = new EquipmentCategoryEntity();
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
    }
}
