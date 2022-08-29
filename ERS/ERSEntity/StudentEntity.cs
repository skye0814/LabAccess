using ERSEntity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSEntity
{
    public class StudentEntity : CommonEntity
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string StudentNumber { get; set; }
        public int CourseID { get; set; }
        public int SectionID { get; set; }
        public int YearID { get; set; }
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int SystemUserID { get; set; }
        // FOR Duplicate Checking
        public string Status { get; set; }
        // For main and archive tables GET
        public bool isArchive { get; set; }
        public string ResetPasswordCode { get; set; }

    }

    public class StudentListEntity: TableDisplayCommonEntity
    {
        public int ID { get; set; }
        public string FirstName { get; set;  }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string StudentNumber { get; set; }
        public string Course { get; set; }
        public string Section { get; set; }
        public string Year { get; set; }
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public int SystemUserID { get; set; }
        // For main and archive tables GET
        public bool isArchive { get; set; }
    }

    public class StudentReportEntity : CommonEntity
    {

    }

    public class objStudentRegistrationExcelEntity : UploadFileEntity
    {
        public List<StudentRegistrationExcelValuesEntity> ExcelValues { get; set; }
    }

    public class StudentRegistrationExcelValuesEntity
    {
        public string Row { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string StudentNumber { get; set; }
        public string Course { get; set; }
        public string Section { get; set; }
        public string Year { get; set; }
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
    }
}
