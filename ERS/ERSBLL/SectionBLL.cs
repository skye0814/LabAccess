using ERSDAL;
using ERSEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSBLL
{
    public class SectionBLL
    {
        SectionDAL _SectionDAL = null;
        public IEnumerable<SectionEntity> GetList()
        {
            List<SectionEntity> result = null;

            try
            {
                _SectionDAL = new SectionDAL();
                result = new List<SectionEntity>();
                result = _SectionDAL.GetList().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}
