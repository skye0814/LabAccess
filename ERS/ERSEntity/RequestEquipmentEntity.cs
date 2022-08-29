using ERSEntity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSEntity
{
    public class RequestEquipmentEntity : CommonEntity 
    {
        public int RequestID { get; set; }
        public string Requestor { get; set; }
        public int Quantity { get; set; }
        public int? EquipmentCategoryID { get; set; } // ito yung ID ng item na nirerequest -> naka-link sa EquipmentCategory table
        public string Date { get; set; } // Date Borrowed
        public string Time { get; set; } // Date Return
        public bool isApproved { get; set; }
        public int ModifiedBy { get; set; }
        public string Remarks { get; set; }
    }
    public class RequestEquipmentListEntity : TableDisplayCommonEntity
    {
        public int RequestID { get; set; }
        public string Requestor { get; set; }
        public int Quantity { get; set; }
        public string EquipmentCategory { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public bool isApproved { get; set; }
        public string Remarks { get; set; }

    }
    public class RequestEquipmentReportEntity : CommonEntity
    {

    }

    public class RequestEquipmentItemEntity : CommonEntity
    {
        public int RequestID { get; set; }
        public int EquipmentID { get; set; }
        public int EquipmentItemId { get; set; }

    }
}
