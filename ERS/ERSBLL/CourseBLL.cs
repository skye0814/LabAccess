using ERSDAL;
using ERSEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSBLL
{
    public class CourseBLL
    {
        CourseDAL _courseDAL = null;
        public IEnumerable<CourseEntity> GetList()
        {
            List<CourseEntity> result = null;

            try
            {
                _courseDAL = new CourseDAL();
                result = new List<CourseEntity>();
                result = _courseDAL.GetList().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}
