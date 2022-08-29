using ERSDAL;
using ERSEntity;
using ERSUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSBLL
{
    public class PenaltyBLL
    {
        PenaltyDAL _PenaltyDAL = null;
        PenaltyBLL duplicateCheckBLL = null;
        public PenaltyEntity Insert(PenaltyEntity objPenaltyEntity)
        {
            PenaltyEntity result = new PenaltyEntity();
            PenaltyEntity duplicateCheck = new PenaltyEntity();
            string errorMessage = string.Empty;
            result.MessageList = new List<string>();


            if (objPenaltyEntity == null)
                objPenaltyEntity = new PenaltyEntity();

            if (result.MessageList.Count() == 0)
            {
                try
                {
                    _PenaltyDAL = new PenaltyDAL();
                    result = _PenaltyDAL.Get(objPenaltyEntity);
                    result = _PenaltyDAL.Insert(objPenaltyEntity);
                if (result.PenaltyID > 0)
                    result.IsSuccess = true;
                else
                {
                    result.IsSuccess = false;
                    result.MessageList = new List<string>();
                    result.MessageList.Add("Insert unsuccessful . Please try again.");
                }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                _PenaltyDAL = null;
            }
            else
            {
                result.MessageList = new List<string>();
                result.MessageList.Add("Category or Category Code already exists.");
                result.IsSuccess = false;
            }
            return result;
        }
        public PenaltyEntity Update(PenaltyEntity objPenaltyEntity)
        {
            PenaltyEntity result = new PenaltyEntity();
            result.MessageList = new List<string>();


            if (objPenaltyEntity == null)
                objPenaltyEntity = new PenaltyEntity();

            _PenaltyDAL = new PenaltyDAL();

            if (result.MessageList.Count() == 0)
            {

                try
                {
                    _PenaltyDAL = new PenaltyDAL();
                    result.IsSuccess = _PenaltyDAL.Update(objPenaltyEntity);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                _PenaltyDAL = null;
            }

            return result;

            throw new NotImplementedException();
        }
        public IEnumerable<PenaltyListEntity> GetFiltered(PenaltyListEntity objPenaltyListEntityFilter)
        {

            if (objPenaltyListEntityFilter == null)
                throw new Exception(MessageUTIL.PROVIDE_VALID_FILTER + "Equipment.");

            objPenaltyListEntityFilter.SortDirection = ("" + objPenaltyListEntityFilter.SortDirection).Trim().ToLower();

            if (objPenaltyListEntityFilter.RowStart == 0)
                objPenaltyListEntityFilter.NoOfRecord = DefaultValuesUTIL.TableDisplayCountPerPage;
            if (!string.IsNullOrEmpty(objPenaltyListEntityFilter.SortDirection))
                if (objPenaltyListEntityFilter.SortDirection != "asc" && objPenaltyListEntityFilter.SortDirection != "desc")
                    objPenaltyListEntityFilter.SortDirection = "";

            List<PenaltyListEntity> result = null;

            try
            {
                _PenaltyDAL = new PenaltyDAL();
                result = new List<PenaltyListEntity>();
                result = _PenaltyDAL.GetFiltered(objPenaltyListEntityFilter).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public PenaltyEntity Get(PenaltyEntity objPenaltyEntity)
        {
            PenaltyEntity result = new PenaltyEntity();
            result.MessageList = new List<string>();
            if (result.MessageList.Count == 0)
            {
                try
                {
                    _PenaltyDAL = new PenaltyDAL();
                    if (objPenaltyEntity.RequestType == "Equipment")
                        result = _PenaltyDAL.GetEquipment(objPenaltyEntity);
                    else if (objPenaltyEntity.RequestType == "Facility")
                        result = _PenaltyDAL.GetFacility(objPenaltyEntity);
                    else
                        result = _PenaltyDAL.Get(objPenaltyEntity);
                    if (result == null)
                    {
                        result = new PenaltyEntity();
                        result.IsSuccess = false;
                        result.MessageList = new List<string>();
                        result.MessageList.Add("Request does not exists.");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                _PenaltyDAL = null;
            }
            return result;
        }
        public IEnumerable<PenaltyEntity> GetList()
        {
            //List<PenaltyEntity> result = null;

            //try
            //{
            //    _PenaltyDAL = new PenaltyDAL();
            //    result = new List<PenaltyEntity>();
            //    result = _PenaltyDAL.GetList().ToList();
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //return result;
            throw new NotImplementedException();
        }

        public IEnumerable<PenaltyEntity> GetListOnlyAvailable()
        {
            //List<PenaltyEntity> result = null;

            //try
            //{
            //    _PenaltyDAL = new PenaltyDAL();
            //    result = new List<PenaltyEntity>();
            //    result = _PenaltyDAL.GetListOnlyAvailable().ToList();
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //return result;
            throw new NotImplementedException();
        }
    }
}

