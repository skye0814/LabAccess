using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERSEntity.Entity;

namespace ERSEntity
{
    public class FacilityUsageReportEntity : TableDisplayCommonEntity
    {
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string RoomType { get; set; }
        public string RoomNumber { get; set; }
        public string RoomDescription { get; set; }
        public int NoOfTimesBorrowed { get; set; }
    }
}
