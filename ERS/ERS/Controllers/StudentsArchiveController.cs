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
    public class StudentsArchiveController : CommonController
    {
        StudentRegistrationController studentRegController = new StudentRegistrationController();
        StudentEntity _studentEntity = null;

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StudentsArchiveList()
        {
            if (!_objSystemUser.isLabPersonnel)
            {
                return new HttpStatusCodeResult(401);
            }
            return View();
        }

        public ActionResult StudentsArchiveAdd()
        {
            studentRegController.PopulateDropdown();
            return View();
        }

        public ActionResult StudentsArchiveEdit(int ID = 0, bool isArchive = true)
        {
            var result = studentRegController.StudentRegistrationEdit(ID, isArchive);
            return result;
        }

        public JsonResult StudentsArchiveDelete(List<int> SystemUserIDs)
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
                var result = studentRegController.DeleteStudentFromArchive(SystemUserIDs);
                return result;
            }
        }
    }
}