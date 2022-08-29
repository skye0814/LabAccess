using ERSDAL;
using ERSEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSBLL
{
   public class YearBLL
    {
        YearDAL _YearDAL = null;
        public IEnumerable<YearEntity> GetList()
        {
            List<YearEntity> result = null;

            try
            {
                _YearDAL = new YearDAL();
                result = new List<YearEntity>();
                result = _YearDAL.GetList().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}
