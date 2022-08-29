using ERSDAL;
using ERSEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSBLL
{
    public class EquipmentCategoryReportBLL
    {
        EquipmentCategoryReportDAL _equipmentCategoryReportDAL = null;
        public IEnumerable<EquipmentCategoryReportEntity> GetFiltered(EquipmentCategoryReportEntity objEquipmentCategory)
        {
            List<EquipmentCategoryReportEntity> result = null;

            try
            {
                _equipmentCategoryReportDAL = new EquipmentCategoryReportDAL();
                result = new List<EquipmentCategoryReportEntity>();
                result = _equipmentCategoryReportDAL.GetFiltered(objEquipmentCategory).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return result;
        }
    }
}
