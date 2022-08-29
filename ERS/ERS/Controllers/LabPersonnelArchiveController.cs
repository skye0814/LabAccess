using ERSBLL;
using ERS;
using ERSDAL;
using ERSEntity;
using ERSEntity.Entity;
using ERSUtil;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERS.Controllers
{
    public class LabPersonnelArchiveController : CommonController
    {
        LabPersonnelRegistrationController labPersonnelRegController = new LabPersonnelRegistrationController();
        LabPersonnelEntity objEntity = null;

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LabPersonnelArchiveList()
        {
            if (!_objSystemUser.isLabPersonnel || !_objSystemUser.isStudent)
            {
                return new HttpStatusCodeResult(401);
            }
            return View();
        }

        public ActionResult LabPersonnelArchiveAdd()
        {
            return View();
        }

        public ActionResult LabPersonnelArchiveEdit(int ID = 0, bool isArchive = true)
        {
            var result = labPersonnelRegController.LabPersonnelRegistrationEdit(ID, isArchive);
            return result;
        }

        public JsonResult LabPersonnelArchiveDelete(List<int> SystemUserIDs)
        {
            if (!_objSystemUser.isLabPersonnel || !_objSystemUser.isStudent)
            {
                ResultEntity result = new ResultEntity();
                result.IsSuccess = false;
                result.IsListResult = false;
                result.Result = "Unauthorized Access";
                return Json(result);
            }
            else
            {
                var result = labPersonnelRegController.DeleteLabPersonnelFromArchive(SystemUserIDs);
                return result;
            }
        }
    }
}