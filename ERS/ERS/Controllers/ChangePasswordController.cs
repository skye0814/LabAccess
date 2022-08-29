using ERSBLL;
using ERSEntity;
using ERSUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERS.Controllers
{
    
    public class ChangePasswordController : CommonController
    {
        SystemUserBLL _systemUserBLL = null;
        SystemUserChangePasswordEntity _systemUserChangePasswordEntity = null;
        SystemUserEntity _systemUserEntity = null;
        //GET: ChangePassword
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SavePassword(SystemUserChangePasswordEntity objSystemUserChangePasswordEntity)
        {
            try
            {
                
                objSystemUserChangePasswordEntity.SystemUserID = _objSystemUser.ID;
                _systemUserChangePasswordEntity = new SystemUserChangePasswordEntity();
                var err = string.Empty;
                var encrypUtil = new EncryptionUTIL();
              
                var oldPassword = encrypUtil.Encode(objSystemUserChangePasswordEntity.OldPassword, ref err);
                //var newPassword = encrypUtil.Encode(objSystemUserChangePasswordEntity.NewPassword, ref err);
                var newPassword = objSystemUserChangePasswordEntity.NewPassword;

                if (_objSystemUser.Password != oldPassword)
                {
                    _resultEntity.IsSuccess = false;
                    _resultEntity.Result = "Incorrect old password.";
                }
                else
                {
                    newPassword = encrypUtil.Encode(objSystemUserChangePasswordEntity.NewPassword, ref err);
                    _systemUserEntity = new SystemUserEntity();
                    _systemUserEntity.ID = _objSystemUser.ID;
                    _systemUserEntity.UserName = _objSystemUser.UserName;
                    _systemUserEntity.Password = newPassword;
                    _systemUserEntity.ModifiedBy = _objSystemUser.ID;
                    _systemUserBLL = new SystemUserBLL();
                    _systemUserEntity = _systemUserBLL.Update(_systemUserEntity);
                    if (_systemUserEntity.IsSuccess)
                    {
                        _resultEntity.IsSuccess = true;
                    }
                    else
                    {
                        _resultEntity.IsSuccess = false;
                        _resultEntity.IsListResult = true;
                        _resultEntity.Result = _systemUserEntity.MessageList;
                    }
                    //if (newPassword.Length < 5)
                    //{
                    //    _resultEntity.IsSuccess = false;
                    //    _resultEntity.Result = "Password must be 5 characters long";
                    //}
                    
                }
            }
            catch (Exception ex)
            {
                _resultEntity.IsSuccess = false;
                _resultEntity.Result = ex.Message;
            }

            _systemUserBLL = null;

            return Json(_resultEntity);
        }

        //public JsonResult SaveEmail(SystemUserEntity obj)
    }
}