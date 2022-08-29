using ERSEntity;
using ERSDAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERS.Controllers
{
    public class SecurityController : Controller
    {
        public SystemUserEntity _objSystemUser = null;
        PenaltyDAL _penaltyDAL = null;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (_objSystemUser == null)
                _objSystemUser = (SystemUserEntity)Session["SystemUser"];




            if (_objSystemUser == null)
            {
                //// Test 1
                //// Remove or comment these lines for live or uat environment
                //_objSystemUser = new SystemUserEntity();
                //_objSystemUser.Id = 1;
                //_objSystemUser.Username = "Dev";
                //_objSystemUser.CompanyId = 1;
                //_objSystemUser.IsPasswordChanged = true;
                //Session["SystemUser"] = _objSystemUser;

                //_objSystemUser = null;
                // End of Test 1

                // Uncomment for live or uat environment
                if (Request.IsAjaxRequest())
                {
                    throw new Exception("Session Expired");
                }
                else
                {
                    Response.Redirect("~/");
                }
            }
            else
            {
                if (_objSystemUser.isLabPersonnel)
                {
                    Session.Timeout = 30;
                }
                else
                {
                    Session.Timeout = 5;
                }
                // For notif panel
                _penaltyDAL = new PenaltyDAL();
                var allPenalties = _penaltyDAL.GetAllActiveRequestPenaltyBySystemUserID(_objSystemUser.ID);
                ViewBag.AllPenalties = allPenalties;

                AssignedPartialLoadPage(filterContext);


            }
        }
    

        private void AssignedPartialLoadPage(ActionExecutingContext filterContext)
        {
            // Assign what partial view would load if the page is refereshed.
            try
            {
                if (((ReflectedActionDescriptor)filterContext.ActionDescriptor).MethodInfo.ReturnType.Name == "ActionResult")
                {
                    Dictionary<string, string> pageHistory = (Dictionary<string, string>)Session["PageHistory"];
                    string controller = "" + filterContext.RouteData.Values["controller"];
                    string url = "" + Request.RawUrl;

                    if (pageHistory == null)
                        pageHistory = new Dictionary<string, string>();

                    if (Request.IsAjaxRequest())
                    {
                        // Can be used for testing
                        //TempData["CurrentPage"] = Request.RawUrl;
                        //TempData["CurrentController"] = filterContext.RouteData.Values["controller"];

                        if (!pageHistory.ContainsKey(controller))
                            pageHistory.Add(controller, url);
                        else
                            pageHistory[controller] = url;
                    }
                    else
                    {
                        // Can be used for testing
                        //if (("" + TempData["CurrentController"]) != ("" + filterContext.RouteData.Values["controller"]))
                        //{
                        //    TempData["CurrentPage"] = "";
                        //}

                        if (!pageHistory.ContainsKey(controller))
                            pageHistory.Add(controller, "");
                    }

                    Session["PageHistory"] = pageHistory;
                }
            }
            catch
            {
                // Can be used for testing
                //TempData["CurrentPage"] = "";

                Session["PageHistory"] = "";
            }
        }

      

    }
}