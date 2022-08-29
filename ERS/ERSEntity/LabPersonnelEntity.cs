using ERSEntity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSEntity
{
    public class LabPersonnelEntity : CommonEntity
    {
        public int ID { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
        public int SystemUserID { get; set; }
        public bool isArchive { get; set; }

        // FOR Duplicate Checking
        public string Status { get; set; }
    }

    public class LabPersonnelListEntity : TableDisplayCommonEntity
    {
        public int ID { get; set; }
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string UserName { get; set; }
        public int SystemUserID { get; set; }
        public bool isArchive { get; set; }
    }

    public class LabPersonnelReportEntity : CommonEntity
    {

    }
}
