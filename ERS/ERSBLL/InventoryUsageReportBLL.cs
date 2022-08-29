using ERSDAL;
using ERSEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSBLL
{
    public class InventoryUsageReportBLL
    {
        InventoryUsageReportDAL _inventoryUsageDAL = null;
        public IEnumerable<EquipmentUsageReportEntity> GetFiltered(EquipmentUsageReportEntity objEquipmentUsage)
        {
            List<EquipmentUsageReportEntity> result = null;

            try
            {
                _inventoryUsageDAL = new InventoryUsageReportDAL();
                result = new List<EquipmentUsageReportEntity>();
                result = _inventoryUsageDAL.GetFiltered(objEquipmentUsage).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return result;
        }
    }
}
