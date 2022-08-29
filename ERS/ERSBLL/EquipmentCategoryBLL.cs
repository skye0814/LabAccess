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
    public class EquipmentCategoryBLL
    {
        EquipmentCategoryDAL _EquipmentCategoryDAL = null;
        EquipmentCategoryBLL duplicateCheckBLL = null;
        public EquipmentCategoryEntity Insert(EquipmentCategoryEntity objEquipmentCategoryEntity)
        {
            EquipmentCategoryEntity result = new EquipmentCategoryEntity();
            EquipmentCategoryEntity duplicateCheck = null;
            string errorMessage = string.Empty;
            result.MessageList = new List<string>();


            if (objEquipmentCategoryEntity == null)
                objEquipmentCategoryEntity = new EquipmentCategoryEntity();

            if (string.IsNullOrEmpty(objEquipmentCategoryEntity.Category))
                result.MessageList.Add(MessageUTIL.PRE_PROVIDE_INPUT + " Category.");

            duplicateCheckBLL = new EquipmentCategoryBLL();
            duplicateCheck = new EquipmentCategoryEntity
            {
                Category = objEquipmentCategoryEntity.Category
            };
            duplicateCheck = duplicateCheckBLL.Get(duplicateCheck);
            if (duplicateCheck.IsSuccess)
            {
                result.MessageList.Add("Category already in use.");
                result.IsSuccess = false;
            }
            duplicateCheck = new EquipmentCategoryEntity
            {
                CategoryCode = objEquipmentCategoryEntity.CategoryCode
            };
            duplicateCheck = duplicateCheckBLL.Get(duplicateCheck);
            if (duplicateCheck.IsSuccess)
            {
                result.MessageList.Add("Category code already in use.");
                result.IsSuccess = false;
            }
            if (result.MessageList.Count() == 0)
            {
                try
                {
                    _EquipmentCategoryDAL = new EquipmentCategoryDAL();
                    result = _EquipmentCategoryDAL.Insert(objEquipmentCategoryEntity);
                    if (result.EquipmentCategoryID > 0)
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

                _EquipmentCategoryDAL = null;
            }
            return result;
        }
        public EquipmentCategoryEntity Update(EquipmentCategoryEntity objEquipmentCategoryEntity)
        {
            EquipmentCategoryEntity duplicateCheck = new EquipmentCategoryEntity();
            EquipmentCategoryBLL _duplicateCheckBLL = new EquipmentCategoryBLL();
            EquipmentCategoryEntity result = new EquipmentCategoryEntity();
            result.MessageList = new List<string>();


            if (objEquipmentCategoryEntity == null)
                objEquipmentCategoryEntity = new EquipmentCategoryEntity();

            _EquipmentCategoryDAL = new EquipmentCategoryDAL();
            duplicateCheck.EquipmentCategoryID = objEquipmentCategoryEntity.EquipmentCategoryID;
            duplicateCheck.Category = objEquipmentCategoryEntity.Category;
            duplicateCheck.CategoryCode = objEquipmentCategoryEntity.CategoryCode;
            duplicateCheck = _duplicateCheckBLL.Get(duplicateCheck);
            if (duplicateCheck.Status == "recheck")
            {
                result.MessageList = new List<string>();
                duplicateCheck = new EquipmentCategoryEntity
                {
                    EquipmentCategoryID = objEquipmentCategoryEntity.EquipmentCategoryID,
                    Category = objEquipmentCategoryEntity.Category,
                    Mode = "check"
                };
                duplicateCheck = _duplicateCheckBLL.Get(duplicateCheck);
                if (!duplicateCheck.IsSuccess)
                {
                    result.MessageList.Add("Category already in use.");
                    result.IsSuccess = false;
                }
                duplicateCheck = new EquipmentCategoryEntity
                {
                    EquipmentCategoryID = objEquipmentCategoryEntity.EquipmentCategoryID,
                    CategoryCode = objEquipmentCategoryEntity.CategoryCode,
                    Mode = "check"
                };
                duplicateCheck = _duplicateCheckBLL.Get(duplicateCheck);
                if (!duplicateCheck.IsSuccess)
                {
                    result.MessageList.Add("Category code already in use.");
                    result.IsSuccess = false;
                }
            }
            if (result.MessageList.Count() == 0)
            {

                try
                {
                    _EquipmentCategoryDAL = new EquipmentCategoryDAL();
                    result.IsSuccess = _EquipmentCategoryDAL.Update(objEquipmentCategoryEntity);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                _EquipmentCategoryDAL = null;
            }

            return result;

        }
        public IEnumerable<EquipmentCategoryListEntity> GetFiltered(EquipmentCategoryListEntity objEquipmentCategoryListEntityFilter)
        {

            if (objEquipmentCategoryListEntityFilter == null)
                throw new Exception(MessageUTIL.PROVIDE_VALID_FILTER + "Equipment.");

            objEquipmentCategoryListEntityFilter.SortDirection = ("" + objEquipmentCategoryListEntityFilter.SortDirection).Trim().ToLower();

            if (objEquipmentCategoryListEntityFilter.RowStart == 0)
                objEquipmentCategoryListEntityFilter.NoOfRecord = DefaultValuesUTIL.TableDisplayCountPerPage;
            if (!string.IsNullOrEmpty(objEquipmentCategoryListEntityFilter.SortDirection))
                if (objEquipmentCategoryListEntityFilter.SortDirection != "asc" && objEquipmentCategoryListEntityFilter.SortDirection != "desc")
                    objEquipmentCategoryListEntityFilter.SortDirection = "";

            List<EquipmentCategoryListEntity> result = null;

            try
            {
                _EquipmentCategoryDAL = new EquipmentCategoryDAL();
                result = new List<EquipmentCategoryListEntity>();
                result = _EquipmentCategoryDAL.GetFiltered(objEquipmentCategoryListEntityFilter).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return result;
        }
        public EquipmentCategoryEntity Get(EquipmentCategoryEntity objEquipmentCategoryEntity)
        {
            EquipmentCategoryEntity result = new EquipmentCategoryEntity();
            result.MessageList = new List<string>();
            if (result.MessageList.Count == 0)
            {
                try
                {
                    _EquipmentCategoryDAL = new EquipmentCategoryDAL();
                    result = _EquipmentCategoryDAL.Get(objEquipmentCategoryEntity);
                    if (result == null)
                    {
                        result = new EquipmentCategoryEntity();
                        result.IsSuccess = false;
                        result.MessageList = new List<string>();
                        result.MessageList.Add("Category does not exists.");
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


                _EquipmentCategoryDAL = null;

            }

            return result;
        }
        public IEnumerable<EquipmentCategoryEntity> GetList()
        {
            List<EquipmentCategoryEntity> result = null;

            try
            {
                _EquipmentCategoryDAL = new EquipmentCategoryDAL();
                result = new List<EquipmentCategoryEntity>();
                result = _EquipmentCategoryDAL.GetList().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public IEnumerable<EquipmentCategoryEntity> GetListOnlyAvailable()
        {
            List<EquipmentCategoryEntity> result = null;

            try
            {
                _EquipmentCategoryDAL = new EquipmentCategoryDAL();
                result = new List<EquipmentCategoryEntity>();
                result = _EquipmentCategoryDAL.GetListOnlyAvailable().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}

