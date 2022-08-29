using ERSDAL.DB;
using ERSEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSDAL
{
    public class TransactionReportDAL
    {
        public IEnumerable<TransactionReportEntity> GetFiltered(TransactionReportEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spTransactionReport",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@DateFrom", SqlDbType.VarChar);
                    cmd.Parameters.Add("@DateTo", SqlDbType.VarChar);
                    
                    cmd.Parameters["@DateFrom"].Value = e.DateFrom;
                    cmd.Parameters["@DateTo"].Value = e.DateTo;


                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new TransactionReportEntity()
                            {
                                DateFrom = reader["DateFrom"].ToString(),
                                DateTo = reader["DateTo"].ToString(),
                                ClaimedTime = reader["ClaimedTime"].ToString(),
                                ReturnedTime = reader["ReturnedTime"].ToString(),
                                Remarks = reader["Remarks"].ToString(),
                                BorrowedDescription = reader["BorrowedDescription"].ToString(),
                                BorrowedItemOrRoom = reader["BorrowedItemOrRoom"].ToString(),
                                StudentNumber = reader["StudentNumber"].ToString(),
                                YearSection = reader["YearSection"].ToString(),
                                Requestor = reader["Requestor"].ToString()
                            };
                        }
                    }
                }
            }
        }
    }
}
