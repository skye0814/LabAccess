using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using ERSEntity;
using ERSBLL;
using ERSEntity.Entity;
using ERSUtil;

namespace ERS.Controllers
{
    public class LoginController : Controller
    {
        SystemUserBLL _systemUserBLL = null;
        ResultEntity _resultEntity = new ResultEntity();

        [Route("")]
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["SystemUser"] != null)
            {
                return Redirect("~/Home");
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ValidateCredentials(SystemUserEntity objSystemUser)
        {
            var keybytes = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["keybytes"].ToString());
            var iv = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["ivbytes"].ToString());

            var encryptedUsername = Convert.FromBase64String(objSystemUser.UserName);
            objSystemUser.UserName = Util.DecryptStringFromBytes(encryptedUsername, keybytes, iv);

            SystemUserEntity _companyDetails = new SystemUserEntity();
            _systemUserBLL = new SystemUserBLL();
            try
            {
                objSystemUser = _systemUserBLL.ValidateUserCredential(objSystemUser);

                if (objSystemUser.IsSuccess)
                {
                    Session["SystemUser"] = objSystemUser;
                    _resultEntity.IsSuccess = objSystemUser.IsSuccess;
                }
                else
                {
                    _resultEntity.IsSuccess = false;
                    _resultEntity.Result = "Invalid username and/or password.";
                    _resultEntity.IsListResult = false;
                }
            }
            catch (Exception ex)
            {
                _resultEntity.IsSuccess = false;
                _resultEntity.IsListResult = false;
                _resultEntity.Result = ex.Message;
            }

            _systemUserBLL = null;

            return Json(_resultEntity);

        }

        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            return Redirect("~/");
        }


      
    }
}