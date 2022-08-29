using ERSBLL;
using ERSDAL;
using ERSEntity;
using ERSEntity.Entity;
using ERSUtil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace ERS.Controllers
{
    public class ForgotPasswordController : Controller
    {
        StudentEntity studentEntity = null;
        StudentRegistrationDAL objStudentDAL = null;
        SystemUserBLL systemUserBLL = null;
        ResultEntity objResultEntity = null;

        // GET: ForgotPassword
        public ActionResult Index()
        {
            return View();
        }

        public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "ResetPassword", string username = "")
        {
            var verifyUrl = "/ForgotPassword/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("cpelabaccess@gmail.com", "CpE LabAccess");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "lkemhcoaxwbzhmay";

            string subject = "";
            string body = "";
            if (emailFor == "ResetPassword")
            {
                subject = "Reset Password";
                //body = "Hello, <span style='font-weight:bold; color: orange;'>" + username + "!</span><br/><br/>We got a request to reset your account password for CPE LabAccess. " +
                //    "If you did not send such request, you can safely ignore this message and delete this from your inbox. Otherwise, please click on the link below to reset your password." +
                //    "<br/><br/><a href=" + link + ">Click here to reset your password</a>";

                string FilePath = Server.MapPath("~/Content/emailtemplate.html");
                StreamReader str = new StreamReader(FilePath);
                string MailText = str.ReadToEnd();
                str.Close();

                MailText = MailText.Replace("@username", username);
                MailText = MailText.Replace("@resetPassLink", link);

                body = MailText;
            }

            var credentials = new System.Net.NetworkCredential(fromEmail.Address, fromEmailPassword);

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(fromEmail.Address);
                mail.To.Add(emailID);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = credentials;
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }

        [HttpPost]
        public JsonResult ForgotPassword(string EmailID)
        {
            //Verify Email ID
            //Generate Reset password link 
            //Send Email 
            string message = "";

            objStudentDAL = new StudentRegistrationDAL();
            studentEntity = new StudentEntity();
            studentEntity.EmailAddress = EmailID;

            var studentUser = objStudentDAL.GetStudentExistingCheck(studentEntity);

            if (studentUser != null)
            {
                string resetCode = Guid.NewGuid().ToString();
                SendVerificationLinkEmail(studentUser.EmailAddress, resetCode, "ResetPassword", studentUser.UserName);

                studentEntity.ResetPasswordCode = resetCode;
                objStudentDAL.UpdateStudentAccountSetResetPasswordCodeByEmailAddress(studentEntity);

                message = "<span style='color: black'>Reset password link has been sent to your email. If you do not see it in your primary, check your Spam folder</span>";

                ResultEntity result = new ResultEntity();
                result.IsSuccess = false;
                result.IsListResult = false;
                result.Result = message;
                return Json(result);
            }
            else
            {
                message = "<span style='color: red'>Account with the email you entered does not exist</span>";
                ResultEntity result = new ResultEntity();
                result.IsSuccess = false;
                result.IsListResult = false;
                result.Result = message;
                return Json(result);
            }
        }

        public ActionResult ResetPassword(string id)
        {
            //Verify the reset password link
            //Find account associated with this link
            //redirect to reset password page
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }

            StudentEntity studentEntity = new StudentEntity();
            studentEntity.ResetPasswordCode = id;
            objStudentDAL = new StudentRegistrationDAL();
            var systemUser = objStudentDAL.GetStudentExistingCheck(studentEntity);

            if(systemUser != null)
            {
                SystemUserChangePasswordEntity changePasswordEntity = new SystemUserChangePasswordEntity();
                changePasswordEntity.ResetPasswordCode = id;

                return View(changePasswordEntity);
            }
            else
            {
                return new HttpStatusCodeResult(404);
            }
        }

        [HttpPost]
        public JsonResult ChangePasswordByResetPasswordCode(SystemUserChangePasswordEntity objEntity)
        {
            try
            {
                objResultEntity = new ResultEntity();
                var err = string.Empty;
                var encrypUtil = new EncryptionUTIL();

                var newPassword = encrypUtil.Encode(objEntity.NewPassword, ref err);
                SystemUserEntity _systemUserEntity = new SystemUserEntity();
                _systemUserEntity.Password = newPassword;
                _systemUserEntity.ResetPasswordCode = objEntity.ResetPasswordCode;
                systemUserBLL = new SystemUserBLL();
                _systemUserEntity = systemUserBLL.UpdatePasswordByResetPasswordCode(_systemUserEntity);
                if (_systemUserEntity.IsSuccess)
                {
                    objResultEntity.IsSuccess = true;
                }
                else
                {
                    objResultEntity.IsSuccess = false;
                    objResultEntity.IsListResult = true;
                    objResultEntity.Result = _systemUserEntity.MessageList;
                }
            }
            catch(Exception error)
            {
                objResultEntity.IsSuccess = false;
                objResultEntity.Result = error.Message;
            }

            systemUserBLL = null;
            return Json(objResultEntity);
        }
    }
}