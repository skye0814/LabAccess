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
    public class FacilityBLL
    {
        FacilityDAL _FacilityDAL = null;
        FacilityBLL duplicateCheckBLL = null;
        public FacilityEntity Insert(FacilityEntity objFacilityEntity)
        {
            FacilityEntity result = new FacilityEntity();
            FacilityEntity duplicateCheck = new FacilityEntity();
            string errorMessage = string.Empty;
            result.MessageList = new List<string>();


            if (objFacilityEntity == null)
                objFacilityEntity = new FacilityEntity();

            if (string.IsNullOrEmpty(objFacilityEntity.RoomNumber))
                result.MessageList.Add(MessageUTIL.PRE_PROVIDE_INPUT + " Room Number.");
            duplicateCheck.RoomNumber = objFacilityEntity.RoomNumber;
            duplicateCheckBLL = new FacilityBLL();
            duplicateCheck = duplicateCheckBLL.Get(duplicateCheck);
            if (duplicateCheck.IsSuccess)
            {
                result.MessageList.Add("Room Number already registered.");
                result.IsSuccess = false;
            }
            if (result.MessageList.Count() == 0)
            {
                try
                {
                    _FacilityDAL = new FacilityDAL();
                    result = _FacilityDAL.Insert(objFacilityEntity);
                    if (result.FacilityID > 0)
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

                _FacilityDAL = null;
            }
            return result;
        }
        public FacilityEntity Update(FacilityEntity objFacilityEntity)
        {
            FacilityEntity result = new FacilityEntity();
            FacilityEntity duplicateCheck = new FacilityEntity();
            result.MessageList = new List<string>();


            if (objFacilityEntity == null)
                objFacilityEntity = new FacilityEntity();

            _FacilityDAL = new FacilityDAL();
            duplicateCheck.FacilityID = objFacilityEntity.FacilityID;
            duplicateCheck.RoomNumber = objFacilityEntity.RoomNumber;
            duplicateCheckBLL = new FacilityBLL();
            duplicateCheck = duplicateCheckBLL.Get(duplicateCheck);
            if (duplicateCheck.Status == "recheck")
            {
                duplicateCheck = null;
                duplicateCheck = new FacilityEntity
                {
                    FacilityID = objFacilityEntity.FacilityID,
                    RoomNumber = objFacilityEntity.RoomNumber,
                    Mode = "check"

                };
                duplicateCheck = duplicateCheckBLL.Get(duplicateCheck);
                if (!duplicateCheck.IsSuccess)
                {
                    result.MessageList.Add("Room Number already registered.");
                    result.IsSuccess = false;
                }
            }
            if (result.MessageList.Count() == 0)
            {

                try
                {
                    _FacilityDAL = new FacilityDAL();
                    result.IsSuccess = _FacilityDAL.Update(objFacilityEntity);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                _FacilityDAL = null;
            }
            return result;
        }
        public IEnumerable<FacilityListEntity> GetFiltered(FacilityListEntity objFacilityListEntityFilter)
        {

            if (objFacilityListEntityFilter == null)
                throw new Exception(MessageUTIL.PROVIDE_VALID_FILTER + "Facility.");

            objFacilityListEntityFilter.SortDirection = ("" + objFacilityListEntityFilter.SortDirection).Trim().ToLower();

            if (objFacilityListEntityFilter.RowStart == 0)
                objFacilityListEntityFilter.NoOfRecord = DefaultValuesUTIL.TableDisplayCountPerPage;
            if (!string.IsNullOrEmpty(objFacilityListEntityFilter.SortDirection))
                if (objFacilityListEntityFilter.SortDirection != "asc" && objFacilityListEntityFilter.SortDirection != "desc")
                    objFacilityListEntityFilter.SortDirection = "";

            List<FacilityListEntity> result = null;

            try
            {
                _FacilityDAL = new FacilityDAL();
                result = new List<FacilityListEntity>();
                result = _FacilityDAL.GetFiltered(objFacilityListEntityFilter).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return result;
        }
        public FacilityEntity Get(FacilityEntity objFacilityEntity)
        {
            FacilityEntity result = new FacilityEntity();
            result.MessageList = new List<string>();
            if (result.MessageList.Count == 0)
            {
                try
                {
                    _FacilityDAL = new FacilityDAL();
                    result = _FacilityDAL.Get(objFacilityEntity);
                    if (result == null)
                    {
                        result = new FacilityEntity();
                        result.IsSuccess = false;
                        result.MessageList = new List<string>();
                        result.MessageList.Add("Facility does not exists.");
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


                _FacilityDAL = null;

            }

            return result;
        }
        public IEnumerable<FacilityEntity> GetListOnlyAvailable()
        {
            List<FacilityEntity> result = null;

            try
            {
                _FacilityDAL = new FacilityDAL();
                result = new List<FacilityEntity>();
                result = _FacilityDAL.GetListOnlyAvailable().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public FacilityEntity InsertSchedule(FacilityEntity objFacilityEntity)
        {
            FacilityEntity result = new FacilityEntity();
            string errorMessage = string.Empty;
            result.MessageList = new List<string>();


            if (objFacilityEntity == null)
                objFacilityEntity = new FacilityEntity();

            try
            {
                _FacilityDAL = new FacilityDAL();
                result = _FacilityDAL.InsertSchedule(objFacilityEntity);
                if (result.ScheduleID > 0)
                    result.IsSuccess = true;
                else
                {
                    result.IsSuccess = false;
                    result.MessageList = new List<string>();
                    result.MessageList.Add("Something went wrong. Schedule not saved. Schedule ID is 0.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            _FacilityDAL = null;
            return result;
        }

        public FacilityEntity DeleteSchedule(FacilityEntity objFacilityEntity)
        {
            FacilityEntity result = new FacilityEntity();
            string errorMessage = string.Empty;
            result.MessageList = new List<string>();


            if (objFacilityEntity == null)
                objFacilityEntity = new FacilityEntity();

            try
            {
                _FacilityDAL = new FacilityDAL();
                result = _FacilityDAL.DeleteSchedule(objFacilityEntity);
                if (!result.IsSuccess)
                {
                    result.MessageList = new List<string>();
                    result.MessageList.Add("Something went wrong. Schedule not deleted.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            _FacilityDAL = null;
            return result;
        }
        public IEnumerable<FacilityListEntity> GetScheduleList(FacilityListEntity objListEntityFilter)
        {

            if (objListEntityFilter == null)
                throw new Exception(MessageUTIL.PROVIDE_VALID_FILTER + "Facility.");

            objListEntityFilter.SortDirection = ("" + objListEntityFilter.SortDirection).Trim().ToLower();

            if (objListEntityFilter.RowStart == 0)
                objListEntityFilter.NoOfRecord = DefaultValuesUTIL.TableDisplayCountPerPage;
            if (!string.IsNullOrEmpty(objListEntityFilter.SortDirection))
                if (objListEntityFilter.SortDirection != "asc" && objListEntityFilter.SortDirection != "desc")
                    objListEntityFilter.SortDirection = "";

            List<FacilityListEntity> result = null;

            try
            {
                _FacilityDAL = new FacilityDAL();
                result = new List<FacilityListEntity>();
                result = _FacilityDAL.GetScheduleList(objListEntityFilter).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return result;
        }
    }
}
