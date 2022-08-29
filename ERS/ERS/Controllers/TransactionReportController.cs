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
    public class TransactionReportController : CommonController
    {
        TransactionReportBLL _TransactionReportBLL = null;
        // GET: TransactionReport
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GenerateTransaction()
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
            TransactionReportEntity objEquipmentInventory = new TransactionReportEntity();
            List<TransactionReportEntity> lstEquipmentInventory = new List<TransactionReportEntity>();
            ReportParameterEntity _objReportParameter = new ReportParameterEntity();
            try
            {
                _TransactionReportBLL = new TransactionReportBLL();
                objEquipmentInventory.DateFrom = DateFrom;
                objEquipmentInventory.DateTo = DateTo;
                lstEquipmentInventory = _TransactionReportBLL.GetFiltered(objEquipmentInventory).ToList();

                if (lstEquipmentInventory.Count() > 0)
                {
                    // Build the report entity object
                    _objReportParameter = new ReportParameterEntity();
                    _objReportParameter.DataSet = "dsTransaction"; // Data Set
                    _objReportParameter.Data = lstEquipmentInventory; // Record from Data
                    _objReportParameter.ReportPath = Server.MapPath("~/Reports/"); // Path folder where the RDLC file is located
                    _objReportParameter.ReportFilename = "TransactionReport.rdlc"; // Filename of the RDLC file


                    _objReportParameter.Filename = "Transaction Report" + "_" + DateTime.Now.ToString("MMddyyy"); // Filename of the dowload type

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

            _TransactionReportBLL = null;

            _objReportParameter = null;

            lstEquipmentInventory = null;

            return new JsonResult { Data = _resultEntity, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult GetTransactionList(string sidx = "", string sord = "", int page = 1, int rows = 10
          , TransactionReportEntity objTransactionReportEntityFilter = null)
        {
            IEnumerable<TransactionReportEntity> objEquipmentItemListTableView = null;
            TransactionReportEntity objEquipmentItemListTableFilter = null;

            int totalRecords = 0;
            int noOfPages = 1;
            int rowStart = 1;

            try
            {
                rowStart = page > 1 ? page * rows - rows + 1 : rowStart;

                objEquipmentItemListTableFilter = new TransactionReportEntity();
                objEquipmentItemListTableFilter.RowStart = rowStart;
                objEquipmentItemListTableFilter.NoOfRecord = rows;
                objEquipmentItemListTableFilter.SortColumn = sidx == "" ? 0 : Convert.ToInt32(sidx);
                objEquipmentItemListTableFilter.SortDirection = sord;
                objEquipmentItemListTableFilter.DateFrom = objTransactionReportEntityFilter.DateFrom == null ? "" : objTransactionReportEntityFilter.DateFrom;
                objEquipmentItemListTableFilter.DateTo = objTransactionReportEntityFilter.DateTo == null ? "" : objTransactionReportEntityFilter.DateTo;

                _TransactionReportBLL = new TransactionReportBLL();
                objEquipmentItemListTableView = _TransactionReportBLL.GetFiltered(objEquipmentItemListTableFilter).ToList();

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
            _TransactionReportBLL = null;

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