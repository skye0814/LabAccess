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
    public class EquipmentItemBLL
    {
        EquipmentItemDAL _EquipmentItemDAL = null;
        EquipmentItemBLL duplicateCheckBLL = null;
        public EquipmentItemEntity Insert(EquipmentItemEntity objEquipmentItemEntity)
        {
            EquipmentItemEntity result = new EquipmentItemEntity();
            EquipmentItemEntity duplicateCheck = new EquipmentItemEntity();
            string errorMessage = string.Empty;
            result.MessageList = new List<string>();


            if (objEquipmentItemEntity == null)
                objEquipmentItemEntity = new EquipmentItemEntity();

            if (string.IsNullOrEmpty(objEquipmentItemEntity.Category))
                result.MessageList.Add(MessageUTIL.PRE_PROVIDE_INPUT + " Category.");
            duplicateCheck.ItemSerialNumber = objEquipmentItemEntity.ItemSerialNumber;
            duplicateCheckBLL = new EquipmentItemBLL();
            duplicateCheck = duplicateCheckBLL.Get(duplicateCheck);
            if (duplicateCheck == null || duplicateCheck.IsSuccess == false)
            {
                if (result.MessageList.Count() == 0)
                {
                    try
                    {
                        _EquipmentItemDAL = new EquipmentItemDAL();
                        result = _EquipmentItemDAL.Insert(objEquipmentItemEntity);
                        if (result.EquipmentItemID > 0)
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

                    _EquipmentItemDAL = null;
                }
            }
            else
            {
                result.MessageList = new List<string>();
                result.MessageList.Add("Serial Number already exists.");
                result.IsSuccess = false;
            }
            return result;
        }
        public EquipmentItemEntity Update(EquipmentItemEntity objEquipmentItemEntity)
        {
            EquipmentItemEntity result = new EquipmentItemEntity();
            EquipmentItemEntity duplicateCheck = new EquipmentItemEntity();
            result.MessageList = new List<string>();


            if (objEquipmentItemEntity == null)
                objEquipmentItemEntity = new EquipmentItemEntity();

            _EquipmentItemDAL = new EquipmentItemDAL();
            duplicateCheck.EquipmentItemID = objEquipmentItemEntity.EquipmentItemID;
            duplicateCheck.ItemSerialNumber = objEquipmentItemEntity.ItemSerialNumber;
            duplicateCheckBLL = new EquipmentItemBLL();
            duplicateCheck = duplicateCheckBLL.Get(duplicateCheck);
            if (duplicateCheck.Status == "recheck")
            {
                duplicateCheck = null;
                duplicateCheck = new EquipmentItemEntity();
                duplicateCheck.ItemSerialNumber = objEquipmentItemEntity.ItemSerialNumber;
                duplicateCheck = duplicateCheckBLL.Get(duplicateCheck);
            }
            if (duplicateCheck == null || duplicateCheck.IsSuccess == false)
            {
                if (result.MessageList.Count() == 0)
                {

                    try
                    {
                        _EquipmentItemDAL = new EquipmentItemDAL();
                        result.IsSuccess = _EquipmentItemDAL.Update(objEquipmentItemEntity);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    _EquipmentItemDAL = null;
                }
            }
            else
            {
                result.MessageList = new List<string>();
                result.MessageList.Add("Serial Number already exists.");
                result.IsSuccess = false;
            }
            return result;
        } 
            public IEnumerable<EquipmentItemListEntity> GetFiltered(EquipmentItemListEntity objEquipmentItemListEntityFilter)
        {

            if (objEquipmentItemListEntityFilter == null)
                throw new Exception(MessageUTIL.PROVIDE_VALID_FILTER + "Equipment.");

            objEquipmentItemListEntityFilter.SortDirection = ("" + objEquipmentItemListEntityFilter.SortDirection).Trim().ToLower();

            if (objEquipmentItemListEntityFilter.RowStart == 0)
                objEquipmentItemListEntityFilter.NoOfRecord = DefaultValuesUTIL.TableDisplayCountPerPage;
            if (!string.IsNullOrEmpty(objEquipmentItemListEntityFilter.SortDirection))
                if (objEquipmentItemListEntityFilter.SortDirection != "asc" && objEquipmentItemListEntityFilter.SortDirection != "desc")
                    objEquipmentItemListEntityFilter.SortDirection = "";

            List<EquipmentItemListEntity> result = null;

            try
            {
                _EquipmentItemDAL = new EquipmentItemDAL();
                result = new List<EquipmentItemListEntity>();
                result = _EquipmentItemDAL.GetFiltered(objEquipmentItemListEntityFilter).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return result;
        }
        public EquipmentItemEntity Get(EquipmentItemEntity objEquipmentItemEntity)
        {
            EquipmentItemEntity result = new EquipmentItemEntity();
            result.MessageList = new List<string>();
            if (result.MessageList.Count == 0)
            {
                try
                {
                    _EquipmentItemDAL = new EquipmentItemDAL();
                    result = _EquipmentItemDAL.Get(objEquipmentItemEntity);
                    if (result == null)
                    {
                        result = new EquipmentItemEntity();
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


                _EquipmentItemDAL = null;

            }

            return result;
        }
    }
}
