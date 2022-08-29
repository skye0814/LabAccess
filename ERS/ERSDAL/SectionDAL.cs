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
    public class SectionDAL : IBaseDAL<SectionEntity, int>
    {
        public bool Delete(SectionEntity e)
        {
            throw new NotImplementedException();
        }

        public SectionEntity Get(SectionEntity e)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SectionEntity> GetList()
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spSectionGet",
                    CommandType = CommandType.StoredProcedure
                })
                {

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            yield return new SectionEntity()
                            {
                                SectionID = Convert.ToInt32(reader["SectionID"]),
                                SectionCode = reader["SectionCode"].ToString(),
                                SectionDescription = reader["SectionDescription"].ToString()
                            };
                        }
                    }
                }
            }
        }

        public SectionEntity Insert(SectionEntity e)
        {
            throw new NotImplementedException();
        }

        public bool Update(SectionEntity e)
        {
            throw new NotImplementedException();
        }
    }
}
