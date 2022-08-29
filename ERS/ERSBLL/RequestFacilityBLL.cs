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
    public class RequestFacilityBLL
    {
        RequestFacilityDAL _requestFacilityDAL = null;
        public RequestFacilityEntity Insert(RequestFacilityEntity entity)
        {
            RequestFacilityEntity result = new RequestFacilityEntity();
            string errorMessage = string.Empty;
            result.MessageList = new List<string>();


            if (entity == null)
                entity = new RequestFacilityEntity();

            if (!entity.FacilityID.HasValue || entity.FacilityID == null)
                result.MessageList.Add(MessageUTIL.PRE_PROVIDE_INPUT + " Description.");


            if (result.MessageList.Count() == 0)
            {
                try
                {
                    _requestFacilityDAL = new RequestFacilityDAL();
                    result = _requestFacilityDAL.Insert(entity);
                    if (result.FacilityRequestID == 0)
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

                _requestFacilityDAL = null;
            }

            return result;
        }
        public RequestFacilityEntity Update(RequestFacilityEntity entity)
        {
            RequestFacilityEntity result = new RequestFacilityEntity();
            result.MessageList = new List<string>();


            if (entity == null)
                entity = new RequestFacilityEntity();

            _requestFacilityDAL = new RequestFacilityDAL();

            if (result.MessageList.Count() == 0)
            {

                try
                {
                    _requestFacilityDAL = new RequestFacilityDAL();
                    result.IsSuccess = _requestFacilityDAL.Update(entity);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                _requestFacilityDAL = null;
            }

            return result;
        }

        public IEnumerable<RequestFacilityListEntity> GetFiltered(RequestFacilityListEntity objListEntityFilter)
        {

            if (objListEntityFilter == null)
                throw new Exception(MessageUTIL.PROVIDE_VALID_FILTER + "Facility.");

            objListEntityFilter.SortDirection = ("" + objListEntityFilter.SortDirection).Trim().ToLower();

            if (objListEntityFilter.RowStart == 0)
                objListEntityFilter.NoOfRecord = DefaultValuesUTIL.TableDisplayCountPerPage;
            if (!string.IsNullOrEmpty(objListEntityFilter.SortDirection))
                if (objListEntityFilter.SortDirection != "asc" && objListEntityFilter.SortDirection != "desc")
                    objListEntityFilter.SortDirection = "";
           
            List<RequestFacilityListEntity> result = null; 

            try
            {
                _requestFacilityDAL = new RequestFacilityDAL();
                result = new List<RequestFacilityListEntity>();
                result = _requestFacilityDAL.GetFiltered(objListEntityFilter).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return result;
        }

        public RequestFacilityEntity Get(RequestFacilityEntity objEntity)
        {
            RequestFacilityEntity result = new RequestFacilityEntity();
            result.MessageList = new List<string>();
            if (result.MessageList.Count == 0)
            {
                try
                {
                    _requestFacilityDAL = new RequestFacilityDAL();
                    result = _requestFacilityDAL.Get(objEntity);
                    if (result == null)
                    {
                        result = new RequestFacilityEntity();
                        result.IsSuccess = false;
                        result.MessageList = new List<string>();
                        result.MessageList.Add("RequestGUID does not exists.");
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


                _requestFacilityDAL = null;

            }

            return result;
        }

        public ResultEntity Delete(List<int> IDs)
        {

            ResultEntity result = new ResultEntity();
            try
            {
                _requestFacilityDAL = new RequestFacilityDAL();
                result.IsSuccess = _requestFacilityDAL.BulkDelete(IDs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}
