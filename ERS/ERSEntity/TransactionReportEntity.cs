using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERSEntity.Entity;

namespace ERSEntity
{
    public class TransactionReportEntity : TableDisplayCommonEntity
    {
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string ClaimedTime { get; set; }
        public string ReturnedTime { get; set; }
        public string Remarks { get; set; }
        public string BorrowedDescription { get; set; }
        public string BorrowedItemOrRoom { get; set; }
        public string StudentNumber { get; set; }
        public string YearSection { get; set; }
        public string Requestor { get; set; }
    }
}
