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
    public class ClaimReturnFacilityBLL
    {
        ClaimReturnFacilityDAL _ClaimReturnFacilityDAL = null;
        public RequestFacilityEntity Insert(RequestFacilityEntity objRequestFacilityEntity)
        {
            throw new NotImplementedException();
        }
        public RequestFacilityEntity Update(RequestFacilityEntity objRequestFacilityEntity)
        {
            throw new NotImplementedException();
        }
        public RequestFacilityEntity RequestFacilityUpdate(RequestFacilityEntity objRequestFacilityEntity)
        {
            RequestFacilityEntity result = new RequestFacilityEntity();
            string errorMessage = string.Empty;
            result.MessageList = new List<string>();
            if (objRequestFacilityEntity == null)
                objRequestFacilityEntity = new RequestFacilityEntity();

            if (result.MessageList.Count() == 0)
            {
                try
                {
                    _ClaimReturnFacilityDAL = new ClaimReturnFacilityDAL();
                    if (objRequestFacilityEntity.Status == "Unclaimed")
                    {
                        result = _ClaimReturnFacilityDAL.RequestFacilityClaim(objRequestFacilityEntity);
                        if (!result.IsSuccess)
                        {
                            result.MessageList = new List<string>();
                            result.MessageList.Add("Something went wrong. Contact your admin.");
                        }
                    }
                    else if (objRequestFacilityEntity.Status == "Claimed")
                    {
                        result = _ClaimReturnFacilityDAL.RequestFacilityReturn(objRequestFacilityEntity);
                        if (!result.IsSuccess)
                        {
                            result.MessageList = new List<string>();
                            result.MessageList.Add("Something went wrong. Contact your admin.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                _ClaimReturnFacilityDAL = null;
            }
            return result;
        }
        public RequestFacilityEntity UpdateClaimedRequest(RequestFacilityEntity objRequestFacilityEntity)
        {
            RequestFacilityEntity result = new RequestFacilityEntity();
            result.MessageList = new List<string>();
            if (objRequestFacilityEntity == null)
                objRequestFacilityEntity = new RequestFacilityEntity();
            _ClaimReturnFacilityDAL = new ClaimReturnFacilityDAL();
            if (result.MessageList.Count() == 0)
            {
                try
                {
                    _ClaimReturnFacilityDAL = new ClaimReturnFacilityDAL();
                    if (objRequestFacilityEntity.Mode == "reset")
                    {
                        result = _ClaimReturnFacilityDAL.ResetClaimedRequest(objRequestFacilityEntity);
                        if (!result.IsSuccess)
                        {
                            result.IsSuccess = false;
                            result.MessageList = new List<string>();
                            result.MessageList.Add("Something went wrong.");
                        }
                    }
                    else if (objRequestFacilityEntity.Mode == "confirm")
                    {
                        result = _ClaimReturnFacilityDAL.ConfirmClaimedRequest(objRequestFacilityEntity);

                        if (!result.IsSuccess)
                        {
                            result.IsSuccess = false;
                            result.MessageList = new List<string>();
                            result.MessageList.Add("Something went wrong.");
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
                _ClaimReturnFacilityDAL = null;
            }
            return result;
        }
    }
}

