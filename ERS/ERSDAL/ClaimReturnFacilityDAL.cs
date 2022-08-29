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
    public class ClaimReturnFacilityDAL : IBaseDAL<RequestFacilityEntity, int>
    {
        public bool Delete(RequestFacilityEntity e)
        {
            throw new NotImplementedException();
        }

        public bool CancelUnclaimedFacilityRequests()
        {
            bool isSuccess = false;
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spCancelUnclaimedFacilityRequests",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.AddWithValue("@GetDate", DateTime.Now);
                    conn.Open();
                    isSuccess = Convert.ToBoolean(cmd.ExecuteScalar());
                }
                return isSuccess;
            }
        }

        public RequestFacilityEntity Get(RequestFacilityEntity e)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RequestFacilityEntity> GetList()
        {
            throw new NotImplementedException();
        }
       
        public RequestFacilityEntity Insert(RequestFacilityEntity e)
        {
            throw new NotImplementedException();
        }
        public RequestFacilityEntity RequestFacilityClaim(RequestFacilityEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestFacilityClaim",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@requestFacilityGUID", SqlDbType.VarChar);

                    cmd.Parameters["@requestFacilityGUID"].Value = e.RequestFacilityGUID;
                    conn.Open();

                    e.IsSuccess = Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }

            return e;
        }
        public RequestFacilityEntity RequestFacilityReturn(RequestFacilityEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestFacilityReturn",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@requestFacilityGUID", SqlDbType.VarChar);

                    cmd.Parameters["@requestFacilityGUID"].Value = e.RequestFacilityGUID;
                    conn.Open();

                    e.IsSuccess = Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }

            return e;
        }

        public bool Update(RequestFacilityEntity e)
        {
            throw new NotImplementedException();
        }
        public RequestFacilityEntity ResetClaimedRequest(RequestFacilityEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestFacilityReset",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@requestGUID", SqlDbType.VarChar);

                    cmd.Parameters["@requestGUID"].Value = e.RequestFacilityGUID;

                    conn.Open();
                    e.IsSuccess = Convert.ToBoolean(cmd.ExecuteScalar());
                }
                return e;
            }
        }
        public RequestFacilityEntity ConfirmClaimedRequest(RequestFacilityEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestFacilityConfirm",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@requestGUID", SqlDbType.VarChar);
                    cmd.Parameters.Add("@GetDate", SqlDbType.DateTime);

                    cmd.Parameters["@requestGUID"].Value = e.RequestFacilityGUID;
                    cmd.Parameters["@GetDate"].Value = DateTime.Now;

                    conn.Open();
                    e.IsSuccess = Convert.ToBoolean(cmd.ExecuteScalar());
                }
                return e;
            }
        }
    } 
}