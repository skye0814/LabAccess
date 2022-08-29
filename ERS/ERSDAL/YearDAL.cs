using ERSDAL.DB;
using ERSDAL.Service;
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
    public class YearDAL : IBaseDAL<YearEntity, int>
    {
        public bool Delete(YearEntity e)
        {
            throw new NotImplementedException();
        }

        public YearEntity Get(YearEntity e)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<YearEntity> GetList()
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spYearGet",
                    CommandType = CommandType.StoredProcedure
                })
                {

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            yield return new YearEntity()
                            {
                                YearID = Convert.ToInt32(reader["YearID"]),
                                YearCode = reader["YearCode"].ToString(),
                                YearDescription = reader["YearDescription"].ToString()
                            };
                        }
                    }
                }
            }
        }

        public YearEntity Insert(YearEntity e)
        {
            throw new NotImplementedException();
        }

        public bool Update(YearEntity e)
        {
            throw new NotImplementedException();
        }
    }
}
