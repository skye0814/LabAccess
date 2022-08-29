using ERSDAL;
using ERSEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSBLL
{
    public class StudentUsageReportBLL
    {
        StudentUsageReportDAL _StudentUsageDAL = null;
        public IEnumerable<StudentUsageReportEntity> GetFiltered(StudentUsageReportEntity objStudentUsage)
        {
            List<StudentUsageReportEntity> result = null;

            try
            {
                _StudentUsageDAL = new StudentUsageReportDAL();
                result = new List<StudentUsageReportEntity>();
                result = _StudentUsageDAL.GetFiltered(objStudentUsage).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return result;
        }
    }
}
