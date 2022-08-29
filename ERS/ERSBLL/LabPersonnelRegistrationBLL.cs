using ERSDAL;
using ERSEntity;
using ERSEntity.Entity;
using ERSUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSBLL
{
    
    public class LabPersonnelRegistrationBLL
    {
        LabPersonnelDAL _labPersonnelDAL = null;
        EncryptionUTIL _encryptionUTIL = null;
        SystemUserBLL _systemUserBLL = null;
        public IEnumerable<LabPersonnelListEntity> GetFiltered(LabPersonnelListEntity objLabPersonnelListEntityFilter)
        {

            if (objLabPersonnelListEntityFilter == null)
                throw new Exception(MessageUTIL.PROVIDE_VALID_FILTER + "Lab Personnel.");

            objLabPersonnelListEntityFilter.SortDirection = ("" + objLabPersonnelListEntityFilter.SortDirection).Trim().ToLower();

            if (objLabPersonnelListEntityFilter.RowStart == 0)
                objLabPersonnelListEntityFilter.NoOfRecord = DefaultValuesUTIL.TableDisplayCountPerPage;
            if (!string.IsNullOrEmpty(objLabPersonnelListEntityFilter.SortDirection))
                if (objLabPersonnelListEntityFilter.SortDirection != "asc" && objLabPersonnelListEntityFilter.SortDirection != "desc")
                    objLabPersonnelListEntityFilter.SortDirection = "";

            List<LabPersonnelListEntity> result = null;

            try
            {
                _labPersonnelDAL = new LabPersonnelDAL();
                result = new List<LabPersonnelListEntity>();

                if (objLabPersonnelListEntityFilter.isArchive == true)
                    result = _labPersonnelDAL.ArchiveGetFiltered(objLabPersonnelListEntityFilter).ToList();
                else
                    result = _labPersonnelDAL.GetFiltered(objLabPersonnelListEntityFilter).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return result;
        }

        public LabPersonnelEntity Insert(LabPersonnelEntity objLabPersonnelEntity)
        {
            LabPersonnelEntity result = new LabPersonnelEntity();
            LabPersonnelEntity duplicateCheck = new LabPersonnelEntity();
            SystemUserEntity _systemUserEntity = new SystemUserEntity();
            string errorMessage = string.Empty;
            result.MessageList = new List<string>();


            if (objLabPersonnelEntity == null)
                objLabPersonnelEntity = new LabPersonnelEntity();

            _systemUserEntity.UserName = objLabPersonnelEntity.UserName;
            _systemUserBLL = new SystemUserBLL();
            _systemUserEntity = _systemUserBLL.Get(_systemUserEntity);

            if (_systemUserEntity == null || _systemUserEntity.IsSuccess == false)
            {
                _labPersonnelDAL = new LabPersonnelDAL();
                duplicateCheck.EmailAddress = objLabPersonnelEntity.EmailAddress;
                duplicateCheck = _labPersonnelDAL.Get(duplicateCheck);
                if (duplicateCheck == null)
                {
                    if (result.MessageList.Count() == 0)
                    {
                        try
                        {
                            _encryptionUTIL = new EncryptionUTIL();
                            objLabPersonnelEntity.Password = _encryptionUTIL.Encode(DefaultValuesUTIL.defaultadminPassword, ref errorMessage);
                            _labPersonnelDAL = new LabPersonnelDAL();
                            result = _labPersonnelDAL.Insert(objLabPersonnelEntity);
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

                        _labPersonnelDAL = null;
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
                result.MessageList.Add("Username already exists. Please use a different username.");
                result.IsSuccess = false;
            }

            return result;
        }

        public LabPersonnelEntity Update(LabPersonnelEntity objLabPersonnelEntity)
        {
            LabPersonnelEntity duplicateCheck = new LabPersonnelEntity();
            LabPersonnelEntity result = new LabPersonnelEntity();
            LabPersonnelRegistrationBLL _duplicateCheckBLL = new LabPersonnelRegistrationBLL();
            result.MessageList = new List<string>();


            if (objLabPersonnelEntity == null)
                objLabPersonnelEntity = new LabPersonnelEntity();

            _labPersonnelDAL = new LabPersonnelDAL();
            duplicateCheck.ID = objLabPersonnelEntity.ID;
            duplicateCheck.EmailAddress = objLabPersonnelEntity.EmailAddress;
            duplicateCheck.UserName = objLabPersonnelEntity.UserName;
            duplicateCheck = _duplicateCheckBLL.Get(duplicateCheck);
            if (duplicateCheck.Status == "recheck")
            {
                result.MessageList = new List<string>();
                duplicateCheck = new LabPersonnelEntity
                {
                    ID = objLabPersonnelEntity.ID,
                    UserName = objLabPersonnelEntity.UserName,
                    Mode = "check"
                };
                duplicateCheck = _duplicateCheckBLL.Get(duplicateCheck);
                if (!duplicateCheck.IsSuccess)
                {
                    result.MessageList.Add("Username is already in use.");
                    result.IsSuccess = false;
                }
                duplicateCheck = new LabPersonnelEntity
                {
                    ID = objLabPersonnelEntity.ID,
                    EmailAddress = objLabPersonnelEntity.EmailAddress,
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
                    _labPersonnelDAL = new LabPersonnelDAL();
                    result.IsSuccess = _labPersonnelDAL.Update(objLabPersonnelEntity);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                _labPersonnelDAL = null;
            }

            return result;

        }

        public LabPersonnelEntity Get(LabPersonnelEntity objLabPersonnelEntity)
        {
            LabPersonnelEntity result = new LabPersonnelEntity();
            result.MessageList = new List<string>();
            if (result.MessageList.Count == 0)
            {

                try
                {
                    _labPersonnelDAL = new LabPersonnelDAL();
                    result = _labPersonnelDAL.Get(objLabPersonnelEntity);
                    if (result == null)
                    {
                        result = new LabPersonnelEntity();
                        result.IsSuccess = false;
                        result.MessageList = new List<string>();
                        result.MessageList.Add("Lab Personnel does not exist.");
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


                _labPersonnelDAL = null;

            }

            return result;
        }

        public ResultEntity MoveToArchive(int SystemUserID)
        {

            ResultEntity result = new ResultEntity();
            try
            {
                _labPersonnelDAL = new LabPersonnelDAL();
                result.IsSuccess = _labPersonnelDAL.MoveToArchive(SystemUserID);
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
                LabPersonnelEntity objEntity = new LabPersonnelEntity();
                objEntity.SystemUserID = SystemUserID;
                _labPersonnelDAL = new LabPersonnelDAL();
                result.IsSuccess = _labPersonnelDAL.Delete(objEntity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}
