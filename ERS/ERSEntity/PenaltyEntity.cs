using ERSEntity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSEntity
{
    public class PenaltyEntity : CommonEntity
    {
        public int PenaltyID { get; set; }
        public int RequestsID { get; set; }
        public int FacilityRequestID { get; set; }
        public string RequestType { get; set; }
        public int RequestorID { get; set; }
        public int ModifiedBy { get; set; }
        public bool isActive { get; set; }
        public string RequestGUID { get; set; }
        public string PenaltyDetails { get; set; }
        public string Requestor { get; set; }

    }
    public class PenaltyListEntity : TableDisplayCommonEntity
    {
        public int PenaltyID { get; set; }
        public int RequestsID { get; set; }
        public int FacilityRequestID { get; set; }
        public string RequestType { get; set; }
        public int RequestorID { get; set; }
        public int ModifiedBy { get; set; }
        public bool isActive { get; set; }
        public string RequestGUID { get; set; }
        public string Requestor { get; set; }
    }
}
