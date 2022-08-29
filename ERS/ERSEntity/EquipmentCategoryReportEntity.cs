using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERSEntity.Entity;

namespace ERSEntity
{
    public class EquipmentCategoryReportEntity : TableDisplayCommonEntity
    {
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string Category { get; set; }
        public string CategoryCode { get; set; }
        public int Quantity { get; set; }
        public int UsableQuantity { get; set; }
        public int DefectiveQuantity { get; set; }
        public int MissingQuantity { get; set; }
        public int TotalNumberBorrowed { get; set; }
    }
}
