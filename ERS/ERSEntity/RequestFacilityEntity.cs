using ERSEntity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSEntity
{
   public class RequestFacilityEntity : CommonEntity
    {
        //string GenerateGUID = Guid.NewGuid().ToString();
        public int FacilityRequestID { get; set; } // PK
        public string FacilityRequestor { get; set; }
        public int FacilityRequestorID { get; set; }
        public int? FacilityID { get; set; } //FK
        public string RequestDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Status { get; set; } //Cancelled, Unclaimed,Claimed,Completed
        public string ClaimedTime { get; set; }
        public string ReturnedTime { get; set; }   
        public string Remarks { get; set; }
        public string RequestFacilityGUID { get; set; }
        public string RoomNumber { get; set; }
        public string Schedule { get; set; }

        // Vacant Time
        public string VacantStart { get; set; }
        public string VacantEnd { get; set; }
    }
    
    public class RequestFacilityListEntity : TableDisplayCommonEntity
    {
        public int FacilityRequestID { get; set; } 
        public string FacilityRequestor { get; set; }
        public int FacilityRequestorID { get; set; }
        public string Facility { get; set; }
        public int FacilityID { get; set; }
        public string RequestDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Status { get; set; }
        public string ClaimedTime { get; set; }
        public string ReturnedTime { get; set; }
        public string Remarks { get; set; }
        public string RequestFacilityGUID { get; set; }
        public string StatusMode { get; set; }

        //FOR PENALTY
        public int PenaltyID { get; set; }

    }
}
