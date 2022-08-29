using ERSDAL;
using ERSEntity;
using ERSEntity.Entity;
using ERSUtil;
using NPOI.SS.UserModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ERSBLL
{
    public class StudentRegistrationBLL
    {
        StudentRegistrationDAL _studentRegistrationDAL = null;
        SystemUserBLL _systemUserBLL = null;
        EncryptionUTIL _encryptionUTIL = null;
        DataTable dtStudent = null;
        public IEnumerable<StudentListEntity> GetFiltered(StudentListEntity objStudentListEntityFilter)
        {

            if (objStudentListEntityFilter == null)
                throw new Exception(MessageUTIL.PROVIDE_VALID_FILTER + "Students.");

            objStudentListEntityFilter.SortDirection = ("" + objStudentListEntityFilter.SortDirection).Trim().ToLower();

            if (objStudentListEntityFilter.RowStart == 0)
                objStudentListEntityFilter.NoOfRecord = DefaultValuesUTIL.TableDisplayCountPerPage;
            if (!string.IsNullOrEmpty(objStudentListEntityFilter.SortDirection))
                if (objStudentListEntityFilter.SortDirection != "asc" && objStudentListEntityFilter.SortDirection != "desc")
                    objStudentListEntityFilter.SortDirection = "";

            List<StudentListEntity> result = null;

            try
            {
                _studentRegistrationDAL = new StudentRegistrationDAL();
                result = new List<StudentListEntity>();

                if (objStudentListEntityFilter.isArchive == true)
                    result = _studentRegistrationDAL.ArchiveGetFiltered(objStudentListEntityFilter).ToList();
                else
                    result = _studentRegistrationDAL.GetFiltered(objStudentListEntityFilter).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return result;
        }

        public StudentEntity Insert(StudentEntity objStudentEntity)
        {
            StudentEntity result = new StudentEntity();
            StudentEntity duplicateCheck = new StudentEntity();
            SystemUserEntity _systemUserEntity = new SystemUserEntity();
            string errorMessage = string.Empty;
            result.MessageList = new List<string>();


            if (objStudentEntity == null)
                objStudentEntity = new StudentEntity();

            if (string.IsNullOrEmpty(objStudentEntity.StudentNumber))
                result.MessageList.Add(MessageUTIL.PRE_PROVIDE_INPUT + " Student Number.");
            _systemUserEntity.UserName = objStudentEntity.UserName;
            _systemUserBLL = new SystemUserBLL();
            _systemUserEntity = _systemUserBLL.Get(_systemUserEntity);
            if (_systemUserEntity == null || _systemUserEntity.IsSuccess == false)
            {
                _studentRegistrationDAL = new StudentRegistrationDAL();
                duplicateCheck.StudentNumber = objStudentEntity.StudentNumber;
                duplicateCheck = _studentRegistrationDAL.Get(duplicateCheck);
                if (duplicateCheck == null)
                {
                    duplicateCheck = new StudentEntity();
                    duplicateCheck.EmailAddress = objStudentEntity.EmailAddress;
                    duplicateCheck = _studentRegistrationDAL.Get(duplicateCheck);
                    if (duplicateCheck == null)
                    {
                        if (result.MessageList.Count() == 0)
                        {

                            try
                            {

                                _encryptionUTIL = new EncryptionUTIL();
                                objStudentEntity.Password = _encryptionUTIL.Encode(DefaultValuesUTIL.defaultPasssword, ref errorMessage);
                                _studentRegistrationDAL = new StudentRegistrationDAL();
                                result = _studentRegistrationDAL.Insert(objStudentEntity);
                                if (result.ID > 0)
                                    result.IsSuccess = true;
                                else
                                {
                                    result.IsSuccess = false;
                                    result.MessageList = new List<string>();
                                    result.MessageList.Add("An error was encountered. Please try again.");
                                }
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }

                            _studentRegistrationDAL = null;
                        }
                    }
                    else
                    {
                        result.MessageList = new List<string>();
                        result.MessageList.Add("Email address is already in use.");
                        result.IsSuccess = false;
                    }


                }
                else
                {
                    result.MessageList = new List<string>();
                    result.MessageList.Add("Student Number already registered.");
                    result.IsSuccess = false;
                }
            }
            else
            {
                result.MessageList = new List<string>();
                result.MessageList.Add("Username already exists. Please use a different username.");
                result.IsSuccess = false;
            }
            return result;
        }

        public StudentEntity Update(StudentEntity objStudentEntity)
        {
            StudentEntity duplicateCheck = new StudentEntity();
            StudentEntity result = new StudentEntity();
            StudentRegistrationBLL _duplicateCheckBLL = new StudentRegistrationBLL();
            result.MessageList = new List<string>();


            if (objStudentEntity == null)
                objStudentEntity = new StudentEntity();

            _studentRegistrationDAL = new StudentRegistrationDAL();
            duplicateCheck.ID = objStudentEntity.ID;
            duplicateCheck.StudentNumber = objStudentEntity.StudentNumber;
            duplicateCheck.EmailAddress = objStudentEntity.EmailAddress;
            duplicateCheck.UserName = objStudentEntity.UserName;
            duplicateCheck = _duplicateCheckBLL.Get(duplicateCheck);
            if (duplicateCheck.Status == "recheck")
            {
                result.MessageList = new List<string>();
                duplicateCheck = new StudentEntity
                {
                    ID = objStudentEntity.ID,
                    UserName = objStudentEntity.UserName,
                    Mode = "check"
                };
                duplicateCheck = _duplicateCheckBLL.Get(duplicateCheck);
                if (!duplicateCheck.IsSuccess)
                {
                    result.MessageList.Add("Username is already in use.");
                    result.IsSuccess = false;
                }
                duplicateCheck = new StudentEntity
                {
                    ID = objStudentEntity.ID,
                    StudentNumber = objStudentEntity.StudentNumber,
                    Mode = "check"
                };
                duplicateCheck = _duplicateCheckBLL.Get(duplicateCheck);
                if (!duplicateCheck.IsSuccess)
                {
                    result.MessageList.Add("Student Number is already registered.");
                    result.IsSuccess = false;
                }
                duplicateCheck = new StudentEntity
                {
                    ID = objStudentEntity.ID,
                    EmailAddress = objStudentEntity.EmailAddress,
                    Mode = "check"
                };
                duplicateCheck = _duplicateCheckBLL.Get(duplicateCheck);
                if (!duplicateCheck.IsSuccess)
                {
                    result.MessageList.Add("Email address is already in use.");
                    result.IsSuccess = false;
                }
            }
            if (result.MessageList.Count() == 0)
            {

                try
                {
                    _studentRegistrationDAL = new StudentRegistrationDAL();
                    result.IsSuccess = _studentRegistrationDAL.Update(objStudentEntity);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                _studentRegistrationDAL = null;
            }
            return result;

        }

        public StudentEntity Get(StudentEntity objStudentEntity)
        {
            StudentEntity result = new StudentEntity();
            result.MessageList = new List<string>();
            if (result.MessageList.Count == 0)
            {

                try
                {
                    _studentRegistrationDAL = new StudentRegistrationDAL();
                    result = _studentRegistrationDAL.Get(objStudentEntity);
                    if (result == null)
                    {
                        result = new StudentEntity();
                        result.IsSuccess = false;
                        result.MessageList = new List<string>();
                        result.MessageList.Add("Student does not exists.");
                    }
                    else
                    {
                        result.IsSuccess = true;

                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }


                _studentRegistrationDAL = null;

            }

            return result;
        }

        public ResultEntity MoveToArchive(int SystemUserID)
        {

            ResultEntity result = new ResultEntity();
            try
            {
                _studentRegistrationDAL = new StudentRegistrationDAL();
                result.IsSuccess = _studentRegistrationDAL.MoveToArchive(SystemUserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public ResultEntity DeleteFromArchive(int SystemUserID)
        {

            ResultEntity result = new ResultEntity();
            try
            {
                StudentEntity objEntity = new StudentEntity();
                objEntity.SystemUserID = SystemUserID;
                _studentRegistrationDAL = new StudentRegistrationDAL();
                result.IsSuccess = _studentRegistrationDAL.Delete(objEntity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        #region Upload
        public IEnumerable<StudentRegistrationExcelValuesEntity> StudentRegistrationGetUploadValues(string filename, string FilePath, int type = 0)
        {

            IWorkbook iWorkbook = null;
            ISheet iSheet = null;
            FileStream file = null;

            #region NPOI
            try
            {
             

                file = new FileStream(Path.Combine(FilePath, filename), FileMode.Open, FileAccess.Read);
                iWorkbook = WorkbookFactory.Create(file);
                iSheet = iWorkbook.GetSheetAt(0);
            }
            catch (Exception ex)
            {
                throw;
            }
            if (iSheet != null)
            {
                // Row index starts at 0 for NPOI
                for (int i = 1; i < iSheet.LastRowNum + 1; i++)
                {
                    if (iSheet.GetRow(i) != null) // null is when the row only contains empty cells 
                    {

                        yield return new StudentRegistrationExcelValuesEntity()
                        {
                            Row = (i + 1).ToString(),
                            FirstName = ("" + iSheet.GetRow(i).GetCell(StudentRegistrationColumnUtil.FirstName - 1)).Trim(),
                            MiddleName = ("" + iSheet.GetRow(i).GetCell(StudentRegistrationColumnUtil.MiddleName - 1)).Trim(),
                            LastName = ("" + iSheet.GetRow(i).GetCell(StudentRegistrationColumnUtil.LastName - 1)).Trim(),
                            StudentNumber = ("" + iSheet.GetRow(i).GetCell(StudentRegistrationColumnUtil.StudentNumber - 1)).Trim(),

                            Course = ("" + iSheet.GetRow(i).GetCell(StudentRegistrationColumnUtil.Course - 1)).Trim(),
                            Section = ("" + iSheet.GetRow(i).GetCell(StudentRegistrationColumnUtil.Section - 1)).Trim(),
                            Year = ("" + iSheet.GetRow(i).GetCell(StudentRegistrationColumnUtil.Year - 1)).Trim(),
                            EmailAddress = ("" + iSheet.GetRow(i).GetCell(StudentRegistrationColumnUtil.EmailAddress - 1)).Trim(),
                            UserName = ("" + iSheet.GetRow(i).GetCell(StudentRegistrationColumnUtil.UserName - 1)).Trim(),

                        };
                    }
                }
            }
            #endregion


            file.Close();
            file.Dispose();
            file = null;

        }

        public CommonEntity StudentRegistrationUpload(objStudentRegistrationExcelEntity objStudentResigtrationExcelEntity, int UserID = 0)
        {
            CommonEntity result = new CommonEntity();
            result.MessageList = new List<string>();
            List<String> lstErrorMessage = new List<string>();
          

            #region Level 1 Input validation - Without database validation

            List<StudentRegistrationExcelValuesEntity> lstStudentRegistrationExcelValues = new List<StudentRegistrationExcelValuesEntity>();

            if (objStudentResigtrationExcelEntity == null)
            {
                objStudentResigtrationExcelEntity = new objStudentRegistrationExcelEntity();
            }
            if (objStudentResigtrationExcelEntity.ExcelValues == null)
            {
                objStudentResigtrationExcelEntity.ExcelValues = new List<StudentRegistrationExcelValuesEntity>();
            }

            string uploadedFilename = ("" + objStudentResigtrationExcelEntity.UploadedFilename).Trim();
            string uploadedServerFilename = ("" + objStudentResigtrationExcelEntity.UploadedServerFilename).Trim();

            if (string.IsNullOrEmpty(uploadedFilename))
            {
                result.MessageList.Add(MessageUTIL.PRE_PROVIDE_INPUT + "Filename.");
            }
            if (string.IsNullOrEmpty(uploadedServerFilename))
            {
                result.MessageList.Add(MessageUTIL.PRE_PROVIDE_INPUT + "Server Filename.");
            }


            result.MessageList = ValidateStudentRegistrationEntity(objStudentResigtrationExcelEntity.ExcelValues);

            #endregion

            #region Level 2 - Database Validation
            if (result.MessageList.Count() == 0)
            {
                result.MessageList = ValidateStudentRegistrationEntityDatabase(objStudentResigtrationExcelEntity.ExcelValues);
            }
            #endregion

            if (result.MessageList.Count() == 0)
            {
                result.IsSuccess = true;
                try
                {
                    _studentRegistrationDAL = new StudentRegistrationDAL();
                    result.IsSuccess = _studentRegistrationDAL.UploadStudent(dtStudent, UserID).IsSuccess;
                }

                catch (Exception ex)
                {
                    throw;
                }
                _studentRegistrationDAL = null;

            }
            return result;
        }

        private List<String> ValidateStudentRegistrationEntity(List<StudentRegistrationExcelValuesEntity> lstStudentRegistrationExcelValues)
        {
            List<String> errorMessages = new List<string>();
            try
            {
                foreach (StudentRegistrationExcelValuesEntity objStudentRegistrationValue in lstStudentRegistrationExcelValues)
                {

                    Regex regexName = new Regex(RegexUTIL.REGEX_NAME);
                    //CHECK IF EXCEL INPUT IS COMPLETE
                    if (!string.IsNullOrEmpty(objStudentRegistrationValue.Row))
                    {
                        #region First Name

                        if (string.IsNullOrEmpty(objStudentRegistrationValue.FirstName))
                        {
                            errorMessages.Add(objStudentRegistrationValue.Row + "* " + objStudentRegistrationValue.FirstName + "* " + MessageUTIL.PRO_VALID_VAL +
                                " First Name.");

                        }

                        else if (!regexName.IsMatch(objStudentRegistrationValue.FirstName))
                        {
                            errorMessages.Add(objStudentRegistrationValue.Row + "* " + objStudentRegistrationValue.FirstName + "* " + MessageUTIL.PRO_VALID_VAL +
                                " First Name.");
                        }

                        #endregion

                        #region Middle Name
                        if (!string.IsNullOrEmpty(objStudentRegistrationValue.MiddleName) && !regexName.IsMatch(objStudentRegistrationValue.MiddleName))
                        {
                            errorMessages.Add(objStudentRegistrationValue.Row + "* " + objStudentRegistrationValue.MiddleName + "* " + MessageUTIL.PRO_VALID_VAL +
                                " Middle Name.");
                        }

                        #endregion

                        #region Last Name

                        if (string.IsNullOrEmpty(objStudentRegistrationValue.LastName))
                        {
                            errorMessages.Add(objStudentRegistrationValue.Row + "* " + objStudentRegistrationValue.LastName + "* " + MessageUTIL.PRO_VALID_VAL +
                                " Last Name.");

                        }

                        else if (!regexName.IsMatch(objStudentRegistrationValue.LastName))
                        {
                            errorMessages.Add(objStudentRegistrationValue.Row + "* " + objStudentRegistrationValue.LastName + "* " + MessageUTIL.PRO_VALID_VAL +
                                " Last Name.");
                        }

                        #endregion

                        #region Student Number

                        if (string.IsNullOrEmpty(objStudentRegistrationValue.StudentNumber) && !regexName.IsMatch(objStudentRegistrationValue.StudentNumber))
                        {
                            errorMessages.Add(objStudentRegistrationValue.Row + "* " + objStudentRegistrationValue.StudentNumber + "* " + MessageUTIL.PRO_VALID_VAL +
                                " Student Number.");

                        }

                        #endregion

                        #region Course

                        if (string.IsNullOrEmpty(objStudentRegistrationValue.Course))
                        {
                            errorMessages.Add(objStudentRegistrationValue.Row + "* " + objStudentRegistrationValue.Course + "* " + MessageUTIL.PRO_VALID_VAL +
                                " Course.");

                        }
                        #endregion


                        #region Section

                        if (string.IsNullOrEmpty(objStudentRegistrationValue.Section))
                        {
                            errorMessages.Add(objStudentRegistrationValue.Row + "* " + objStudentRegistrationValue.Section + "* " + MessageUTIL.PRO_VALID_VAL +
                                " Section.");

                        }
                        #endregion

                        #region Year

                        if (string.IsNullOrEmpty(objStudentRegistrationValue.Year))
                        {
                            errorMessages.Add(objStudentRegistrationValue.Row + "* " + objStudentRegistrationValue.Year + "* " + MessageUTIL.PRO_VALID_VAL +
                                " Year.");

                        }
                        #endregion

                        #region EmailAddress

                        if (string.IsNullOrEmpty(objStudentRegistrationValue.EmailAddress))
                        {
                            errorMessages.Add(objStudentRegistrationValue.Row + "* " + objStudentRegistrationValue.EmailAddress + "* " + MessageUTIL.PRO_VALID_VAL +
                                " EmailAddress.");

                        }
                        #endregion

                        #region UserName

                        //if (string.IsNullOrEmpty(objStudentRegistrationValue.UserName))
                        //{
                        //    errorMessages.Add(objStudentRegistrationValue.Row + "* " + objStudentRegistrationValue.UserName + "* " + MessageUTIL.PRO_VALID_VAL +
                        //        " UserName.");

                        //}
                        #endregion


                    }
                    //#region Validate if with duplicate record in excel file
                    //if (errorMessages.Count == 0)
                    //{
                    //    int index = 0;
                    //    index = lstStudentRegistrationExcelValues.FindIndex(x => x.UserName == objStudentRegistrationValue.UserName);

                    //    if (index != -1)
                    //    {
                    //        if (lstStudentRegistrationExcelValues[index].Row != objStudentRegistrationValue.Row)
                    //        {
                    //            errorMessages.Add(objStudentRegistrationValue.Row + "*" + objStudentRegistrationValue.UserName +
                    //                "* Deduction Code already exist in Row " + lstStudentRegistrationExcelValues[index].Row + ".");
                    //        }
                    //    }
                    //}
                    //#endregion
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return errorMessages;
        }

        private List<String> ValidateStudentRegistrationEntityDatabase(List<StudentRegistrationExcelValuesEntity> lstStudentRegistrationExcelValues, int UserId = 0)
        {
            List<String> lstErrorMessage = new List<string>();
            List<string> errorMessages = new List<string>();
            string errorMessage = string.Empty;
            dtStudent = new DataTable();
            dtStudent.Columns.Add("RowNumber", typeof(string));
            dtStudent.Columns.Add("FirstName", typeof(string));
            dtStudent.Columns.Add("MiddleName", typeof(string));
            dtStudent.Columns.Add("LastName", typeof(string));
            dtStudent.Columns.Add("StudentNumber", typeof(string));
            dtStudent.Columns.Add("Course", typeof(string));
            dtStudent.Columns.Add("Section", typeof(string));
            dtStudent.Columns.Add("Year", typeof(string));
            dtStudent.Columns.Add("EmailAddress", typeof(string));
            dtStudent.Columns.Add("UserName", typeof(string));
            dtStudent.Columns.Add("Password", typeof(string));
            

            if (lstStudentRegistrationExcelValues != null)
            {
                _encryptionUTIL = new EncryptionUTIL();
                for (int per = 0; per < lstStudentRegistrationExcelValues.Count(); per++)
                {
                    dtStudent.Rows.Add(
                        lstStudentRegistrationExcelValues[per].Row,
                        lstStudentRegistrationExcelValues[per].FirstName,
                        lstStudentRegistrationExcelValues[per].MiddleName,
                         lstStudentRegistrationExcelValues[per].LastName,
                        lstStudentRegistrationExcelValues[per].StudentNumber,
                        lstStudentRegistrationExcelValues[per].Course,
                        lstStudentRegistrationExcelValues[per].Section,
                        lstStudentRegistrationExcelValues[per].Year,
                        lstStudentRegistrationExcelValues[per].EmailAddress,
                        lstStudentRegistrationExcelValues[per].StudentNumber,
                        _encryptionUTIL.Encode(DefaultValuesUTIL.defaultPasssword, ref errorMessage)

                        );
                }
            }
            _studentRegistrationDAL = new StudentRegistrationDAL();
            errorMessages = _studentRegistrationDAL.ValidateStudentRegistration(dtStudent);

            if (errorMessages.Count > 0)
            {
                foreach (string message in errorMessages)
                {
                    lstErrorMessage.Add(message);
                }
            }


            return lstErrorMessage;
        }

        #endregion
    }
}
