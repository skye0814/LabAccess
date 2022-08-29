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
    public class StudentUsageReportController : CommonController
    {
        StudentUsageReportBLL _StudentUsageReportBLL = null;
        // GET: StudentUsageReport
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GenerateStudentUsage()
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
            StudentUsageReportEntity objStudent = new StudentUsageReportEntity();
            List<StudentUsageReportEntity> lstStudent = new List<StudentUsageReportEntity>();
            ReportParameterEntity _objReportParameter = new ReportParameterEntity();
            try
            {
                _StudentUsageReportBLL = new StudentUsageReportBLL();
                objStudent.DateFrom = DateFrom;
                objStudent.DateTo = DateTo;
                lstStudent = _StudentUsageReportBLL.GetFiltered(objStudent).ToList();

                if (lstStudent.Count() > 0)
                {
                    // Build the report entity object
                    _objReportParameter = new ReportParameterEntity();
                    _objReportParameter.DataSet = "dsStudentUsage"; // Data Set
                    _objReportParameter.Data = lstStudent; // Record from Data
                    _objReportParameter.ReportPath = Server.MapPath("~/Reports/"); // Path folder where the RDLC file is located
                    _objReportParameter.ReportFilename = "StudentUsageReport.rdlc"; // Filename of the RDLC file


                    _objReportParameter.Filename = "Year Level Usage Report" + "_" + DateTime.Now.ToString("MMddyyy"); // Filename of the dowload type

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

            _StudentUsageReportBLL = null;

            _objReportParameter = null;

            lstStudent = null;

            return new JsonResult { Data = _resultEntity, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult GetStudentUsageList(string sidx = "", string sord = "", int page = 1, int rows = 10
          , StudentUsageReportEntity objStudentUsageReportEntityFilter = null)
        {
            IEnumerable<StudentUsageReportEntity> objEquipmentItemListTableView = null;
            StudentUsageReportEntity objEquipmentItemListTableFilter = null;

            int totalRecords = 0;
            int noOfPages = 1;
            int rowStart = 1;

            try
            {
                rowStart = page > 1 ? page * rows - rows + 1 : rowStart;

                objEquipmentItemListTableFilter = new StudentUsageReportEntity();
                objEquipmentItemListTableFilter.RowStart = rowStart;
                objEquipmentItemListTableFilter.NoOfRecord = rows;
                objEquipmentItemListTableFilter.SortColumn = sidx == "" ? 0 : Convert.ToInt32(sidx);
                objEquipmentItemListTableFilter.SortDirection = sord;
                objEquipmentItemListTableFilter.DateFrom = objStudentUsageReportEntityFilter.DateFrom == null ? "" : objStudentUsageReportEntityFilter.DateFrom;
                objEquipmentItemListTableFilter.DateTo = objStudentUsageReportEntityFilter.DateTo == null ? "" : objStudentUsageReportEntityFilter.DateTo;

                _StudentUsageReportBLL = new StudentUsageReportBLL();
                objEquipmentItemListTableView = _StudentUsageReportBLL.GetFiltered(objEquipmentItemListTableFilter).ToList();

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
            _StudentUsageReportBLL = null;

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