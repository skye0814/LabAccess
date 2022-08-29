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
    public class SystemUserDAL : IBaseDAL<SystemUserEntity, int>
    {
        public bool Delete(SystemUserEntity e)
        {
            throw new NotImplementedException();
        }

        public SystemUserEntity Get(SystemUserEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spSystemUserGet",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@UserName", SqlDbType.VarChar);
                    cmd.Parameters.Add("@Password", SqlDbType.VarChar);
                    cmd.Parameters["@Password"].Value = e.Password;
                    cmd.Parameters["@UserName"].Value = e.UserName;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            e.ID = Convert.ToInt32(reader["ID"]);
                            e.UserName = "" + reader["UserName"];
                            e.FirstName = "" + reader["FirstName"];
                            e.MiddleName = "" + reader["MiddleName"];
                            e.LastName = "" + reader["LastName"];
                            e.FailedAttempt = Convert.ToInt32(reader["FailedAttempt"]);
                            e.IsPasswordChanged = Convert.ToBoolean(reader["IsPasswordChanged"]);
                            e.IsLoggedIn = Convert.ToBoolean(reader["IsLoggedIn"]);
                            e.IsActive = Convert.ToBoolean(reader["IsActive"]);
                            e.isLabPersonnel = Convert.ToBoolean(reader["isLabPersonnel"]);
                            e.isStudent = Convert.ToBoolean(reader["isStudent"]);
                        }
                        else
                        {
                            e = null;
                        }
                    }
                }
            }

            return e;
        }

        public IEnumerable<SystemUserEntity> GetList()
        {
            throw new NotImplementedException();
        }

        public SystemUserEntity Insert(SystemUserEntity e)
        {
            throw new NotImplementedException();
        }

        public bool Update(SystemUserEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spSystemUserUpdate",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@ID", SqlDbType.Int);
                    cmd.Parameters.Add("@UserName", SqlDbType.VarChar);
                    cmd.Parameters.Add("@Password", SqlDbType.VarChar);
                    cmd.Parameters.Add("@IsLoggedIn", SqlDbType.Bit);
                    cmd.Parameters.Add("@ModifiedBy", SqlDbType.Int);

                    cmd.Parameters["@ID"].Value = e.ID;
                    cmd.Parameters["@Password"].Value = e.Password;
                    cmd.Parameters["@UserName"].Value = e.UserName;
                    cmd.Parameters["@IsLoggedIn"].Value = e.IsLoggedIn;
                    cmd.Parameters["@ModifiedBy"].Value = e.ModifiedBy;
                    conn.Open();

                    e.IsSuccess = Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }

            return e.IsSuccess;

        }

        public bool UpdatePasswordByResetPasswordCode(SystemUserEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spSystemUserUpdatePasswordByResetPasswordCode",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@ResetPasswordCode", SqlDbType.VarChar);
                    cmd.Parameters.Add("@Password", SqlDbType.VarChar);

                    cmd.Parameters["@ResetPasswordCode"].Value = e.ResetPasswordCode;
                    cmd.Parameters["@Password"].Value = e.Password;
                    conn.Open();

                    e.IsSuccess = Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }

            return e.IsSuccess;

        }
    }
}
