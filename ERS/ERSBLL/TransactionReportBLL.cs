using ERSDAL;
using ERSEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSBLL
{
    public class TransactionReportBLL
    {
        TransactionReportDAL _TransactionReportDAL = null;
        public IEnumerable<TransactionReportEntity> GetFiltered(TransactionReportEntity objTransaction)
        {
            List<TransactionReportEntity> result = null;

            try
            {
                _TransactionReportDAL = new TransactionReportDAL();
                result = new List<TransactionReportEntity>();
                result = _TransactionReportDAL.GetFiltered(objTransaction).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return result;
        }
    }
}
