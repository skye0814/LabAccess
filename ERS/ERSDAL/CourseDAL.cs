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
    public class CourseDAL : IBaseDAL<CourseEntity, int>
    {
        public bool Delete(CourseEntity e)
        {
            throw new NotImplementedException();
        }

        public CourseEntity Get(CourseEntity e)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CourseEntity> GetList()
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spCourseGet",
                    CommandType = CommandType.StoredProcedure
                })
                {

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            yield return new CourseEntity()
                            {
                                CourseID = Convert.ToInt32(reader["CourseID"]),
                                CourseCode = reader["CourseCode"].ToString(),
                                CourseDescription = reader["CourseDescription"].ToString()
                            };
                        }
                    }
                }
            }
        }

        public CourseEntity Insert(CourseEntity e)
        {
            throw new NotImplementedException();
        }

        public bool Update(CourseEntity e)
        {
            throw new NotImplementedException();
        }
    }
}
