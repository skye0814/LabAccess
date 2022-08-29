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
    public class ClaimReturnEquipmentBLL
    {
        ClaimReturnEquipmentDAL _ClaimReturnEquipmentDAL = null;
        public ClaimReturnEquipmentEntity Insert(ClaimReturnEquipmentEntity objClaimReturnEquipmentEntity)
        {
            throw new NotImplementedException();
        }
        public ClaimReturnEquipmentEntity Update(ClaimReturnEquipmentEntity objClaimReturnEquipmentEntity)
        {
            throw new NotImplementedException();
        }
        public ClaimReturnEquipmentEntity UpdateDatabase(RequestDetailsEntity objClaimReturnEquipmentEntity) //Insert RequestEquipmentItemIDs based on RequestDetails
        {
            RequestDetailsEntity update = new RequestDetailsEntity();
            ClaimReturnEquipmentEntity result = new ClaimReturnEquipmentEntity();
            update.MessageList = new List<string>();
            try
            {
                _ClaimReturnEquipmentDAL = new ClaimReturnEquipmentDAL();
                update = _ClaimReturnEquipmentDAL.UpdateDatabase(objClaimReturnEquipmentEntity);
                if (update == null)
                {
                    result = new ClaimReturnEquipmentEntity();
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
            _ClaimReturnEquipmentDAL = null;
            return result;
        }
        public IEnumerable<ClaimReturnEquipmentListEntity> GetFiltered(ClaimReturnEquipmentListEntity objRequestEquipmentItemListEntityFilter)
        {

            if (objRequestEquipmentItemListEntityFilter == null)
                throw new Exception(MessageUTIL.PROVIDE_VALID_FILTER + "Equipment.");

            objRequestEquipmentItemListEntityFilter.SortDirection = ("" + objRequestEquipmentItemListEntityFilter.SortDirection).Trim().ToLower();

            if (objRequestEquipmentItemListEntityFilter.RowStart == 0)
                objRequestEquipmentItemListEntityFilter.NoOfRecord = DefaultValuesUTIL.TableDisplayCountPerPage;
            if (!string.IsNullOrEmpty(objRequestEquipmentItemListEntityFilter.SortDirection))
                if (objRequestEquipmentItemListEntityFilter.SortDirection != "asc" && objRequestEquipmentItemListEntityFilter.SortDirection != "desc")
                    objRequestEquipmentItemListEntityFilter.SortDirection = "";

            List<ClaimReturnEquipmentListEntity> result = null;

            try
            {
                _ClaimReturnEquipmentDAL = new ClaimReturnEquipmentDAL();
                result = new List<ClaimReturnEquipmentListEntity>();
                result = _ClaimReturnEquipmentDAL.GetFiltered(objRequestEquipmentItemListEntityFilter).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public ClaimReturnEquipmentEntity Get(ClaimReturnEquipmentEntity objClaimReturnEquipmentEntity)
        {
            ClaimReturnEquipmentEntity result = new ClaimReturnEquipmentEntity();
            result.MessageList = new List<string>();
            if (result.MessageList.Count == 0)
            {
                try
                {
                    _ClaimReturnEquipmentDAL = new ClaimReturnEquipmentDAL();
                    result = _ClaimReturnEquipmentDAL.Get(objClaimReturnEquipmentEntity);
                    if (result == null)
                    {
                        result = new ClaimReturnEquipmentEntity();
                        result.IsSuccess = false;
                        result.MessageList = new List<string>();
                        result.MessageList.Add("RequestGUID does not exist.");
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
                _ClaimReturnEquipmentDAL = null;
            }
            return result;
        }
        public ClaimReturnEquipmentEntity CheckRequestGUID(ClaimReturnEquipmentEntity objClaimReturnEquipmentEntity)
        {
            ClaimReturnEquipmentEntity result = new ClaimReturnEquipmentEntity();
            result.MessageList = new List<string>();
            if (result.MessageList.Count == 0)
            {
                try
                {
                    _ClaimReturnEquipmentDAL = new ClaimReturnEquipmentDAL();
                    result = _ClaimReturnEquipmentDAL.CheckRequestGUID(objClaimReturnEquipmentEntity);
                    if (result == null)
                    {
                        result = new ClaimReturnEquipmentEntity();
                        result.IsSuccess = false;
                        result.MessageList = new List<string>();
                        result.MessageList.Add("RequestGUID does not exist.");
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
                _ClaimReturnEquipmentDAL = null;
            }
            return result;
        }
        public ClaimReturnEquipmentEntity GetItem(ClaimReturnEquipmentEntity objClaimReturnEquipmentEntity)
        {
            ClaimReturnEquipmentEntity result = new ClaimReturnEquipmentEntity();
            result.MessageList = new List<string>();
            if (result.MessageList.Count == 0)
            {
                try
                {
                    _ClaimReturnEquipmentDAL = new ClaimReturnEquipmentDAL();
                    result = _ClaimReturnEquipmentDAL.GetItem(objClaimReturnEquipmentEntity);
                    if (result == null)
                    {
                        result = new ClaimReturnEquipmentEntity();
                        result.IsSuccess = false;
                        result.MessageList = new List<string>();
                        result.MessageList.Add("RequestGUID does not exist.");
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
                _ClaimReturnEquipmentDAL = null;
            }
            return result;
        }
        public ClaimReturnEquipmentEntity RequestEquipmentItemUpdate (ClaimReturnEquipmentEntity objClaimReturnEquipmentEntity)
        {
            ClaimReturnEquipmentEntity result = new ClaimReturnEquipmentEntity();
            string errorMessage = string.Empty;
            result.MessageList = new List<string>();
            if (objClaimReturnEquipmentEntity == null)
                objClaimReturnEquipmentEntity = new ClaimReturnEquipmentEntity();

            if (result.MessageList.Count() == 0)
            {
                try
                {
                    _ClaimReturnEquipmentDAL = new ClaimReturnEquipmentDAL();
                    if (objClaimReturnEquipmentEntity.Status == "Unclaimed")
                    {
                        result = _ClaimReturnEquipmentDAL.RequestEquipmentItemClaim(objClaimReturnEquipmentEntity);
                        if (result.intReturnValue == 1)
                        {
                            result.IsSuccess = true;
                        }
                        else if (result.intReturnValue == 2)
                        {
                            result.IsSuccess = false;
                            result.MessageList = new List<string>();
                            result.MessageList.Add("Incorrect Equipment Category.");
                        }
                        else if (result.intReturnValue == 3)
                        {
                            result.IsSuccess = false;
                            result.MessageList = new List<string>();
                            result.MessageList.Add("Request is already marked as claimed. Cannot add more items. Proceed to Equipment Return.");

                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.MessageList = new List<string>();
                            result.MessageList.Add("Item Update Error. Contact Admin");
                        }
                    }
                    else if (objClaimReturnEquipmentEntity.Status == "Claimed")
                    {
                        result = _ClaimReturnEquipmentDAL.RequestEquipmentItemReturn(objClaimReturnEquipmentEntity);
                        if (result.intReturnValue == 1)
                        {
                            result.IsSuccess = true;
                        }
                        else if (result.intReturnValue == 2)
                        {
                            result.IsSuccess = false;
                            result.MessageList = new List<string>();
                            result.MessageList.Add("Incorrect Equipment Item.");
                        }
                        else if (result.intReturnValue == 3)
                        {
                            result.IsSuccess = true;
                        }
                        else if (result.intReturnValue == 4)
                        {
                            result.IsSuccess = false;
                            result.MessageList = new List<string>();
                            result.MessageList.Add("Incorrect Equipment Category.");
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.MessageList = new List<string>();
                            result.MessageList.Add("Item Update Error. Contact Admin");
                        }
                    }    
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                _ClaimReturnEquipmentDAL = null;
            }
            return result;
        }
        public ClaimReturnEquipmentEntity UpdateClaimedItems(ClaimReturnEquipmentEntity objClaimReturnEquipmentEntity)
        {
            ClaimReturnEquipmentEntity result = new ClaimReturnEquipmentEntity();
            result.MessageList = new List<string>();
            if (objClaimReturnEquipmentEntity == null)
                objClaimReturnEquipmentEntity = new ClaimReturnEquipmentEntity();
            _ClaimReturnEquipmentDAL = new ClaimReturnEquipmentDAL();
            if (result.MessageList.Count() == 0)
            {
                try
                {
                    _ClaimReturnEquipmentDAL = new ClaimReturnEquipmentDAL();
                    if (objClaimReturnEquipmentEntity.mode == "reset")
                    {
                        result = _ClaimReturnEquipmentDAL.ResetClaimedItems(objClaimReturnEquipmentEntity);
                        if (!result.IsSuccess)
                        {
                            result.IsSuccess = false;
                            result.MessageList = new List<string>();
                            result.MessageList.Add("Something went wrong.");
                        }
                    }
                    else if (objClaimReturnEquipmentEntity.mode == "confirm")
                    {
                        result = _ClaimReturnEquipmentDAL.ConfirmClaimedItems(objClaimReturnEquipmentEntity);

                        if (!result.IsSuccess)
                        {
                            result.IsSuccess = false;
                            result.MessageList = new List<string>();
                            result.MessageList.Add("Something went wrong.");
                        }
                        else if (result.IsSuccess)
                        {
                            result = _ClaimReturnEquipmentDAL.Get(objClaimReturnEquipmentEntity);
                            if (result.Status == "Completed")
                            {
                                ClaimReturnEquipmentListEntity updateResult = new ClaimReturnEquipmentListEntity();
                                List<ClaimReturnEquipmentListEntity> itemResult = null;
                                ClaimReturnEquipmentListEntity objRequestEquipmentItemListTableFilter = new ClaimReturnEquipmentListEntity();
                                objRequestEquipmentItemListTableFilter.RowStart = 1;
                                objRequestEquipmentItemListTableFilter.NoOfRecord = 10;
                                objRequestEquipmentItemListTableFilter.SortColumn = 0;
                                objRequestEquipmentItemListTableFilter.SortDirection = "";
                                objRequestEquipmentItemListTableFilter.RequestGUID = objClaimReturnEquipmentEntity.RequestGUID;
                                try
                                {
                                    _ClaimReturnEquipmentDAL = new ClaimReturnEquipmentDAL();
                                    itemResult = new List<ClaimReturnEquipmentListEntity>();
                                    itemResult = _ClaimReturnEquipmentDAL.GetFiltered(objRequestEquipmentItemListTableFilter).ToList();
                                    foreach (var item in itemResult)
                                    {
                                        if (item.Status == "Claimed" || item.Status == "Returned")
                                            updateResult = _ClaimReturnEquipmentDAL.UpdateEquipmentItem(item);
                                    }
                                    // check for unreturned items
                                    result.intReturnValue = _ClaimReturnEquipmentDAL.CheckUnreturnedItems(objClaimReturnEquipmentEntity.RequestGUID);
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                                itemResult = null;
                                updateResult = null;
                            }
                            result.IsSuccess = true;

                        }
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.MessageList = new List<string>();
                        result.MessageList.Add("Incorrect mode.");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                _ClaimReturnEquipmentDAL = null;
            }
            return result;
        }
        public ClaimReturnEquipmentEntity CheckClaimedItems(ClaimReturnEquipmentEntity objClaimReturnEquipmentEntity)
        {
            ClaimReturnEquipmentEntity result = new ClaimReturnEquipmentEntity();
            result.MessageList = new List<string>();
            if (result.MessageList.Count == 0)
            {
                try
                {
                    _ClaimReturnEquipmentDAL = new ClaimReturnEquipmentDAL();
                    result = _ClaimReturnEquipmentDAL.CheckClaimedItems(objClaimReturnEquipmentEntity);
                    if (!result.IsSuccess)
                    {
                        result = new ClaimReturnEquipmentEntity();
                        result.IsSuccess = false;
                        result.MessageList = new List<string>();
                        result.MessageList.Add("Some requested items are not scanned.");
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
                _ClaimReturnEquipmentDAL = null;
            }
            return result;
        }
    }
}

