using ERSEntity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSEntity
{
    public class EquipmentEntity : CommonEntity 
    {
        public int EquipmentID { get; set; }
        public string EquipmentCode { get; set; }
        public string Description { get; set; }
        public int QuantityTotal { get; set; }
        public int QuantityActive { get; set; }
        public bool isActive { get; set; }
        public int ModifiedBy { get; set; }
        public string Comments { get; set; }
    }
    public class EquipmentListEntity : TableDisplayCommonEntity
    {
        public int EquipmentID { get; set; }
        public string EquipmentCode { get; set; }
        public string Description { get; set; }
        public int QuantityTotal { get; set; }
        public int QuantityActive { get; set; }
        public bool isActive { get; set; }
        public int ModifiedBy { get; set; }
        public string Comments { get; set; }

    }
    public class EquipmentReportEntity : CommonEntity
    {

    }

}
