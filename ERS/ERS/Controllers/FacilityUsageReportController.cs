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
    public class FacilityUsageReportController : CommonController
    {
        FacilityUsageReportBLL _FacilityUsageReportBLL = null;
        // GET: FacilityUsageReport
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GenerateFacilityUsage()
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
            FacilityUsageReportEntity objFacility = new FacilityUsageReportEntity();
            List<FacilityUsageReportEntity> lstFacilityFacility = new List<FacilityUsageReportEntity>();
            ReportParameterEntity _objReportParameter = new ReportParameterEntity();
            try
            {
                _FacilityUsageReportBLL = new FacilityUsageReportBLL();
                objFacility.DateFrom = DateFrom;
                objFacility.DateTo = DateTo;
                lstFacilityFacility = _FacilityUsageReportBLL.GetFiltered(objFacility).ToList();

                if (lstFacilityFacility.Count() > 0)
                {
                    // Build the report entity object
                    _objReportParameter = new ReportParameterEntity();
                    _objReportParameter.DataSet = "dsFacilityUsage"; // Data Set
                    _objReportParameter.Data = lstFacilityFacility; // Record from Data
                    _objReportParameter.ReportPath = Server.MapPath("~/Reports/"); // Path folder where the RDLC file is located
                    _objReportParameter.ReportFilename = "FacilityUsageReport.rdlc"; // Filename of the RDLC file


                    _objReportParameter.Filename = "Facility Usage Report" + "_" + DateTime.Now.ToString("MMddyyy"); // Filename of the dowload type

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

            _FacilityUsageReportBLL = null;

            _objReportParameter = null;

            lstFacilityFacility = null;

            return new JsonResult { Data = _resultEntity, MaxJsonLength = Int32.MaxValue };
        }

        public JsonResult GetFacilityUsageList(string sidx = "", string sord = "", int page = 1, int rows = 10
          , FacilityUsageReportEntity objFacilityUsageReportEntityFilter = null)
        {
            IEnumerable<FacilityUsageReportEntity> objEquipmentItemListTableView = null;
            FacilityUsageReportEntity objEquipmentItemListTableFilter = null;

            int totalRecords = 0;
            int noOfPages = 1;
            int rowStart = 1;

            try
            {
                rowStart = page > 1 ? page * rows - rows + 1 : rowStart;

                objEquipmentItemListTableFilter = new FacilityUsageReportEntity();
                objEquipmentItemListTableFilter.RowStart = rowStart;
                objEquipmentItemListTableFilter.NoOfRecord = rows;
                objEquipmentItemListTableFilter.SortColumn = sidx == "" ? 0 : Convert.ToInt32(sidx);
                objEquipmentItemListTableFilter.SortDirection = sord;
                objEquipmentItemListTableFilter.DateFrom = objFacilityUsageReportEntityFilter.DateFrom == null ? "" : objFacilityUsageReportEntityFilter.DateFrom;
                objEquipmentItemListTableFilter.DateTo = objFacilityUsageReportEntityFilter.DateTo == null ? "" : objFacilityUsageReportEntityFilter.DateTo;

                _FacilityUsageReportBLL = new FacilityUsageReportBLL();
                objEquipmentItemListTableView = _FacilityUsageReportBLL.GetFiltered(objEquipmentItemListTableFilter).ToList();

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
            _FacilityUsageReportBLL = null;

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