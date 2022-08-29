using ERSDAL.DB;
using ERSDAL.Service;
using ERSEntity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSDAL
{
    public class RequestEntityDAL : IBaseDAL<RequestsEntity, int>
    {
        public bool Delete(RequestsEntity e)
        {
            bool isDeleted = false;
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestsDelete",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@RequestID", SqlDbType.Int);
                    cmd.Parameters["@RequestID"].Value = e.RequestID;
                    conn.Open();

                    isDeleted = Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }

            return isDeleted;
        }

        public List<PenaltyEntity> GetAllActiveEquipmentRequestPenaltyBySystemUserID(int SystemUserID)
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
                    CommandText = "spGetAllActiveEquipmentRequestPenaltyBySystemUserID",
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

        public RequestsEntity CancelRequestEquipment(RequestsEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestsCancel",
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

        public RequestsEntity SelectRequestsByRequestGUID(string RequestGUID)
        {

            DataSet ds = null;
            RequestsEntity cobj = null;

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
                    cmd.Parameters.AddWithValue("@RequestGUID", RequestGUID);

                    conn.Open();

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    ds = new DataSet();
                    da.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        cobj = new RequestsEntity();
                        cobj.RequestGUID = ds.Tables[0].Rows[i]["RequestGUID"].ToString();
                        cobj.RequestID = Convert.ToInt32(ds.Tables[0].Rows[i]["RequestID"].ToString());
                        cobj.Requestor = ds.Tables[0].Rows[i]["Requestor"].ToString();
                        cobj.RequestDateTime = ds.Tables[0].Rows[i]["RequestDateTime"].ToString();
                        cobj.StartTime = ds.Tables[0].Rows[i]["StartTime"].ToString();
                        cobj.EndTime = ds.Tables[0].Rows[i]["EndTime"].ToString();
                        cobj.isApproved = Convert.ToInt32(ds.Tables[0].Rows[i]["isApproved"].ToString());
                        cobj.Remarks = ds.Tables[0].Rows[i]["Remarks"].ToString();

                    }
                }
            }
            return cobj;
        }

        public RequestsEntity Get(RequestsEntity e)
        {
            RequestsEntity result = null;

            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestsGet",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@RequestID", SqlDbType.BigInt);

                    cmd.Parameters["@RequestID"].Value = e.RequestID;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())

                        result = new RequestsEntity();
                        result.RequestID = Convert.ToInt32(reader["RequestID"]);
                        result.Requestor = reader["Requestor"].ToString();
                        result.RequestDateTime = reader["RequestDateTime"].ToString();
                        result.StartTime = reader["StartTime"].ToString();
                        result.EndTime = reader["EndTime"].ToString();
                        result.isApproved = Convert.ToInt32(reader["isApproved"]);
                        result.RequestGUID = reader["RequestGUID"].ToString();
                        result.Remarks = reader["Remarks"].ToString();
                    }
                }
            }
            return result;
        }
        public IEnumerable<RequestsEntity> GetList()
        {
            throw new NotImplementedException();
        }

        public RequestsEntity Insert(RequestsEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestsInsert",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@RequestID", SqlDbType.Int);
                    cmd.Parameters.Add("@Requestor", SqlDbType.VarChar);
                    cmd.Parameters.Add("@RequestDateTime", SqlDbType.VarChar);
                    cmd.Parameters.Add("@RequestGUID", SqlDbType.VarChar);
                    cmd.Parameters.Add("@StartTime", SqlDbType.VarChar);
                    cmd.Parameters.Add("@EndTime", SqlDbType.VarChar);
                    cmd.Parameters.Add("@isApproved", SqlDbType.Int);
                    cmd.Parameters.Add("@Remarks", SqlDbType.VarChar);
                    cmd.Parameters.Add("@Status", SqlDbType.VarChar);
                    cmd.Parameters.Add("@RequestorID", SqlDbType.VarChar);

                    cmd.Parameters["@RequestID"].Value = e.RequestID;
                    cmd.Parameters["@Requestor"].Value = e.Requestor;
                    cmd.Parameters["@RequestDateTime"].Value = e.RequestDateTime;
                    cmd.Parameters["@RequestGUID"].Value = e.RequestGUID;
                    cmd.Parameters["@StartTime"].Value = e.StartTime;
                    cmd.Parameters["@EndTime"].Value = e.EndTime;
                    cmd.Parameters["@isApproved"].Value = e.isApproved;
                    cmd.Parameters["@Remarks"].Value = e.Remarks;
                    cmd.Parameters["@Status"].Value = e.Status;
                    cmd.Parameters["@RequestorID"].Value = e.RequestorID;
                    conn.Open();

                    e.RequestID = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            return e;
        }

        public bool Update(RequestsEntity e)
        {
            bool isUpdated = false;
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestsUpdate",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@RequestID", SqlDbType.Int);
                    cmd.Parameters.Add("@Requestor", SqlDbType.VarChar);
                    cmd.Parameters.Add("@RequestDateTime", SqlDbType.VarChar);
                    cmd.Parameters.Add("@StartTime", SqlDbType.VarChar);
                    cmd.Parameters.Add("@EndTime", SqlDbType.VarChar);
                    cmd.Parameters.Add("@isApproved", SqlDbType.Int);
                    cmd.Parameters.Add("@RequestGUID", SqlDbType.VarChar);
                    cmd.Parameters.Add("@Remarks", SqlDbType.VarChar);

                    cmd.Parameters["@RequestID"].Value = e.RequestID;
                    cmd.Parameters["@Requestor"].Value = e.Requestor;
                    cmd.Parameters["@RequestDateTime"].Value = e.RequestDateTime;
                    cmd.Parameters["@StartTime"].Value = e.StartTime;
                    cmd.Parameters["@EndTime"].Value = e.EndTime;
                    cmd.Parameters["@isApproved"].Value = e.isApproved;
                    cmd.Parameters["@RequestGUID"].Value = e.RequestGUID;
                    cmd.Parameters["@Remarks"].Value = e.Remarks;
                    conn.Open();

                    isUpdated = Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }

            return isUpdated;
        }

        public IEnumerable<RequestsListEntity> GetFiltered(RequestsListEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestsListGetFiltered",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@RowStart", SqlDbType.Int);
                    cmd.Parameters.Add("@NoOfRecord", SqlDbType.Int);
                    cmd.Parameters.Add("@SortColumn", SqlDbType.VarChar);
                    cmd.Parameters.Add("@SortDirection", SqlDbType.VarChar);

                    cmd.Parameters.Add("@Requestor", SqlDbType.VarChar);
                    cmd.Parameters.Add("@RequestDateTime", SqlDbType.VarChar);
                    cmd.Parameters.Add("@StartTime", SqlDbType.VarChar);
                    cmd.Parameters.Add("@EndTime", SqlDbType.VarChar);
                    cmd.Parameters.Add("@isApproved", SqlDbType.Int);
                    cmd.Parameters.Add("@Remarks", SqlDbType.VarChar);
                    cmd.Parameters.Add("@RequestGUID", SqlDbType.VarChar);
                    cmd.Parameters.Add("@Status", SqlDbType.VarChar);
                    cmd.Parameters.Add("@ClaimedTime", SqlDbType.VarChar);
                    cmd.Parameters.Add("@ReturnedTime", SqlDbType.VarChar);
                    cmd.Parameters.Add("@RequestorID", SqlDbType.VarChar);
                    cmd.Parameters.Add("@StatusMode", SqlDbType.VarChar);
                    cmd.Parameters["@RowStart"].Value = e.RowStart;
                    cmd.Parameters["@NoOfRecord"].Value = e.NoOfRecord;

                    string sortColumn = "";

                    switch (e.SortColumn)
                    {
                        case 1:
                            sortColumn = "RequestDateTime";
                            break;
                        case 2:
                            sortColumn = "StartTime";
                            break;
                        case 3:
                            sortColumn = "EndTime";
                            break;
                        case 4:
                            sortColumn = "Status";
                            break;
                        case 5:
                            sortColumn = "Remarks";
                            break;
                        case 6:
                            sortColumn = "ClaimedTime";
                            break;
                        case 7:
                            sortColumn = "ReturnedTime";
                            break;
                        case 8:
                            sortColumn = "Requestor";
                            break;
                        case 9:
                            sortColumn = "RequestorID";
                            break;
                        case 10:
                            sortColumn = "RequestGUID";
                            break;
                    }

                    cmd.Parameters["@SortColumn"].Value = sortColumn;
                    cmd.Parameters["@SortDirection"].Value = e.SortDirection;

                    cmd.Parameters["@Requestor"].Value = e.Requestor;
                    cmd.Parameters["@RequestDateTime"].Value = e.RequestDateTime;
                    cmd.Parameters["@StartTime"].Value = e.StartTime;
                    cmd.Parameters["@EndTime"].Value = e.EndTime;
                    cmd.Parameters["@isApproved"].Value = e.isApproved;
                    cmd.Parameters["@RequestGUID"].Value = e.RequestGUID;
                    cmd.Parameters["@Remarks"].Value = e.Remarks;
                    cmd.Parameters["@Status"].Value = e.Status;
                    cmd.Parameters["@ClaimedTime"].Value = e.ClaimedTime;
                    cmd.Parameters["@ReturnedTime"].Value = e.ReturnedTime;
                    cmd.Parameters["@RequestorID"].Value = e.RequestorID;
                    cmd.Parameters["@StatusMode"].Value = e.StatusMode;

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        int totalNoOfRecords = 0;

                        while (reader.Read())
                        {
                            if (totalNoOfRecords == 0)
                                totalNoOfRecords = Convert.ToInt32(reader["TotalNoOfRecord"]);

                            yield return new RequestsListEntity()
                            {
                                TotalRecord = totalNoOfRecords,
                                RequestID = Convert.ToInt32(reader["RequestID"]),
                                Requestor = reader["Requestor"].ToString(),
                                RequestDateTime = reader["RequestDateTime"].ToString(),
                                StartTime = reader["StartTime"].ToString(),
                                EndTime = reader["EndTime"].ToString(),
                                isApproved = Convert.ToInt32(reader["isApproved"]),
                                RequestGUID = reader["RequestGUID"].ToString(),
                                Remarks = reader["Remarks"].ToString(),
                                Status = reader["Status"].ToString(),
                                ClaimedTime = reader["ClaimedTime"].ToString(),
                                ReturnedTime = reader["ReturnedTime"].ToString(),
                                RequestorID = Convert.ToInt32(reader["RequestorID"]),
                                PenaltyID = Convert.ToInt32(reader["PenaltyID"])
                            };
                        }
                    }
                }
            }
        }

        public bool BulkDelete(List<int> IDs)
        {
            bool result = false;
            DataTable dtIDs = null;
            dtIDs = new DataTable();
            dtIDs.Columns.Add("RequestID", typeof(int));
            if (IDs.Count > 0)
            {
                foreach (int id in IDs)
                {
                    dtIDs.Rows.Add(id);
                }
            }
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestsDelete",
                    CommandType = CommandType.StoredProcedure,
                })
                {
                    cmd.Parameters.Add("@IDs", SqlDbType.Structured);
                    conn.Open();
                    cmd.Parameters["@IDs"].Value = dtIDs;

                    result = Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }
            return result;
        }

    }

    public class RequestDetailsDAL : IBaseDAL<RequestDetailsEntity, int>
    {
        public bool Delete(RequestDetailsEntity e)
        {
            bool isDeleted = false;
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestDetailsDelete",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@RequestDetailsID", SqlDbType.Int);
                    cmd.Parameters["@RequestDetailsID"].Value = e.RequestDetailsID;
                    conn.Open();

                    isDeleted = Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }

            return isDeleted;
        }

        public List<RequestDetailsEntity> GetClaimedEquipmentsQuantity()
        {

            DataSet ds = null;
            RequestDetailsEntity cobj = null;
            List<RequestDetailsEntity> result = new List<RequestDetailsEntity>();

            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spGetClaimedEquipmentsQuantity",
                    CommandType = CommandType.StoredProcedure
                })
                {

                    conn.Open();

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    ds = new DataSet();
                    da.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        cobj = new RequestDetailsEntity();
                        cobj.Quantity = Convert.ToInt32(ds.Tables[0].Rows[i]["Quantity"].ToString());

                        result.Add(cobj);
                    }
                }
            }
            return result;
        }

        public List<RequestDetailsEntity> GetUnclaimedEquipmentsQuantity()
        {

            DataSet ds = null;
            RequestDetailsEntity cobj = null;
            List<RequestDetailsEntity> result = new List<RequestDetailsEntity>();

            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spGetUnclaimedEquipmentsQuantity",
                    CommandType = CommandType.StoredProcedure
                })
                {

                    conn.Open();

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    ds = new DataSet();
                    da.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        cobj = new RequestDetailsEntity();
                        cobj.Quantity = Convert.ToInt32(ds.Tables[0].Rows[i]["Quantity"].ToString());

                        result.Add(cobj);
                    }
                }
            }
            return result;
        }

        public RequestDetailsEntity Get(RequestDetailsEntity e)
        {
            RequestDetailsEntity result = null;

            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestDetailsGet",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@RequestGUID", SqlDbType.VarChar);

                    cmd.Parameters["@RequestGUID"].Value = e.RequestGUID;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())

                        result = new RequestDetailsEntity();
                        result.RequestDetailsID = Convert.ToInt32(reader["RequestDetailsID"]);
                        result.EquipmentCategoryID = Convert.ToInt32(reader["EquipmentCategoryID"]);
                        result.Quantity = Convert.ToInt32(reader["Quantity"]);
                        result.RequestGUID = reader["RequestGUID"].ToString();
                    }
                }
            }
            return result;
        }

        public List<RequestDetailsEntity> SelectRequestDetailsByDate(string StartTime, string EndTime)
        {

            DataSet ds = null;
            RequestDetailsEntity cobj = null;
            List<RequestDetailsEntity> result = new List<RequestDetailsEntity>();

            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestDetailsGetByDate",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.AddWithValue("@StartTime", StartTime);
                    cmd.Parameters.AddWithValue("@EndTime", EndTime);

                    conn.Open();

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    ds = new DataSet();
                    da.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        cobj = new RequestDetailsEntity();
                        cobj.RequestGUID = ds.Tables[0].Rows[i]["RequestGUID"].ToString();
                        cobj.RequestDetailsID = Convert.ToInt32(ds.Tables[0].Rows[i]["RequestDetailsID"].ToString());
                        cobj.Quantity = Convert.ToInt32(ds.Tables[0].Rows[i]["Quantity"].ToString());
                        cobj.EquipmentCategoryID = Convert.ToInt32(ds.Tables[0].Rows[i]["EquipmentCategoryID"].ToString());
                        cobj.StartTime = ds.Tables[0].Rows[i]["StartTime"].ToString();
                        cobj.EndTime = ds.Tables[0].Rows[i]["EndTime"].ToString();
                        cobj.Status = ds.Tables[0].Rows[i]["Status"].ToString();

                        result.Add(cobj);
                    }
                }
            }
            return result;
        }

        public List<RequestDetailsEntity> SelectRequestDetailsByStatusAndSystemUserID(string Status, int SystemUserID)
        {

            DataSet ds = null;
            RequestDetailsEntity cobj = null;
            List<RequestDetailsEntity> result = new List<RequestDetailsEntity>();

            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestDetailsGetByStatusAndSystemUserID",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.AddWithValue("@DateToday", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Status", Status);
                    cmd.Parameters.AddWithValue("@SystemUserID", SystemUserID);

                    conn.Open();

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    ds = new DataSet();
                    da.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        cobj = new RequestDetailsEntity();
                        cobj.EquipmentCategoryID = Convert.ToInt32(ds.Tables[0].Rows[i]["EquipmentCategoryID"].ToString());
                        cobj.Category = ds.Tables[0].Rows[i]["Category"].ToString();
                        cobj.RequestGUID = ds.Tables[0].Rows[i]["RequestGUID"].ToString();
                        cobj.Quantity = Convert.ToInt32(ds.Tables[0].Rows[i]["Quantity"].ToString());
                        cobj.StartTime = ds.Tables[0].Rows[i]["StartTime"].ToString();
                        cobj.EndTime = ds.Tables[0].Rows[i]["EndTime"].ToString();
                        cobj.Status = ds.Tables[0].Rows[i]["Status"].ToString();

                        result.Add(cobj);
                    }
                }
            }
            return result;
        }

        public List<RequestDetailsEntity> SelectRequestDetailsByRequestGUID(string RequestGUID)
        {

            DataSet ds = null;
            RequestDetailsEntity cobj = null;
            List<RequestDetailsEntity> result = new List<RequestDetailsEntity>();

            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestDetailsGetByRequestGUID",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.AddWithValue("@RequestGUID", RequestGUID);

                    conn.Open();

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    ds = new DataSet();
                    da.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        cobj = new RequestDetailsEntity();
                        cobj.RequestGUID = ds.Tables[0].Rows[i]["RequestGUID"].ToString();
                        cobj.RequestDetailsID = Convert.ToInt32(ds.Tables[0].Rows[i]["RequestDetailsID"].ToString());
                        cobj.Quantity = Convert.ToInt32(ds.Tables[0].Rows[i]["Quantity"].ToString());
                        cobj.EquipmentCategoryID = Convert.ToInt32(ds.Tables[0].Rows[i]["EquipmentCategoryID"].ToString());
                        cobj.StartTime = ds.Tables[0].Rows[i]["StartTime"].ToString();
                        cobj.EndTime = ds.Tables[0].Rows[i]["EndTime"].ToString();

                        result.Add(cobj);
                    }
                }
            }
            return result;
        }

        public List<RequestDetailsEntity> GetAllRequestDetails()
        {
            DataSet ds = null;
            RequestDetailsEntity cobj = null;
            List<RequestDetailsEntity> result = new List<RequestDetailsEntity>();

            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestDetailsGetAll",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    conn.Open();

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    ds = new DataSet();
                    da.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        cobj = new RequestDetailsEntity();
                        cobj.RequestGUID = ds.Tables[0].Rows[i]["RequestGUID"].ToString();
                        cobj.RequestDetailsID = Convert.ToInt32(ds.Tables[0].Rows[i]["RequestDetailsID"].ToString());
                        cobj.Quantity = Convert.ToInt32(ds.Tables[0].Rows[i]["Quantity"].ToString());
                        cobj.EquipmentCategoryID = Convert.ToInt32(ds.Tables[0].Rows[i]["EquipmentCategoryID"].ToString());
                        cobj.StartTime = ds.Tables[0].Rows[i]["StartTime"].ToString();
                        cobj.EndTime = ds.Tables[0].Rows[i]["EndTime"].ToString();

                        result.Add(cobj);
                    }
                }
            }
            return result;
        }

        public IEnumerable<RequestDetailsEntity> GetList()
        {
            throw new NotImplementedException();
        }

        public RequestDetailsEntity Insert(RequestDetailsEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestDetailsInsert",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@RequestDetailsID", SqlDbType.Int);
                    cmd.Parameters.Add("@EquipmentCategoryID", SqlDbType.Int);
                    cmd.Parameters.Add("@Quantity", SqlDbType.Int);
                    cmd.Parameters.Add("@RequestGUID", SqlDbType.VarChar);
                    cmd.Parameters.Add("@StartTime", SqlDbType.VarChar);
                    cmd.Parameters.Add("@EndTime", SqlDbType.VarChar);
                    cmd.Parameters.Add("@Status", SqlDbType.VarChar);
                    cmd.Parameters.Add("@RequestorID", SqlDbType.Int);

                    cmd.Parameters["@RequestDetailsID"].Value = e.RequestDetailsID;
                    cmd.Parameters["@EquipmentCategoryID"].Value = e.EquipmentCategoryID;
                    cmd.Parameters["@Quantity"].Value = e.Quantity;
                    cmd.Parameters["@RequestGUID"].Value = e.RequestGUID;
                    cmd.Parameters["@StartTime"].Value = e.StartTime;
                    cmd.Parameters["@EndTime"].Value = e.EndTime;
                    cmd.Parameters["@Status"].Value = e.Status;
                    cmd.Parameters["@RequestorID"].Value = e.RequestorID;
                    conn.Open();

                    e.RequestDetailsID = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            return e;
        }

        public bool Update(RequestDetailsEntity e)
        {
            bool isUpdated = false;
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestDetailsUpdate",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@RequestDetailsID", SqlDbType.Int);
                    cmd.Parameters.Add("@EquipmentCategoryID", SqlDbType.Int);
                    cmd.Parameters.Add("@Quantity", SqlDbType.Int);
                    cmd.Parameters.Add("@RequestGUID", SqlDbType.VarChar);

                    cmd.Parameters["@RequestDetailsID"].Value = e.RequestDetailsID;
                    cmd.Parameters["@EquipmentCategoryID"].Value = e.EquipmentCategoryID;
                    cmd.Parameters["@Quantity"].Value = e.Quantity;
                    cmd.Parameters["@RequestGUID"].Value = e.RequestGUID;
                    conn.Open();

                    isUpdated = Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }

            return isUpdated;
        }
    }
}

