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
    public class EquipmentBLL
    {
        EquipmentDAL _EquipmentDAL = null;
        public EquipmentEntity Insert(EquipmentEntity objEquipmentEntity)
        {
            EquipmentEntity result = new EquipmentEntity();
            string errorMessage = string.Empty;
            result.MessageList = new List<string>();


            if (objEquipmentEntity == null)
                objEquipmentEntity = new EquipmentEntity();

            if (string.IsNullOrEmpty(objEquipmentEntity.EquipmentCode))
                result.MessageList.Add(MessageUTIL.PRE_PROVIDE_INPUT + " Equipment Code.");


            if (result.MessageList.Count() == 0)
            {
                try
                {
                    _EquipmentDAL = new EquipmentDAL();
                    result = _EquipmentDAL.Insert(objEquipmentEntity);  
                    if (result.EquipmentID > 0)
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

                _EquipmentDAL = null;
            }

            return result;
        }
        public EquipmentEntity Update(EquipmentEntity objEquipmentEntity)
        {
            EquipmentEntity result = new EquipmentEntity();
            result.MessageList = new List<string>();


            if (objEquipmentEntity == null)
                objEquipmentEntity = new EquipmentEntity();

            _EquipmentDAL = new EquipmentDAL();

            if (result.MessageList.Count() == 0)
            {

                try
                {
                    _EquipmentDAL = new EquipmentDAL();
                    result.IsSuccess = _EquipmentDAL.Update(objEquipmentEntity);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                _EquipmentDAL = null;
            }

            return result;

        }
        public IEnumerable<EquipmentListEntity> GetFiltered(EquipmentListEntity objEquipmentListEntityFilter)
        {

            if (objEquipmentListEntityFilter == null)
                throw new Exception(MessageUTIL.PROVIDE_VALID_FILTER + "Equipment.");

            objEquipmentListEntityFilter.SortDirection = ("" + objEquipmentListEntityFilter.SortDirection).Trim().ToLower();

            if (objEquipmentListEntityFilter.RowStart == 0)
                objEquipmentListEntityFilter.NoOfRecord = DefaultValuesUTIL.TableDisplayCountPerPage;
            if (!string.IsNullOrEmpty(objEquipmentListEntityFilter.SortDirection))
                if (objEquipmentListEntityFilter.SortDirection != "asc" && objEquipmentListEntityFilter.SortDirection != "desc")
                    objEquipmentListEntityFilter.SortDirection = "";

            List<EquipmentListEntity> result = null;

            try
            {
                _EquipmentDAL = new EquipmentDAL();
                result = new List<EquipmentListEntity>();
                result = _EquipmentDAL.GetFiltered(objEquipmentListEntityFilter).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return result;
        }
        public EquipmentEntity Get(EquipmentEntity objEquipmentEntity)
        {
            EquipmentEntity result = new EquipmentEntity();
            result.MessageList = new List<string>();
            if (result.MessageList.Count == 0)
            {
                try
                {
                    _EquipmentDAL = new EquipmentDAL();
                    result = _EquipmentDAL.Get(objEquipmentEntity);
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


                _EquipmentDAL = null;

            }

            return result;
        }
    }
}
