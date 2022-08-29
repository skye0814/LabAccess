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
    public class RequestEquipmentBLL
    {
        RequestEntityDAL _requestEquipmentDAL = null;
        RequestDetailsDAL _requestDetailsDAL = null;

        public RequestsEntity InsertRequests(RequestsEntity entity)
        {
            RequestsEntity result = new RequestsEntity();
            string errorMessage = string.Empty;
            result.MessageList = new List<string>();


            if (entity == null)
                entity = new RequestsEntity();

            if (result.MessageList.Count() == 0)
            {
                try
                {
                    _requestEquipmentDAL = new RequestEntityDAL();
                    result = _requestEquipmentDAL.Insert(entity);
                    if (result.RequestID == 0)
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

                _requestEquipmentDAL = null;
            }

            return result;
        }

        public RequestDetailsEntity InsertRequestDetails(RequestDetailsEntity entity)
        {
            RequestDetailsEntity result = new RequestDetailsEntity();
            string errorMessage = string.Empty;
            result.MessageList = new List<string>();


            if (entity == null)
                entity = new RequestDetailsEntity();

            if (result.MessageList.Count() == 0)
            {
                try
                {
                    _requestDetailsDAL = new RequestDetailsDAL();
                    result = _requestDetailsDAL.Insert(entity);
                    if (result.RequestDetailsID == 0)
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

                _requestDetailsDAL = null;
            }

            return result;
        }

        public RequestsEntity Update(RequestsEntity entity)
        {
            RequestsEntity result = new RequestsEntity();
            result.MessageList = new List<string>();


            if (entity == null)
                entity = new RequestsEntity();

            _requestEquipmentDAL = new RequestEntityDAL();

            if (result.MessageList.Count() == 0)
            {

                try
                {
                    _requestEquipmentDAL = new RequestEntityDAL();
                    result.IsSuccess = _requestEquipmentDAL.Update(entity);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                _requestEquipmentDAL = null;
            }

            return result;
        }
        public IEnumerable<RequestsListEntity> GetFiltered(RequestsListEntity objListEntityFilter)
        {

            if (objListEntityFilter == null)
                throw new Exception(MessageUTIL.PROVIDE_VALID_FILTER + "Equipment.");

            objListEntityFilter.SortDirection = ("" + objListEntityFilter.SortDirection).Trim().ToLower();

            if (objListEntityFilter.RowStart == 0)
                objListEntityFilter.NoOfRecord = DefaultValuesUTIL.TableDisplayCountPerPage;
            if (!string.IsNullOrEmpty(objListEntityFilter.SortDirection))
                if (objListEntityFilter.SortDirection != "asc" && objListEntityFilter.SortDirection != "desc")
                    objListEntityFilter.SortDirection = "";

            List<RequestsListEntity> result = null;

            try
            {
                _requestEquipmentDAL = new RequestEntityDAL();
                result = new List<RequestsListEntity>();
                result = _requestEquipmentDAL.GetFiltered(objListEntityFilter).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return result;
        }

        // GET RequestsEntity
        public RequestsEntity Get(RequestsEntity objEntity)
        {
            RequestsEntity result = new RequestsEntity();
            result.MessageList = new List<string>();
            if (result.MessageList.Count == 0)
            {
                try
                {
                    _requestEquipmentDAL = new RequestEntityDAL();
                    result = _requestEquipmentDAL.Get(objEntity);
                    if (result == null)
                    {
                        result.IsSuccess = false;
                        result.MessageList = new List<string>();
                        result.MessageList.Add("Equipment does not exists.");
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


                _requestEquipmentDAL = null;

            }

            return result;
        }

        // GET RequestDetailsEntity
        public RequestDetailsEntity GetRequestDetails(RequestDetailsEntity objEntity)
        {
            RequestDetailsEntity result = new RequestDetailsEntity();
            result.MessageList = new List<string>();
            if (result.MessageList.Count == 0)
            {
                try
                {
                    _requestDetailsDAL = new RequestDetailsDAL();
                    result = _requestDetailsDAL.Get(objEntity);
                    if (result == null)
                    {
                        result.IsSuccess = false;
                        result.MessageList = new List<string>();
                        result.MessageList.Add("Equipment does not exists.");
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


                _requestDetailsDAL = null;

            }

            return result;
        }
        public ResultEntity Delete(List<int> IDs)
        {

            ResultEntity result = new ResultEntity();
            try
            {
                _requestEquipmentDAL = new RequestEntityDAL();
                result.IsSuccess = _requestEquipmentDAL.BulkDelete(IDs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}
