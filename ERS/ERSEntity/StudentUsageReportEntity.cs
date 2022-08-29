using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERSEntity.Entity;

namespace ERSEntity
{
    public class StudentUsageReportEntity : TableDisplayCommonEntity
    {
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string YearLevel { get; set; }
        public int NoOfTimesBorrowed { get; set; }
    }
}
