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
    public class ClaimReturnEquipmentDAL : IBaseDAL<ClaimReturnEquipmentEntity, int>
    {
        public bool Delete(ClaimReturnEquipmentEntity e)
        {
            throw new NotImplementedException();
        }

        public bool CancelUnclaimedEquipmentRequests()
        {
            bool isSuccess = false;
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spCancelUnclaimedEquipmentRequests",
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

        public ClaimReturnEquipmentEntity Get(ClaimReturnEquipmentEntity e)
        {
            ClaimReturnEquipmentEntity result = null;

            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestEquipmentGet",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@RequestGUID", SqlDbType.VarChar);
                    cmd.Parameters.Add("@requestEquipmentItemID", SqlDbType.Int);

                    cmd.Parameters["@RequestGUID"].Value = e.RequestGUID;
                    cmd.Parameters["@requestEquipmentItemID"].Value = e.RequestEquipmentItemID;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new ClaimReturnEquipmentEntity();
                            result.RequestGUID = reader["RequestGUID"].ToString();
                            result.Requestor = reader["Requestor"].ToString();
                            result.RequestDateTime = Convert.ToDateTime(reader["RequestDateTime"]).ToString("M/d/yyyy hh:mm tt");
                            result.StartTime = Convert.ToDateTime(reader["StartTime"]).ToString("M/d/yyyy hh:mm tt");
                            result.EndTime = Convert.ToDateTime(reader["EndTime"]).ToString("M/d/yyyy hh:mm tt");
                            result.Status = reader["Status"].ToString();
                        }
                    }
                }
            }
            return result;
        }
        public ClaimReturnEquipmentEntity CheckRequestGUID(ClaimReturnEquipmentEntity e)
        {
            ClaimReturnEquipmentEntity result = null;

            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestEquipmentCheckRequestGUID",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@RequestGUID", SqlDbType.VarChar);
                    cmd.Parameters.Add("@requestEquipmentItemID", SqlDbType.Int);

                    cmd.Parameters["@RequestGUID"].Value = e.RequestGUID;
                    cmd.Parameters["@requestEquipmentItemID"].Value = e.RequestEquipmentItemID;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new ClaimReturnEquipmentEntity();
                            result.RequestGUID = reader["RequestGUID"].ToString();
                            result.Requestor = reader["Requestor"].ToString();
                            result.RequestDateTime = Convert.ToDateTime(reader["RequestDateTime"]).ToString("yyyy, MMMM dd h:mm tt");
                            result.StartTime = Convert.ToDateTime(reader["StartTime"]).ToString("h:mm tt");
                            result.EndTime = Convert.ToDateTime(reader["EndTime"]).ToString("h:mm tt");
                            result.Status = reader["Status"].ToString();
                        }
                    }
                }
            }
            return result;
        }
        public ClaimReturnEquipmentEntity GetItem(ClaimReturnEquipmentEntity e)
        {
            ClaimReturnEquipmentEntity result = null;

            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestEquipmentItemGet",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@requestEquipmentItemID", SqlDbType.Int);
                    cmd.Parameters["@requestEquipmentItemID"].Value = e.RequestEquipmentItemID;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new ClaimReturnEquipmentEntity();
                            result.RequestEquipmentItemID = Convert.ToInt32(reader["RequestEquipmentItemID"]);
                            result.Status = reader["Status"].ToString();

                        }
                    }
                }
            }
            return result;
        }

        public IEnumerable<ClaimReturnEquipmentEntity> GetList()
        {
            throw new NotImplementedException();
        }
        public RequestDetailsEntity UpdateDatabase(RequestDetailsEntity e) // Update RequestEquipmentItem based on RequestDetails
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestEquipmentItemDetailsUpdate",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@equipmentCategoryID", SqlDbType.Int);
                    cmd.Parameters.Add("@quantity", SqlDbType.Int);
                    cmd.Parameters.Add("@requestGUID", SqlDbType.VarChar);


                    cmd.Parameters["@equipmentCategoryID"].Value = e.EquipmentCategoryID;
                    cmd.Parameters["@quantity"].Value = e.Quantity;
                    cmd.Parameters["@requestGUID"].Value = e.RequestGUID;

                    conn.Open();
                    e.RequestEquipmentItemID = Convert.ToInt32(cmd.ExecuteScalar());
                }
                return e;
            }
        }
        public ClaimReturnEquipmentEntity Insert(ClaimReturnEquipmentEntity e)
        {
            throw new NotImplementedException();
        }
        public ClaimReturnEquipmentEntity RequestEquipmentItemClaim(ClaimReturnEquipmentEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestEquipmentItemClaim",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@EquipmentItemCode", SqlDbType.VarChar);
                    cmd.Parameters.Add("@requestEquipmentItemID", SqlDbType.Int);

                    cmd.Parameters["@EquipmentItemCode"].Value = e.EquipmentItemCode;
                    cmd.Parameters["@requestEquipmentItemID"].Value = e.RequestEquipmentItemID;
                    conn.Open();

                    e.intReturnValue = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            return e;
        }
        public ClaimReturnEquipmentEntity RequestEquipmentItemReturn(ClaimReturnEquipmentEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestEquipmentItemReturn",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@EquipmentItemCode", SqlDbType.VarChar);
                    cmd.Parameters.Add("@requestEquipmentItemID", SqlDbType.Int);

                    cmd.Parameters["@EquipmentItemCode"].Value = e.EquipmentItemCode;
                    cmd.Parameters["@requestEquipmentItemID"].Value = e.RequestEquipmentItemID;
                    conn.Open();

                    e.intReturnValue = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            return e;
        }

        public bool Update(ClaimReturnEquipmentEntity e)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<ClaimReturnEquipmentListEntity> GetFiltered(ClaimReturnEquipmentListEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestEquipmentItemList",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@RowStart", SqlDbType.Int);
                    cmd.Parameters.Add("@NoOfRecord", SqlDbType.Int);
                    cmd.Parameters.Add("@SortColumn", SqlDbType.VarChar);
                    cmd.Parameters.Add("@SortDirection", SqlDbType.VarChar);

                    cmd.Parameters.Add("@RequestGUID", SqlDbType.VarChar);
                    cmd.Parameters["@RowStart"].Value = e.RowStart;
                    cmd.Parameters["@NoOfRecord"].Value = e.NoOfRecord;

                    string sortColumn = "";

                    switch (e.SortColumn)
                    {
                        case 1:
                            sortColumn = "Category";
                            break;
                        case 2:
                            sortColumn = "EquipmentItemCode";
                            break;
                        case 3:
                            sortColumn = "isClaimed";
                            break;
                        case 4:
                            sortColumn = "Status";
                            break;
                    }

                    cmd.Parameters["@SortColumn"].Value = sortColumn;
                    cmd.Parameters["@SortDirection"].Value = e.SortDirection;

                    cmd.Parameters["@RequestGUID"].Value = e.RequestGUID;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        int totalNoOfRecords = 0;

                        while (reader.Read())
                        {
                            if (totalNoOfRecords == 0)
                                totalNoOfRecords = Convert.ToInt32(reader["TotalNoOfRecord"]);

                            yield return new ClaimReturnEquipmentListEntity()
                            {
                                TotalRecord = totalNoOfRecords,
                                RequestEquipmentItemID = Convert.ToInt32(reader["RequestEquipmentItemID"]),
                                RequestGUID = reader["RequestGUID"].ToString(),
                                Category = reader["Category"].ToString(),
                                EquipmentItemCode = (reader["EquipmentItemCode"].ToString()),
                                EquipmentItemID = Convert.ToInt32(reader["EquipmentItemID"]),
                                isClaimed = Convert.ToBoolean(reader["isClaimed"]),
                                Status = reader["Status"].ToString(),
                            };
                        }
                    }
                }
            }
        }
        public ClaimReturnEquipmentEntity ResetClaimedItems(ClaimReturnEquipmentEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestEquipmentItemReset",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@requestGUID", SqlDbType.VarChar);

                    cmd.Parameters["@requestGUID"].Value = e.RequestGUID;

                    conn.Open();
                    e.IsSuccess = Convert.ToBoolean(cmd.ExecuteScalar());
                }
                return e;
            }
        }
        public ClaimReturnEquipmentEntity ConfirmClaimedItems(ClaimReturnEquipmentEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestEquipmentItemConfirm",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@requestGUID", SqlDbType.VarChar);
                    cmd.Parameters.Add("@GetDate", SqlDbType.DateTime);

                    cmd.Parameters["@requestGUID"].Value = e.RequestGUID;
                    cmd.Parameters["@GetDate"].Value = DateTime.Now;

                    conn.Open();
                    e.IsSuccess = Convert.ToBoolean(cmd.ExecuteScalar());
                }
                return e;
            }
        }
        public ClaimReturnEquipmentListEntity UpdateEquipmentItem(ClaimReturnEquipmentListEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestEquipmentItemUpdateItem",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@equipmentItemID", SqlDbType.VarChar);
                    cmd.Parameters.Add("@requestGUID", SqlDbType.VarChar);

                    cmd.Parameters["@equipmentItemID"].Value = e.EquipmentItemID;
                    cmd.Parameters["@requestGUID"].Value = e.RequestGUID;

                    conn.Open();
                    e.IsSuccess = Convert.ToBoolean(cmd.ExecuteScalar());
                }
                return e;
            }
        }

        public int CheckUnreturnedItems(string requestGUID)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestEquipmentItemCheckUnreturnedItems",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@requestGUID", SqlDbType.VarChar);

                    cmd.Parameters["@requestGUID"].Value = requestGUID;

                    conn.Open();
                    result = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            return result;
        }
        public ClaimReturnEquipmentEntity CheckClaimedItems(ClaimReturnEquipmentEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spCheckClaimedItem",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@requestGUID", SqlDbType.VarChar);

                    cmd.Parameters["@requestGUID"].Value = e.RequestGUID;

                    conn.Open();
                    e.IsSuccess = Convert.ToBoolean(cmd.ExecuteScalar());
                }
                return e;
            }
        }
    } 
}