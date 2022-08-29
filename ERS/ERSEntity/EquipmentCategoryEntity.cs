using ERSEntity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSEntity
{
    public class EquipmentCategoryEntity : CommonEntity
    {
        public int EquipmentCategoryID { get; set; }
        public bool isActive { get; set; }
        public string Category { get; set; }
        public string CategoryCode { get; set; }
        public int QuantityTotal { get; set; }
        public int QuantityUsable { get; set; }
        public int QuantityDefective { get; set; }
        public int QuantityMissing { get; set; }
        public int NoOfTimesBorrowed { get; set; }
        public int ModifiedBy { get; set; }
        public string Comments { get; set; }

        // FOR Duplicate Checking
        public string Status { get; set; }
    }
    public class EquipmentCategoryListEntity : TableDisplayCommonEntity
    {
        public int EquipmentCategoryID { get; set; }
        public bool isActive { get; set; }
        public string Category { get; set; }
        public string CategoryCode { get; set; }
        public int QuantityTotal { get; set; }
        public int QuantityUsable { get; set; }
        public int QuantityDefective { get; set; }
        public int QuantityMissing { get; set; }
        public int NoOfTimesBorrowed { get; set; }
        public int ModifiedBy { get; set; }
        public string Comments { get; set; }

    }

    public class EquipmentCategoryQuantityAvailable
    {
        public List<List<int>> QuantityForEach { get; set; }
    }

}
