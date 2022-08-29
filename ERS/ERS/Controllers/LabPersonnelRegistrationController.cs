using ERSBLL;
using ERSEntity;
using ERSEntity.Entity;
using ERSUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERS.Controllers
{
    public class LabPersonnelRegistrationController : CommonController
    {
        LabPersonnelRegistrationBLL _labPersonnelRegistrationBLL = null;
        LabPersonnelEntity _labPersonnelEntity = null;
        // GET: LabPersonnelRegistration

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LabPersonnelRegistrationAdd()
        {
            if (!_objSystemUser.isLabPersonnel || !_objSystemUser.isStudent)
            {
                return new HttpStatusCodeResult(401);
            }
            return View();
        }

        public ActionResult LabPersonnelRegistrationList()
        {
            if (!_objSystemUser.isLabPersonnel)
            {
                return new HttpStatusCodeResult(401);
            }
            return View();
        }

        public ActionResult LabPersonnelRegistrationEdit(int ID = 0, bool isArchive = false)
        {
            try
            {
                _exception = null;

                _labPersonnelRegistrationBLL = new LabPersonnelRegistrationBLL();
                _labPersonnelEntity = new LabPersonnelEntity();
                _labPersonnelEntity.ID = ID;

                if (isArchive == true)
                    _labPersonnelEntity.isArchive = true;
                else if (isArchive == false)
                    _labPersonnelEntity.isArchive = false;

                _labPersonnelEntity = _labPersonnelRegistrationBLL.Get(_labPersonnelEntity);
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
            _labPersonnelRegistrationBLL = null;

            if (_exception != null)
            {
                _resultEntity.IsListResult = false;
                _resultEntity.Result = _exception.Message;

                return Json(_resultEntity, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (_labPersonnelEntity.IsSuccess)
                {
                    return PartialView(_labPersonnelEntity);
                }
                else
                {
                    _resultEntity.IsSuccess = _labPersonnelEntity.IsSuccess;
                    _resultEntity.Result = _labPersonnelEntity.MessageList;
                    _resultEntity.IsListResult = false;

                    return Json(_resultEntity, JsonRequestBehavior.AllowGet);
                }
            }

        }

        public JsonResult GetLabPersonnelList(string sidx = "", string sord = "", int page = 1, int rows = 10
          , LabPersonnelListEntity objLabPersonnelListEntityFilter = null)
        {
            IEnumerable<LabPersonnelListEntity> objLabPersonnelListTableView = null;
            LabPersonnelListEntity objLabPersonnelListTableFilter = null;

            int totalRecords = 0;
            int noOfPages = 1;
            int rowStart = 1;

            try
            {
                rowStart = page > 1 ? page * rows - rows + 1 : rowStart;

                objLabPersonnelListTableFilter = new LabPersonnelListEntity();
                objLabPersonnelListTableFilter.RowStart = rowStart;
                objLabPersonnelListTableFilter.NoOfRecord = rows;
                objLabPersonnelListTableFilter.SortColumn = sidx == "" ? 0 : Convert.ToInt32(sidx);
                objLabPersonnelListTableFilter.SortDirection = sord;
                objLabPersonnelListTableFilter.MiddleName = objLabPersonnelListEntityFilter.MiddleName;
                objLabPersonnelListTableFilter.FirstName = objLabPersonnelListEntityFilter.FirstName;
                objLabPersonnelListTableFilter.LastName = objLabPersonnelListEntityFilter.LastName;
                objLabPersonnelListTableFilter.isArchive = objLabPersonnelListEntityFilter.isArchive;

                _labPersonnelRegistrationBLL = new LabPersonnelRegistrationBLL();
                objLabPersonnelListTableView = _labPersonnelRegistrationBLL.GetFiltered(objLabPersonnelListTableFilter).ToList();

                if (objLabPersonnelListTableView != null && objLabPersonnelListTableView.Count() > 0)
                {
                    totalRecords = objLabPersonnelListTableView.FirstOrDefault().TotalRecord;
                    noOfPages = (int)Math.Ceiling((float)totalRecords / (float)rows);

                    if (noOfPages == 0)
                        noOfPages = 1;
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                _resultEntity.IsListResult = false;
                _resultEntity.Result = ex.Message;

                return Json(_resultEntity, JsonRequestBehavior.AllowGet);
            }

            _labPersonnelRegistrationBLL = null;

            objLabPersonnelListTableFilter = null;


            var jsonData = new
            {
                total = noOfPages,
                page,
                sort = sidx,
                records = totalRecords - 1,
                rows = objLabPersonnelListTableView
            };

            objLabPersonnelListTableView = null;

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveLabPersonnelRegistration(LabPersonnelEntity objLabPersonnelEntity)
        {
            try
            {
                _labPersonnelRegistrationBLL = new LabPersonnelRegistrationBLL();
                _labPersonnelEntity = new LabPersonnelEntity();

                if (objLabPersonnelEntity.ID == 0)
                {
                    objLabPersonnelEntity.CreatedBy = _objSystemUser.ID;
                    _labPersonnelEntity = _labPersonnelRegistrationBLL.Insert(objLabPersonnelEntity);
                }
                else
                {
                    if (objLabPersonnelEntity.isArchive)
                    {
                        objLabPersonnelEntity.CreatedBy = _objSystemUser.ID;
                        _labPersonnelEntity = _labPersonnelRegistrationBLL.Insert(objLabPersonnelEntity);
                    }
                    else
                    {
                        _labPersonnelEntity = _labPersonnelRegistrationBLL.Update(objLabPersonnelEntity);
                    }
                }

                if (_labPersonnelEntity.IsSuccess)
                {
                    _resultEntity.IsSuccess = true;
                }
                else
                {
                    _resultEntity.IsSuccess = false;
                    _resultEntity.IsListResult = true;
                    _resultEntity.Result = _labPersonnelEntity.MessageList;
                }
            }
            catch (Exception ex)
            {
                _resultEntity.IsSuccess = false;
                _resultEntity.Result = ex.Message;
            }

            _labPersonnelRegistrationBLL = null;

            return Json(_resultEntity);
        }

        public JsonResult MoveLabPersonnelToArchive(List<int> SystemUserIDs)
        {

            _resultEntity = new ResultEntity();

            try
            {
                if (SystemUserIDs == null || SystemUserIDs.Count < 1)
                {
                    _resultEntity.IsSuccess = false;
                    _resultEntity.IsListResult = false;
                    _resultEntity.Result = MessageUTIL.NO_RECORD;
                }
                else
                {
                    _labPersonnelRegistrationBLL = new LabPersonnelRegistrationBLL();
                    foreach (var SystemUserID in SystemUserIDs)
                    {
                        _resultEntity = _labPersonnelRegistrationBLL.MoveToArchive(SystemUserID);

                        if (_resultEntity.IsSuccess)
                        {
                            _resultEntity.IsSuccess = _resultEntity.IsSuccess;
                            _resultEntity.IsListResult = false;
                            _resultEntity.Result = SystemUserIDs.Count + MessageUTIL.SUFF_SUCESS_ARCHIVE;
                        }
                        else
                        {
                            _resultEntity.IsSuccess = _resultEntity.IsSuccess;
                            _resultEntity.IsListResult = false;
                            _resultEntity.Result = MessageUTIL.NO_RECORD_ARCHIVED;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _resultEntity.IsSuccess = false;
                _resultEntity.Result = ex.Message;
            }

            _labPersonnelRegistrationBLL = null;

            return Json(_resultEntity);
        }

        public JsonResult DeleteLabPersonnelFromArchive(List<int> SystemUserIDs)
        {

            _resultEntity = new ResultEntity();

            try
            {
                if (SystemUserIDs == null || SystemUserIDs.Count < 1)
                {
                    _resultEntity.IsSuccess = false;
                    _resultEntity.IsListResult = false;
                    _resultEntity.Result = MessageUTIL.NO_RECORD;
                }
                else
                {
                    _labPersonnelRegistrationBLL = new LabPersonnelRegistrationBLL();
                    foreach (var SystemUserID in SystemUserIDs)
                    {
                        _resultEntity = _labPersonnelRegistrationBLL.DeleteFromArchive(SystemUserID);

                        if (_resultEntity.IsSuccess)
                        {
                            _resultEntity.IsSuccess = _resultEntity.IsSuccess;
                            _resultEntity.IsListResult = false;
                            _resultEntity.Result = SystemUserIDs.Count + MessageUTIL.SUFF_SUCESS_DELETE;
                        }
                        else
                        {
                            _resultEntity.IsSuccess = _resultEntity.IsSuccess;
                            _resultEntity.IsListResult = false;
                            _resultEntity.Result = MessageUTIL.NO_RECORD_DELETED;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _resultEntity.IsSuccess = false;
                _resultEntity.Result = ex.Message;
            }

            _labPersonnelRegistrationBLL = null;

            return Json(_resultEntity);
        }

    }
}