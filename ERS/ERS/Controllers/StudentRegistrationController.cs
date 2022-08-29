using ERSBLL;
using ERSEntity;
using ERSEntity.Entity;
using ERSUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERSBLL.FileManager;
using System.Text.RegularExpressions;

namespace ERS.Controllers
{
    public class StudentRegistrationController : CommonController
    {
        StudentRegistrationBLL _studentRegistrationBLL = null;
        CourseBLL _courseBLL = null;
        YearBLL _yearBLL = null;
        SectionBLL _sectionBLL = null;
        StudentEntity _studentEntity = null;
        // GET: Registration
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StudentRegistrationList()
        {
            if (!_objSystemUser.isLabPersonnel)
            {
                return new HttpStatusCodeResult(401);
            }

            return View();
        }

        public ActionResult StudentRegistrationAdd()
        {
            PopulateDropdown();
            return View();
        }

        public ActionResult StudentRegistrationEdit(int ID = 0, bool isArchive = false)
        {
            try
            {
                _exception = null;

                _studentRegistrationBLL = new StudentRegistrationBLL();
                _studentEntity = new StudentEntity();
                _studentEntity.ID = ID;
                if (isArchive == true)
                {
                    _studentEntity.isArchive = true;
                    
                }
                else if (isArchive == false)
                {
                    _studentEntity.isArchive = false;
                }

                _studentEntity = _studentRegistrationBLL.Get(_studentEntity);

                if (_studentEntity.IsSuccess)
                {
                    PopulateDropdown();
                }
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
            _studentRegistrationBLL = null;

            if (_exception != null)
            {
                _resultEntity.IsListResult = false;
                _resultEntity.Result = _exception.Message;

                return Json(_resultEntity, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (_studentEntity.IsSuccess)
                {
                    return PartialView(_studentEntity);
                }
                else
                {
                    _resultEntity.IsSuccess = _studentEntity.IsSuccess;
                    _resultEntity.Result = _studentEntity.MessageList;
                    _resultEntity.IsListResult = false;

                    return Json(_resultEntity, JsonRequestBehavior.AllowGet);
                }
            }

        }

        public JsonResult GetStudentList(string sidx = "", string sord = "", int page = 1, int rows = 10
          , StudentListEntity objStudentListEntityFilter = null)
        {
            IEnumerable<StudentListEntity> objStudentListTableView = null;
            StudentListEntity objStudentListTableFilter = null;

            int totalRecords = 0;
            int noOfPages = 1;
            int rowStart = 1;

            try
            {
                rowStart = page > 1 ? page * rows - rows + 1 : rowStart;

                objStudentListTableFilter = new StudentListEntity();
                objStudentListTableFilter.RowStart = rowStart;
                objStudentListTableFilter.NoOfRecord = rows;
                objStudentListTableFilter.SortColumn = sidx == "" ? 0 : Convert.ToInt32(sidx);
                objStudentListTableFilter.SortDirection = sord;
                objStudentListTableFilter.StudentNumber = objStudentListEntityFilter.StudentNumber;
                objStudentListTableFilter.FirstName = objStudentListEntityFilter.FirstName;
                objStudentListTableFilter.LastName = objStudentListEntityFilter.LastName;
                objStudentListTableFilter.Year = objStudentListEntityFilter.Year;
                objStudentListTableFilter.Section = objStudentListEntityFilter.Section;
                objStudentListTableFilter.isArchive = objStudentListEntityFilter.isArchive;

                _studentRegistrationBLL = new StudentRegistrationBLL();
                objStudentListTableView = _studentRegistrationBLL.GetFiltered(objStudentListTableFilter).ToList();

                if (objStudentListTableView != null && objStudentListTableView.Count() > 0)
                {
                    totalRecords = objStudentListTableView.FirstOrDefault().TotalRecord;
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

            _studentRegistrationBLL = null;

            objStudentListTableFilter = null;


            var jsonData = new
            {
                total = noOfPages,
                page,
                sort = sidx,
                records = totalRecords - 1,
                rows = objStudentListTableView
            };

            objStudentListTableView = null;

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveStudentRegistration(StudentEntity objStudentEntity)
        {
            try
            {
                _studentRegistrationBLL = new StudentRegistrationBLL();
                _studentEntity = new StudentEntity();
                
                if (objStudentEntity.ID == 0)
                {
                    objStudentEntity.CreatedBy = _objSystemUser.ID;
                    _studentEntity = _studentRegistrationBLL.Insert(objStudentEntity);
                }
                else
                {
                    if (objStudentEntity.isArchive)
                    {
                        objStudentEntity.CreatedBy = _objSystemUser.ID;
                        _studentEntity = _studentRegistrationBLL.Insert(objStudentEntity);
                    }
                    else
                    {
                        objStudentEntity.CreatedBy = _objSystemUser.ID;
                        _studentEntity = _studentRegistrationBLL.Update(objStudentEntity);
                    }
                }

                if (_studentEntity.IsSuccess)
                {
                    _resultEntity.IsSuccess = true;
                }
                else
                {
                    _resultEntity.IsSuccess = false;
                    _resultEntity.IsListResult = true;
                    _resultEntity.Result = _studentEntity.MessageList;
                }
            }
            catch (Exception ex)
            {
                _resultEntity.IsSuccess = false;
                _resultEntity.Result = ex.Message;
            }

            _studentRegistrationBLL = null;

            return Json(_resultEntity);
        }

        public void PopulateDropdown()
        {
            List<CourseEntity> lstCourseEntity = new List<CourseEntity>();
            SelectListItem CourseItemSelection = new SelectListItem();
            List<SelectListItem> lstCourseItemSelection = new List<SelectListItem>();

            _courseBLL = new CourseBLL();
            lstCourseEntity = _courseBLL.GetList().ToList();

            if (lstCourseEntity.ToList().Count > 0)
            {
                foreach (CourseEntity item in lstCourseEntity)
                {
                    CourseItemSelection = new SelectListItem();
                    CourseItemSelection.Value = "" + item.CourseID.ToString();
                    CourseItemSelection.Text = "(" + item.CourseCode + ") " + item.CourseDescription;
                    lstCourseItemSelection.Add(CourseItemSelection);
                    CourseItemSelection = null;
                }
            }

            List<SectionEntity> lstSectionEntity = new List<SectionEntity>();
            SelectListItem SectionItemSelection = new SelectListItem();
            List<SelectListItem> lstSectionItemSelection = new List<SelectListItem>();

            _sectionBLL = new SectionBLL();
            lstSectionEntity = _sectionBLL.GetList().ToList();

            if (lstSectionEntity.ToList().Count > 0)
            {
                foreach (SectionEntity item in lstSectionEntity)
                {
                    SectionItemSelection = new SelectListItem();
                    SectionItemSelection.Value = "" + item.SectionID.ToString();
                    SectionItemSelection.Text = "(" + item.SectionCode + ") " + item.SectionDescription;
                    lstSectionItemSelection.Add(SectionItemSelection);
                    SectionItemSelection = null;
                }
            }

            List<YearEntity> lstYearEntity = new List<YearEntity>();
            SelectListItem YearItemSelection = new SelectListItem();
            List<SelectListItem> lstYearItemSelection = new List<SelectListItem>();

            _yearBLL = new YearBLL();
            lstYearEntity = _yearBLL.GetList().ToList();

            if (lstYearEntity.ToList().Count > 0)
            {
                foreach (YearEntity item in lstYearEntity)
                {
                    YearItemSelection = new SelectListItem();
                    YearItemSelection.Value = "" + item.YearID.ToString();
                    YearItemSelection.Text = "(" + item.YearCode + ") " + item.YearDescription;
                    lstYearItemSelection.Add(YearItemSelection);
                    YearItemSelection = null;
                }
            }
            ViewBag.Course = lstCourseItemSelection;
            ViewBag.Section = lstSectionItemSelection;
            ViewBag.Year = lstYearItemSelection;
        }

        public JsonResult MoveStudentToArchive(List<int> SystemUserIDs)
        {

            _resultEntity = new ResultEntity();

            try
            {
                if (SystemUserIDs == null || SystemUserIDs.Count < 1)
                {
                    _resultEntity.IsSuccess = false;
                    _resultEntity.IsListResult = false;
                    _resultEntity.Result = MessageUTIL.NO_RECORD;
                }
                else
                {
                    _studentRegistrationBLL = new StudentRegistrationBLL();
                    foreach (var SystemUserID in SystemUserIDs)
                    {
                        _resultEntity = _studentRegistrationBLL.MoveToArchive(SystemUserID);

                        if (_resultEntity.IsSuccess)
                        {
                            _resultEntity.IsSuccess = _resultEntity.IsSuccess;
                            _resultEntity.IsListResult = false;
                            _resultEntity.Result = SystemUserIDs.Count + MessageUTIL.SUFF_SUCESS_ARCHIVE;
                        }
                        else
                        {
                            _resultEntity.IsSuccess = _resultEntity.IsSuccess;
                            _resultEntity.IsListResult = false;
                            _resultEntity.Result = MessageUTIL.NO_RECORD_ARCHIVED;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _resultEntity.IsSuccess = false;
                _resultEntity.Result = ex.Message;
            }

            _studentRegistrationBLL = null;

            return Json(_resultEntity);
        }

        public JsonResult DeleteStudentFromArchive(List<int> SystemUserIDs)
        {

            _resultEntity = new ResultEntity();

            try
            {
                if (SystemUserIDs == null || SystemUserIDs.Count < 1)
                {
                    _resultEntity.IsSuccess = false;
                    _resultEntity.IsListResult = false;
                    _resultEntity.Result = MessageUTIL.NO_RECORD;
                }
                else
                {
                    _studentRegistrationBLL = new StudentRegistrationBLL();
                    foreach (var SystemUserID in SystemUserIDs)
                    {
                        _resultEntity = _studentRegistrationBLL.DeleteFromArchive(SystemUserID);

                        if (_resultEntity.IsSuccess)
                        {
                            _resultEntity.IsSuccess = _resultEntity.IsSuccess;
                            _resultEntity.IsListResult = false;
                            _resultEntity.Result = SystemUserIDs.Count + MessageUTIL.SUFF_SUCESS_DELETE;
                        }
                        else
                        {
                            _resultEntity.IsSuccess = _resultEntity.IsSuccess;
                            _resultEntity.IsListResult = false;
                            _resultEntity.Result = MessageUTIL.NO_RECORD_DELETED;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _resultEntity.IsSuccess = false;
                _resultEntity.Result = ex.Message;
            }

            _studentRegistrationBLL = null;

            return Json(_resultEntity);
        }

        #region for Upload and export
        [HttpPost]
        public JsonResult DeleteUploadedFile(string contentSessionId)
        {
            FileManagerBLL fileManagerBLL = null;
            FileManagerEntity objFileManager = null;
            UploadSessionInfoEntity objUploadSessionInfo = (UploadSessionInfoEntity)Session[contentSessionId];

            // Clear Upload Session
            Session[contentSessionId] = null;
            Session.Remove(contentSessionId);

            try
            {
                objFileManager = new FileManagerEntity();
                objFileManager.Filename = objUploadSessionInfo.ServerFilename;
                objFileManager.Path = System.Web.HttpContext.Current.Server.MapPath(Util.GetUploadFilePath((int)UploadPathUtil.StudentRegistration));

                fileManagerBLL = new FileManagerBLL();
                fileManagerBLL.Delete(objFileManager);

                _resultEntity.IsListResult = false;
                _resultEntity.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _resultEntity.IsListResult = false;
                _resultEntity.IsSuccess = false;
                _resultEntity.Result = ex.Message;
            }

            objFileManager = null;
            objUploadSessionInfo = null;

            fileManagerBLL = null;

            return new JsonResult { Data = _resultEntity, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public ActionResult UploadStudentRegistration()
        {
            string uploadedFileName = Request["HTTP_X_FILE_NAME"];
            string uploadedFileSize = Request["HTTP_X_FILE_SIZE"];

            FileManagerBLL fileManagerBLL = null;
            FileManagerEntity objFileManager = null;
            List<StudentRegistrationExcelValuesEntity> objStudentRegisrationExcelValuesEntity = null;
            UploadSessionInfoEntity objUploadSessionInfo = null;

            Regex objRegex = new Regex(RegexUTIL.REGEX_UPLOAD_EXCEL);

            if (objRegex.IsMatch(uploadedFileName))
            {
                try
                {
                    // Save File
                    objFileManager = new FileManagerEntity();
                    objFileManager.Filename = Util.GetFormUploadFilename(FormUtil.StudentRegistration, uploadedFileName);
                    objFileManager.Path = System.Web.HttpContext.Current.Server.MapPath(Util.GetUploadFilePath((int)UploadPathUtil.StudentRegistration));
                    objFileManager.Byte = new byte[Request.InputStream.Length];

                    // Read uploaded file byte
                    int offset = 0;
                    int cnt = 0;
                    while ((cnt = Request.InputStream.Read(objFileManager.Byte, offset, 10)) > 0)
                    {
                        offset += cnt;
                    }

                    // Save uploaded Excel file then read
                    fileManagerBLL = new FileManagerBLL();
                    objFileManager = fileManagerBLL.Save(objFileManager);

                    if (objFileManager.IsSuccess)
                    {
                        try
                        {
                            _studentRegistrationBLL = new StudentRegistrationBLL();
                            objStudentRegisrationExcelValuesEntity = _studentRegistrationBLL.StudentRegistrationGetUploadValues(objFileManager.Filename, objFileManager.Path).ToList();
                        }
                        catch (Exception ex)
                        {
                            // Delete uploaded file incase of error
                            try
                            {
                                fileManagerBLL.Delete(objFileManager);
                            }
                            catch { }

                            throw ex;
                        }

                        // Build the uploaded file session
                        objUploadSessionInfo = new UploadSessionInfoEntity();
                        objUploadSessionInfo.Content = objStudentRegisrationExcelValuesEntity;
                        objUploadSessionInfo.ContentSessionId = Util.GetUploadSessionId();
                        objUploadSessionInfo.OriginalFilename = uploadedFileName;
                        objUploadSessionInfo.ServerFilename = objFileManager.Filename;

                        Session[objUploadSessionInfo.ContentSessionId] = objUploadSessionInfo;

                        _resultEntity.IsSuccess = true;
                        _resultEntity.IsListResult = false;
                        _resultEntity.Result = objUploadSessionInfo;
                    }
                    else
                    {
                        _resultEntity.IsSuccess = false;
                        _resultEntity.IsListResult = true;
                        _resultEntity.Result = objFileManager.MessageList;
                    }
                }
                catch (Exception ex)
                {
                    _resultEntity.IsSuccess = false;
                    _resultEntity.IsListResult = false;
                    _resultEntity.Result = MessageUTIL.ERROR_EXCEL_FILE; //ex.Message;//PayrollMessageUtil.ERROR_EXCEL_FILE;
                }
            }
            else
            {
                _resultEntity.IsSuccess = false;
                _resultEntity.IsListResult = false;
                _resultEntity.Result = MessageUTIL.ERRMSG_INVALID_FILE;
            }

            _studentRegistrationBLL = null;

            fileManagerBLL = null;

            objStudentRegisrationExcelValuesEntity = null;
            objFileManager = null;
            objUploadSessionInfo = null;
            objRegex = null;

            uploadedFileName = null;
            uploadedFileSize = null;

            return new JsonResult { Data = _resultEntity, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult SaveUploadedStudentRegistration(string contentSessionId)
        {
            FileManagerBLL fileManagerBLL = null;
            FileManagerEntity objFileManager = null;
            CommonEntity result = new CommonEntity();
            objStudentRegistrationExcelEntity objStudentRegistrationExcelEntity = new objStudentRegistrationExcelEntity();
            UploadSessionInfoEntity objUploadSessionInfo = (UploadSessionInfoEntity)Session[contentSessionId];

            // Clear Upload Session
            Session[contentSessionId] = null;
            Session.Remove(contentSessionId);

            try
            {
                objStudentRegistrationExcelEntity.ExcelValues = (List<StudentRegistrationExcelValuesEntity>)objUploadSessionInfo.Content;
                objStudentRegistrationExcelEntity.UploadedFilename = objUploadSessionInfo.OriginalFilename;
                objStudentRegistrationExcelEntity.UploadedServerFilename = objUploadSessionInfo.ServerFilename;

                _studentRegistrationBLL = new StudentRegistrationBLL();
                result = _studentRegistrationBLL.StudentRegistrationUpload(objStudentRegistrationExcelEntity, _objSystemUser.ID);
                if (result.IsSuccess)
                {
                    _resultEntity.IsSuccess = true;
                    _resultEntity.IsListResult = false;
                    _resultEntity.Result = MessageUTIL.SCSSMSG_REC_SAVE;
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

            // Delete uploaded file incase of error
            if (!_resultEntity.IsSuccess)
            {
                try
                {
                    objFileManager = new FileManagerEntity();
                    objFileManager.Filename = objUploadSessionInfo.ServerFilename;
                    objFileManager.Path = System.Web.HttpContext.Current.Server.MapPath(Util.GetUploadFilePath((int)UploadPathUtil.StudentRegistration));

                    fileManagerBLL = new FileManagerBLL();
                    fileManagerBLL.Delete(objFileManager);
                }
                catch { }
            }

            fileManagerBLL = null;
            _studentRegistrationBLL = null;
            objFileManager = null;
            objStudentRegistrationExcelEntity = null;
            objUploadSessionInfo = null;

            return new JsonResult { Data = _resultEntity, MaxJsonLength = Int32.MaxValue };
        }
        #endregion
    }
}
