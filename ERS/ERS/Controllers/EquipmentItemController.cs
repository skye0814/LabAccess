using ERSBLL;
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
    public class EquipmentItemController : CommonController
    {
        EquipmentItemBLL _EquipmentItemBLL = null;
        EquipmentItemEntity _EquipmentItemEntity = null;
        EquipmentCategoryBLL _EquipmentCategoryBLL = null;

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult EquipmentItemList()
        {
            if (!_objSystemUser.isLabPersonnel)
            {
                return new HttpStatusCodeResult(401);
            }
            return View();
        }
        public ActionResult EquipmentItemAdd()
        {
            PopulateDropdown();
            return View();
        }
        public ActionResult EquipmentItemEdit(int EquipmentItemID = 0)
        {
            PopulateDropdown();
            try
            {
                _exception = null;

                _EquipmentItemBLL = new EquipmentItemBLL();
                _EquipmentItemEntity = new EquipmentItemEntity();
                _EquipmentItemEntity.EquipmentItemID = EquipmentItemID;
                _EquipmentItemEntity = _EquipmentItemBLL.Get(_EquipmentItemEntity);
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
            _EquipmentItemBLL = null;

            if (_exception != null)
            {
                _resultEntity.IsListResult = false;
                _resultEntity.Result = _exception.Message;

                return Json(_resultEntity, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (_EquipmentItemEntity.IsSuccess)
                {
                    return PartialView(_EquipmentItemEntity);
                }
                else
                {
                    _resultEntity.IsSuccess = _EquipmentItemEntity.IsSuccess;
                    _resultEntity.Result = _EquipmentItemEntity.MessageList;
                    _resultEntity.IsListResult = false;

                    return Json(_resultEntity, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult GetEquipmentItemList(string sidx = "", string sord = "", int page = 1, int rows = 10
          , EquipmentItemListEntity objEquipmentItemListEntityFilter = null)
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
                objEquipmentItemListTableFilter.EquipmentItemCode = objEquipmentItemListEntityFilter.EquipmentItemCode;
                objEquipmentItemListTableFilter.Category = objEquipmentItemListEntityFilter.Category;

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
        [HttpPost]
        public JsonResult SaveEquipmentItem(EquipmentItemEntity objEquipmentItemEntity)
        {
            try
            {
                _EquipmentItemBLL = new EquipmentItemBLL();
                _EquipmentItemEntity = null;
                _EquipmentItemEntity = new EquipmentItemEntity();

                if (objEquipmentItemEntity.EquipmentItemID == 0)
                {
                    objEquipmentItemEntity.CreatedBy = _objSystemUser.ID;
                    _EquipmentItemEntity = _EquipmentItemBLL.Insert(objEquipmentItemEntity);
                }
                else
                {
                    objEquipmentItemEntity.ModifiedBy = _objSystemUser.ID;
                    if (objEquipmentItemEntity.Status == "Missing")
                        objEquipmentItemEntity.isActive = false;
                    _EquipmentItemEntity = _EquipmentItemBLL.Update(objEquipmentItemEntity);
                }

                if (_EquipmentItemEntity.IsSuccess)
                {
                    _resultEntity.IsSuccess = true;
                }
                else
                {
                    _resultEntity.IsSuccess = false;
                    _resultEntity.IsListResult = true;
                    _resultEntity.Result = _EquipmentItemEntity.MessageList;
                }
            }
            catch (Exception ex)
            {
                _resultEntity.IsSuccess = false;
                _resultEntity.Result = ex.Message;
            }

            _EquipmentItemBLL = null;

            return Json(_resultEntity);
        }
        
        private void PopulateDropdown()
        {
            List<EquipmentCategoryEntity> lstEquipmentCategoryEntity = new List<EquipmentCategoryEntity>();
            SelectListItem CategoryItemSelection = new SelectListItem();
            List<SelectListItem> lstCategoryItemSelection = new List<SelectListItem>();

            _EquipmentCategoryBLL = new EquipmentCategoryBLL();
            lstEquipmentCategoryEntity = _EquipmentCategoryBLL.GetList().ToList();

            if (lstEquipmentCategoryEntity.ToList().Count > 0)
            {
                foreach (EquipmentCategoryEntity item in lstEquipmentCategoryEntity)
                {
                    CategoryItemSelection = new SelectListItem();
                    CategoryItemSelection.Value = "" + item.Category.ToString();
                    CategoryItemSelection.Text = "(" + item.CategoryCode + ") " + item.Category;
                    lstCategoryItemSelection.Add(CategoryItemSelection);
                    CategoryItemSelection = null;
                }
            }

            ViewBag.Category = lstCategoryItemSelection;
        }
        public JsonResult GenerateEquipmentItemQRCode(string EquipmentItemCode)
        {
            _resultEntity = new ResultEntity();
            EncryptionUTIL encrypt = new EncryptionUTIL();
            string errorMessage = String.Empty;
            try
            {
                _resultEntity.Result = QRGenerate(encrypt.Encode(EquipmentItemCode, ref errorMessage));

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

    }
}
