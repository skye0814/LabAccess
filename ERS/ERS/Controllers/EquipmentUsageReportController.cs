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
    public class EquipmentUsageReportController : CommonController
    {
        InventoryUsageReportBLL _InventoryUsageReportBLL = null;
        // GET: EquipmentUsageReport
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GenerateEquipmentUsage()
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
            EquipmentUsageReportEntity objEquipmentInventory = new EquipmentUsageReportEntity();
            List<EquipmentUsageReportEntity> lstEquipmentInventory = new List<EquipmentUsageReportEntity>();
            ReportParameterEntity _objReportParameter = new ReportParameterEntity();
            try
            {
                _InventoryUsageReportBLL = new InventoryUsageReportBLL();
                objEquipmentInventory.DateFrom = DateFrom;
                objEquipmentInventory.DateTo = DateTo;
                lstEquipmentInventory = _InventoryUsageReportBLL.GetFiltered(objEquipmentInventory).ToList();

                if (lstEquipmentInventory.Count() > 0)
                {
                    // Build the report entity object
                    _objReportParameter = new ReportParameterEntity();
                    _objReportParameter.DataSet = "dsInventoryUsage"; // Data Set
                    _objReportParameter.Data = lstEquipmentInventory; // Record from Data
                    _objReportParameter.ReportPath = Server.MapPath("~/Reports/"); // Path folder where the RDLC file is located
                    _objReportParameter.ReportFilename = "InventoryUsageReport.rdlc"; // Filename of the RDLC file


                    _objReportParameter.Filename = "Inventory Usage Report" + "_" + DateTime.Now.ToString("MMddyyy"); // Filename of the dowload type

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

            _InventoryUsageReportBLL = null;

            _objReportParameter = null;

            lstEquipmentInventory = null;

            return new JsonResult { Data = _resultEntity, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult GetEquipmentUsageList(string sidx = "", string sord = "", int page = 1, int rows = 10
          , EquipmentUsageReportEntity objEquipmentUsageReportEntityFilter = null)
        {
            IEnumerable<EquipmentUsageReportEntity> objEquipmentItemListTableView = null;
            EquipmentUsageReportEntity objEquipmentItemListTableFilter = null;

            int totalRecords = 0;
            int noOfPages = 1;
            int rowStart = 1;

            try
            {
                rowStart = page > 1 ? page * rows - rows + 1 : rowStart;

                objEquipmentItemListTableFilter = new EquipmentUsageReportEntity();
                objEquipmentItemListTableFilter.RowStart = rowStart;
                objEquipmentItemListTableFilter.NoOfRecord = rows;
                objEquipmentItemListTableFilter.SortColumn = sidx == "" ? 0 : Convert.ToInt32(sidx);
                objEquipmentItemListTableFilter.SortDirection = sord;
                objEquipmentItemListTableFilter.DateFrom = objEquipmentUsageReportEntityFilter.DateFrom == null ? "" : objEquipmentUsageReportEntityFilter.DateFrom;
                objEquipmentItemListTableFilter.DateTo = objEquipmentUsageReportEntityFilter.DateTo == null ? "" : objEquipmentUsageReportEntityFilter.DateTo;

                _InventoryUsageReportBLL = new InventoryUsageReportBLL();
                objEquipmentItemListTableView = _InventoryUsageReportBLL.GetFiltered(objEquipmentItemListTableFilter).ToList();

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
            _InventoryUsageReportBLL = null;

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