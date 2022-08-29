using ERSDAL;
using ERSEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSBLL
{
    public class FacilityUsageReportBLL
    {
        FacilityUsageReportDAL _FacilityUsageDAL = null;
        public IEnumerable<FacilityUsageReportEntity> GetFiltered(FacilityUsageReportEntity objFacilityUsage)
        {
            List<FacilityUsageReportEntity> result = null;

            try
            {
                _FacilityUsageDAL = new FacilityUsageReportDAL();
                result = new List<FacilityUsageReportEntity>();
                result = _FacilityUsageDAL.GetFiltered(objFacilityUsage).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return result;
        }
    }
}
