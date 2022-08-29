using ERSBLL;
using ERSEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERS.Controllers
{
    public class PenaltyController : CommonController
    {
        PenaltyBLL _PenaltyBLL = null;
        PenaltyEntity _PenaltyEntity = null;

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PenaltyList()
        {
            PopulateDropdown();
            return View();
        }
        public ActionResult PenaltyAdd(string requestguid = "", string requestType = "")
        {
            try
            {
                _PenaltyBLL = new PenaltyBLL();
                _PenaltyEntity = new PenaltyEntity();
                _PenaltyEntity.RequestGUID = requestguid;
                _PenaltyEntity.RequestType = requestType;
                _PenaltyEntity = _PenaltyBLL.Get(_PenaltyEntity);
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
                if (_PenaltyEntity.IsSuccess)
                {
                    return PartialView(_PenaltyEntity);
                }
                else
                {
                    _resultEntity.IsSuccess = _PenaltyEntity.IsSuccess;
                    _resultEntity.Result = _PenaltyEntity.MessageList;
                    _resultEntity.IsListResult = false;

                    return Json(_resultEntity, JsonRequestBehavior.AllowGet);
                }
            }

        }
        public ActionResult PenaltyEdit(int PenaltyID = 0)
        {
            try
            {
                _exception = null;

                _PenaltyBLL = new PenaltyBLL();
                _PenaltyEntity = new PenaltyEntity();
                _PenaltyEntity.PenaltyID = PenaltyID;
                _PenaltyEntity = _PenaltyBLL.Get(_PenaltyEntity);
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
            _PenaltyBLL = null;

            if (_exception != null)
            {
                _resultEntity.IsListResult = false;
                _resultEntity.Result = _exception.Message;

                return Json(_resultEntity, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (_PenaltyEntity.IsSuccess)
                {
                    return PartialView(_PenaltyEntity);
                }
                else
                {
                    _resultEntity.IsSuccess = _PenaltyEntity.IsSuccess;
                    _resultEntity.Result = _PenaltyEntity.MessageList;
                    _resultEntity.IsListResult = false;

                    return Json(_resultEntity, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public ActionResult EquipmentUpdate()
        {
            return View();
        }

        public JsonResult GetPenaltyList(string sidx = "", string sord = "", int page = 1, int rows = 10
          , PenaltyListEntity objPenaltyListEntityFilter = null)
        {
            IEnumerable<PenaltyListEntity> objEquipmentListTableView = null;
            PenaltyListEntity objPenaltyListTableFilter = null;

            int totalRecords = 0;
            int noOfPages = 1;
            int rowStart = 1;

            try
            {
                rowStart = page > 1 ? page * rows - rows + 1 : rowStart;

                objPenaltyListTableFilter = new PenaltyListEntity();
                objPenaltyListTableFilter.RowStart = rowStart;
                objPenaltyListTableFilter.NoOfRecord = rows;
                objPenaltyListTableFilter.SortColumn = sidx == "" ? 0 : Convert.ToInt32(sidx);
                objPenaltyListTableFilter.SortDirection = sord;
                objPenaltyListTableFilter.RequestType = objPenaltyListEntityFilter.RequestType;
                objPenaltyListTableFilter.Requestor = objPenaltyListEntityFilter.Requestor;
                if (_objSystemUser.isLabPersonnel)
                    objPenaltyListTableFilter.RequestorID = 0;
                else
                    objPenaltyListTableFilter.RequestorID = _objSystemUser.ID;
                _PenaltyBLL = new PenaltyBLL();
                objEquipmentListTableView = _PenaltyBLL.GetFiltered(objPenaltyListTableFilter).ToList();

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

            _PenaltyBLL = null;

            objPenaltyListTableFilter = null;


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
        public JsonResult SavePenalty(PenaltyEntity objPenaltyEntity)
        {
            try
            {
                _PenaltyBLL = new PenaltyBLL();
                _PenaltyEntity = new PenaltyEntity();

                if (objPenaltyEntity.PenaltyID == 0)
                {
                    objPenaltyEntity.CreatedBy = _objSystemUser.ID;
                    _PenaltyEntity = _PenaltyBLL.Insert(objPenaltyEntity);
                }
                else
                {
                    objPenaltyEntity.ModifiedBy = _objSystemUser.ID;
                    _PenaltyEntity = _PenaltyBLL.Update(objPenaltyEntity);
                }

                if (_PenaltyEntity.IsSuccess)
                {
                    _resultEntity.IsSuccess = true;
                }
                else
                {
                    _resultEntity.IsSuccess = false;
                    _resultEntity.IsListResult = true;
                    _resultEntity.Result = _PenaltyEntity.MessageList;
                }
            }
            catch (Exception ex)
            {
                _resultEntity.IsSuccess = false;
                _resultEntity.Result = ex.Message;
            }

            _PenaltyBLL = null;

            return Json(_resultEntity);
        }
        public JsonResult GenerateQRCode(int EquipmentID)
        {
            string QRCode = string.Empty;
            PenaltyEntity result = new PenaltyEntity();
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
        private void PopulateDropdown()
        {
            List<SelectListItem> lstCategoryItemSelection = new List<SelectListItem>();
            SelectListItem CategoryItemSelection = null;
            string[] RequestTypes = {
                "Equipment"
                ,"Facility"
            };
            foreach (var i in RequestTypes)
            {
                CategoryItemSelection = new SelectListItem();
                CategoryItemSelection.Value = "" + i;
                CategoryItemSelection.Text = "" + i;
                lstCategoryItemSelection.Add(CategoryItemSelection);
                CategoryItemSelection = null;
            }
            ViewBag.requestTypeViewBag = lstCategoryItemSelection;
        }
    }
}
