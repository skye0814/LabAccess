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
    public class EquipmentCategoryReportDAL
    {
        public IEnumerable<EquipmentCategoryReportEntity> GetFiltered(EquipmentCategoryReportEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spEquipmentCategoryReport",
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
                            yield return new EquipmentCategoryReportEntity()
                            {
                                DateFrom = reader["DateFrom"].ToString(),
                                DateTo = reader["DateTo"].ToString(),
                                Category = reader["Category"].ToString(),
                                CategoryCode = reader["CategoryCode"].ToString(),
                                Quantity = Convert.ToInt32(reader["QuantityTotal"]),
                                UsableQuantity = Convert.ToInt32(reader["QuantityUsable"]),
                                DefectiveQuantity = Convert.ToInt32(reader["QuantityDefective"]),
                                MissingQuantity = Convert.ToInt32(reader["QuantityMissing"]),
                                TotalNumberBorrowed = Convert.ToInt32(reader["NoOfTimesBorrowed"])
                            };
                        }
                    }
                }
            }
        }
    }
}
