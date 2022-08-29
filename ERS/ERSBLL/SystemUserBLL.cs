using ERSDAL;
using ERSEntity;
using ERSUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSBLL
{
    public class SystemUserBLL
    {
        SystemUserDAL _systemUserDAL = null;
        public SystemUserEntity ValidateUserCredential(SystemUserEntity objSystemUser)
        {
            SystemUserEntity result = new SystemUserEntity();
            result.MessageList = new List<string>();

            result.UserName = ("" + objSystemUser.UserName).Trim();
            result.Password = ("" + objSystemUser.Password).Trim();
           
            if (string.IsNullOrEmpty(result.UserName))
                result.MessageList.Add(MessageUTIL.PRE_PROVIDE_INPUT + " Username.");
            if (string.IsNullOrEmpty(result.Password))
                result.MessageList.Add(MessageUTIL.PRE_PROVIDE_INPUT + " Password.");

            if (result.MessageList.Count() == 0)
            {
                try
                {
                    _systemUserDAL = new SystemUserDAL();


                    objSystemUser = _systemUserDAL.Get(objSystemUser);
                    if (objSystemUser == null)
                    {
                        objSystemUser = new SystemUserEntity();
                        objSystemUser.UserName = result.UserName;
                        result.MessageList.Add("Username and/or Password " + MessageUTIL.SUFF_INCORRECT);
                        result.IsSuccess = false;
                    }
                    else
                    {
                        if (!objSystemUser.IsActive)
                        {
                            result.MessageList.Add(MessageUTIL.ACCT_INACTIVE);
                            result.IsSuccess = false;
                        }
                        else
                        {
                            result = objSystemUser;
                            result.IsSuccess = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.MessageList = new List<string>();
                    result.IsSuccess = false;
                    result.MessageList.Add(ex.Message);
                }

                _systemUserDAL = null;
            }
            return result;
        }

        public SystemUserEntity Update(SystemUserEntity systemUserEntity)
        {
            SystemUserEntity result = new SystemUserEntity();
            result.MessageList = new List<string>();


            if (systemUserEntity == null)
                systemUserEntity = new SystemUserEntity();

            _systemUserDAL = new SystemUserDAL();

            if (result.MessageList.Count() == 0)
            {

                try
                {
                    _systemUserDAL = new SystemUserDAL();
                    result.IsSuccess = _systemUserDAL.Update(systemUserEntity);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                _systemUserDAL = null;
            }

            return result;

        }

        public SystemUserEntity UpdatePasswordByResetPasswordCode(SystemUserEntity systemUserEntity)
        {
            SystemUserEntity result = new SystemUserEntity();
            result.MessageList = new List<string>();


            if (systemUserEntity == null)
                systemUserEntity = new SystemUserEntity();

            _systemUserDAL = new SystemUserDAL();

            if (result.MessageList.Count() == 0)
            {

                try
                {
                    _systemUserDAL = new SystemUserDAL();
                    result.IsSuccess = _systemUserDAL.UpdatePasswordByResetPasswordCode(systemUserEntity);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                _systemUserDAL = null;
            }

            return result;

        }

        public SystemUserEntity Get(SystemUserEntity systemUserEntity)
        {
            SystemUserEntity result = new SystemUserEntity();
            result.MessageList = new List<string>();
            if (result.MessageList.Count == 0)
            {
                try
                {
                    _systemUserDAL = new SystemUserDAL();
                    result = _systemUserDAL.Get(systemUserEntity);
                    if (result == null)
                    {
                        result = new SystemUserEntity();
                        result.IsSuccess = false;
                        result.MessageList = new List<string>();
                        result.MessageList.Add("System user does not exists.");
                        
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


                _systemUserDAL = null;

            }

            return result;
        }
    }
}
