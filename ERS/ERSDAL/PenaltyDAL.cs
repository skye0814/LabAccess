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
    public class PenaltyDAL : IBaseDAL<PenaltyEntity, int>
    {
        public bool Delete(PenaltyEntity e)
        {
            throw new NotImplementedException();
        }

        public PenaltyEntity Get(PenaltyEntity e)
        {
            PenaltyEntity result = null;
            result = new PenaltyEntity();

            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spPenaltyGet",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@PenaltyID", SqlDbType.BigInt);
                    cmd.Parameters.Add("@RequestGUID", SqlDbType.VarChar);


                    cmd.Parameters["@PenaltyID"].Value = e.PenaltyID;
                    cmd.Parameters["@RequestGUID"].Value = e.RequestGUID;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result.Requestor = reader["Requestor"].ToString();
                            result.RequestType = reader["RequestType"].ToString();
                            result.RequestGUID = reader["RequestGUID"].ToString();
                            result.PenaltyDetails = reader["PenaltyDetails"].ToString();
                            result.isActive = Convert.ToBoolean(reader["isActive"].ToString());
                            result.IsSuccess = true;
                        }
                    }
                }
            }
            return result;
        }

        public List<PenaltyEntity> GetAllActiveRequestPenaltyBySystemUserID(int SystemUserID)
        {

            DataSet ds = null;
            PenaltyEntity cobj = null;
            List<PenaltyEntity> result = new List<PenaltyEntity>();

            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spGetAllActiveRequestPenaltyBySystemUserID",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.AddWithValue("@SystemUserID", SystemUserID);

                    conn.Open();

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    ds = new DataSet();
                    da.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        cobj = new PenaltyEntity();
                        cobj.RequestGUID = ds.Tables[0].Rows[i]["RequestGUID"].ToString();
                        cobj.RequestType = ds.Tables[0].Rows[i]["RequestType"].ToString();
                        cobj.PenaltyDetails = ds.Tables[0].Rows[i]["PenaltyDetails"].ToString();

                        result.Add(cobj);
                    }
                }
            }
            return result;
        }

        public PenaltyEntity GetEquipment(PenaltyEntity e)
        {
            PenaltyEntity result = null;

            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestsGetByRequestGUID",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@RequestGUID", SqlDbType.VarChar);

                    cmd.Parameters["@RequestGUID"].Value = e.RequestGUID;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new PenaltyEntity();
                            result.RequestGUID = reader["RequestGUID"].ToString();
                            result.Requestor = reader["Requestor"].ToString();
                            result.IsSuccess = true;
                        }
                    }
                }
            }
            return result;
        }
        public PenaltyEntity GetFacility(PenaltyEntity e)
        {
            PenaltyEntity result = null;

            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestFacilityGet",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@RequestFacilityGUID", SqlDbType.VarChar);

                    cmd.Parameters["@RequestFacilityGUID"].Value = e.RequestGUID;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new PenaltyEntity();
                            result.RequestGUID = reader["RequestFacilityGUID"].ToString();
                            result.Requestor = reader["FacilityRequestor"].ToString();
                            result.IsSuccess = true;
                        }
                    }
                }
            }
            return result;
        }
        public IEnumerable<PenaltyEntity> GetList()
        {
            //using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            //{
            //    using (SqlCommand cmd = new SqlCommand()
            //    {
            //        Connection = conn,
            //        CommandTimeout = DBConnection.GetConnectionTimeOut(),
            //        CommandText = "spPenaltyGet",
            //        CommandType = CommandType.StoredProcedure
            //    })
            //    {
            //        conn.Open();
            //        using (SqlDataReader reader = cmd.ExecuteReader())
            //        {
            //            while (reader.Read())
            //            {

            //                yield return new PenaltyEntity()
            //                {
            //                    PenaltyID = Convert.ToInt32(reader["PenaltyID"]),
            //                    Category = reader["Category"].ToString(),
            //                    CategoryCode = reader["CategoryCode"].ToString()
            //                };
            //            }
            //        }
            //    }
            //}
            throw new NotImplementedException();
        }

        public IEnumerable<PenaltyEntity> GetListOnlyAvailable()
        {
            //using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            //{
            //    using (SqlCommand cmd = new SqlCommand()
            //    {
            //        Connection = conn,
            //        CommandTimeout = DBConnection.GetConnectionTimeOut(),
            //        CommandText = "spRequestEquipmentGetOnlyAvailable",
            //        CommandType = CommandType.StoredProcedure
            //    })
            //    {
            //        conn.Open();
            //        using (SqlDataReader reader = cmd.ExecuteReader())
            //        {
            //            while (reader.Read())
            //            {

            //                yield return new PenaltyEntity()
            //                {
            //                    PenaltyID = Convert.ToInt32(reader["PenaltyID"]),
            //                    Category = reader["Category"].ToString(),
            //                    CategoryCode = reader["CategoryCode"].ToString(),
            //                    QuantityUsable = Convert.ToInt32(reader["QuantityUsable"])
            //                };
            //            }
            //        }
            //    }
            //}
            throw new NotImplementedException();
        }

        public PenaltyEntity Insert(PenaltyEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spPenaltyInsert",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@RequestType", SqlDbType.VarChar);
                    cmd.Parameters.Add("@RequestGUID", SqlDbType.VarChar);
                    cmd.Parameters.Add("@PenaltyDetails", SqlDbType.VarChar);
                    cmd.Parameters.Add("@CreatedBy", SqlDbType.BigInt);


                    cmd.Parameters["@RequestType"].Value = e.RequestType;
                    cmd.Parameters["@RequestGUID"].Value = e.RequestGUID;
                    cmd.Parameters["@PenaltyDetails"].Value = e.PenaltyDetails;
                    cmd.Parameters["@CreatedBy"].Value = e.CreatedBy;

                    conn.Open();
                    e.PenaltyID = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            return e;
        }

        public bool Update(PenaltyEntity e)
        {
            bool isUpdated = false;
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spPenaltyUpdate",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@PenaltyID", SqlDbType.Int);            
                    cmd.Parameters.Add("@isActive", SqlDbType.Bit);
                    cmd.Parameters.Add("@ModifiedBy", SqlDbType.Int);
                   

                    cmd.Parameters["@PenaltyID"].Value = e.PenaltyID;
                    cmd.Parameters["@isActive"].Value = e.isActive;
                    cmd.Parameters["@ModifiedBy"].Value = e.ModifiedBy;
                    conn.Open();

                    isUpdated = Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }

            return isUpdated;
        }
        public IEnumerable<PenaltyListEntity> GetFiltered(PenaltyListEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spPenaltyListGetFiltered",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@RowStart", SqlDbType.Int);
                    cmd.Parameters.Add("@NoOfRecord", SqlDbType.Int);
                    cmd.Parameters.Add("@SortColumn", SqlDbType.VarChar);
                    cmd.Parameters.Add("@SortDirection", SqlDbType.VarChar);

                    cmd.Parameters.Add("@RequestorID", SqlDbType.BigInt);
                    cmd.Parameters.Add("@RequestType", SqlDbType.VarChar);
                    cmd.Parameters.Add("@StudentName", SqlDbType.VarChar);


                    cmd.Parameters["@RowStart"].Value = e.RowStart;
                    cmd.Parameters["@NoOfRecord"].Value = e.NoOfRecord;


                    string sortColumn = "";

                    switch (e.SortColumn)
                    {
                        case 1:
                            sortColumn = "isActive";
                            break;
                        case 2:
                            sortColumn = "Requestor";
                            break;
                        case 3:
                            sortColumn = "RequestType";
                            break;
                        case 4:
                            sortColumn = "RequestGUID";
                            break;
                    }

                    cmd.Parameters["@SortColumn"].Value = sortColumn;
                    cmd.Parameters["@SortDirection"].Value = e.SortDirection;

                    cmd.Parameters["@RequestorID"].Value = e.RequestorID;
                    cmd.Parameters["@RequestType"].Value = e.RequestType;
                    cmd.Parameters["@StudentName"].Value = e.Requestor;

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        int totalNoOfRecords = 0;

                        while (reader.Read())
                        {
                            if (totalNoOfRecords == 0)
                                totalNoOfRecords = Convert.ToInt32(reader["TotalNoOfRecord"]);

                            yield return new PenaltyListEntity()
                            {
                                TotalRecord = totalNoOfRecords,
                                PenaltyID = Convert.ToInt32(reader["PenaltyID"]),
                                RequestsID = int.Parse(reader["RequestID"].ToString()),
                                FacilityRequestID = int.Parse(reader["FacilityRequestID"].ToString()),
                                RequestorID = int.Parse(reader["RequestorID"].ToString()),
                                isActive = Convert.ToBoolean(reader["isActive"]),
                                RequestGUID = reader["RequestGUID"].ToString(),
                                Requestor = reader["Requestor"].ToString(),
                                RequestType = reader["RequestType"].ToString(),

                            };
                        }
                    }
                }
            }
        }
    }
}