using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERSEntity.Entity;

namespace ERSEntity
{
    public class EquipmentUsageReportEntity : TableDisplayCommonEntity
    {
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string Category { get; set; }
        public string CategoryCode { get; set; }
        public string EquipmentItemCode { get; set; }
        public string ItemBrand { get; set; }
        public string ItemModel { get; set; }
        public string ItemSerialNumber { get; set; }
        public string Status { get; set; }
        public int NoOfTimesBorrowed { get; set; }
    }
}
