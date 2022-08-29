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
    public class StudentUsageReportDAL
    {
        public IEnumerable<StudentUsageReportEntity> GetFiltered(StudentUsageReportEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spStudentUsageReport",
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
                            yield return new StudentUsageReportEntity()
                            {
                                DateFrom = reader["DateFrom"].ToString(),
                                DateTo = reader["DateTo"].ToString(),
                                YearLevel = reader["YearLevel"].ToString(),
                                NoOfTimesBorrowed = Convert.ToInt32(reader["NoOfTimesBorrowed"])
                            };
                        }
                    }
                }
            }
        }
    }
}
