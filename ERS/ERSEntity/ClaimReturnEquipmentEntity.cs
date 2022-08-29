using ERSEntity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSEntity
{
    public class ClaimReturnEquipmentEntity : CommonEntity
    {
        public int RequestEquipmentItemID { get; set; }
        public string RequestGUID { get; set; }
        public int EquipmentItemID { get; set; }
        public string EquipmentItemCode { get; set; }
        public string Category { get; set; }
        public bool isClaimed { get; set; }
        public string Status { get; set; }
        public string qrCodeValue { get; set; }
        public string mode { get; set; }
        public int intReturnValue { get; set; }

        //Request Details
        public string Requestor { get; set; }
        public string RequestDateTime { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
    public class ClaimReturnEquipmentListEntity : TableDisplayCommonEntity
    {
        public int RequestEquipmentItemID { get; set; }
        public string RequestGUID { get; set; }
        public int EquipmentItemID { get; set; }
        public string EquipmentItemCode { get; set; }
        public string Category { get; set; }
        public bool isClaimed { get; set; }
        public string Status { get; set; }

    }
    public class ClaimReturnEquipmentReportEntity : CommonEntity
    {

    }
}
