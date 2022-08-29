using ERSEntity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSEntity
{
    public class RequestsEntity : CommonEntity 
    {
        public int RequestID { get; set; }
        public string Requestor { get; set; }
        public int RequestorID { get; set; }
        public string RequestDateTime { get; set; }
        public string StartTime { get; set; } 
        public string EndTime { get; set; } 
        public int isApproved { get; set; }
        public string RequestGUID { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; } // Completed, Cancelled, Claimed, Unclaimed
        public List<RequestDetailsEntity> RequestDetails { get; set; }
        public RequestDetailsEntity RequestDetailsEach { get; set; }
    }

    public class RequestDetailsEntity : CommonEntity
    {
        public int RequestDetailsID { get; set; }
        public int? EquipmentCategoryID { get; set; }
        public int Quantity { get; set; }
        public string RequestGUID { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Status { get; set; }
        public int RequestorID { get; set; }
        public List<RequestDetailsEntity> RequestDetailsList { get; set; }
        public int RequestEquipmentItemID { get; set; }
        public string Category { get; set; }
    }

    public class RequestEquipmentSumEntity
    {
        public int EquipmentCategoryID { get; set; }
        public int TotalSumOfBorrowed { get; set; }
    }

    public class RequestsListEntity : TableDisplayCommonEntity
    {
        public int RequestID { get; set; }
        public string Requestor { get; set; }
        public int RequestorID { get; set; }
        public string RequestDateTime { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int isApproved { get; set; }
        public string RequestGUID { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public string ClaimedTime { get; set; }
        public string ReturnedTime { get; set; }
        public string StatusMode { get; set; }

        //FOR PENALTY
        public int PenaltyID { get; set; }

    }
    public class RequestsReportEntity : CommonEntity
    {

    }



}
