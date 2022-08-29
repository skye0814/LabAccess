using ERSBLL;
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
    public class EquipmentRequestHistoryController: CommonController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EquipmentRequestHistoryList()
        {
            return View();
        }
    }
}