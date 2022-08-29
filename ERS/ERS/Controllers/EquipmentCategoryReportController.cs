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
    public class EquipmentCategoryReportController : CommonController
    {
        EquipmentCategoryReportBLL _equipmentCategoryReportBLL = null;
        // GET: EquipmentCategoryReport
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GenerateEquipmentCategory ()
        {
            if (!_objSystemUser.isLabPersonnel)
            {
                return new HttpStatusCodeResult(401);
            }
            return View();
        }

        public JsonResult ExportReport(String DateFrom = "", String DateTo = "")
        {

            string sessionId = "";
            EquipmentCategoryReportEntity objEquipmentCategory = new EquipmentCategoryReportEntity();
            List<EquipmentCategoryReportEntity> lstEquipmentCategory = new List<EquipmentCategoryReportEntity>();
            ReportParameterEntity _objReportParameter = new ReportParameterEntity();
            try
            {
                _equipmentCategoryReportBLL = new EquipmentCategoryReportBLL();
                objEquipmentCategory.DateFrom = DateFrom;
                objEquipmentCategory.DateTo = DateTo;
                lstEquipmentCategory = _equipmentCategoryReportBLL.GetFiltered(objEquipmentCategory).ToList();

                if (lstEquipmentCategory.Count() > 0)
                {
                    // Build the report entity object
                    _objReportParameter = new ReportParameterEntity();
                    _objReportParameter.DataSet = "dsEquipmentCategoryReportEntity"; // Data Set
                    _objReportParameter.Data = lstEquipmentCategory; // Record from Data
                    _objReportParameter.ReportPath = Server.MapPath("~/Reports/"); // Path folder where the RDLC file is located
                    _objReportParameter.ReportFilename = "EquipmentCategoryReport.rdlc"; // Filename of the RDLC file
                   

                    _objReportParameter.Filename = "Equipment Category Report" + "_" + DateTime.Now.ToString("MMddyyy"); // Filename of the dowload type

                    sessionId = Util.GetReportSessionId();
                    Session[sessionId] = _objReportParameter; // Assign the report entity object on a session


                    _resultEntity.IsSuccess = true;
                    _resultEntity.Result = sessionId;
                }
                else
                {
                    _resultEntity.IsSuccess = false;
                    _resultEntity.Result = "No Records to extract.";
                }
            }
            catch (Exception ex)
            {
                _resultEntity.IsSuccess = false;
                _resultEntity.IsListResult = false;
                _resultEntity.Result = ex.Message;
            }

            _equipmentCategoryReportBLL = null;

            _objReportParameter = null;

            lstEquipmentCategory = null;

            return new JsonResult { Data = _resultEntity, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult GetEquipmentCategoryUsageList(string sidx = "", string sord = "", int page = 1, int rows = 10
          , EquipmentCategoryReportEntity objEquipmentCategoryReportEntityFilter = null)
        {
            IEnumerable<EquipmentCategoryReportEntity> objEquipmentItemListTableView = null;
            EquipmentCategoryReportEntity objEquipmentItemListTableFilter = null;

            int totalRecords = 0;
            int noOfPages = 1;
            int rowStart = 1;

            try
            {
                rowStart = page > 1 ? page * rows - rows + 1 : rowStart;

                objEquipmentItemListTableFilter = new EquipmentCategoryReportEntity();
                objEquipmentItemListTableFilter.RowStart = rowStart;
                objEquipmentItemListTableFilter.NoOfRecord = rows;
                objEquipmentItemListTableFilter.SortColumn = sidx == "" ? 0 : Convert.ToInt32(sidx);
                objEquipmentItemListTableFilter.SortDirection = sord;
                objEquipmentItemListTableFilter.DateFrom = objEquipmentCategoryReportEntityFilter.DateFrom == null ? "" : objEquipmentCategoryReportEntityFilter.DateFrom;
                objEquipmentItemListTableFilter.DateTo = objEquipmentCategoryReportEntityFilter.DateTo == null ? "" : objEquipmentCategoryReportEntityFilter.DateTo;

                _equipmentCategoryReportBLL = new EquipmentCategoryReportBLL();
                objEquipmentItemListTableView = _equipmentCategoryReportBLL.GetFiltered(objEquipmentItemListTableFilter).ToList();

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
            _equipmentCategoryReportBLL = null;

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