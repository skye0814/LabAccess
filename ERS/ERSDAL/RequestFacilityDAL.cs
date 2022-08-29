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
    public class RequestFacilityDAL : IBaseDAL<RequestFacilityEntity, int>
    {
        public bool Delete(RequestFacilityEntity e)
        {
            bool isDeleted = false;
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestFacilityDelete",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@FacilityRequestID", SqlDbType.Int);
                    cmd.Parameters["@FacilityRequestID"].Value = e.FacilityRequestID;
                    conn.Open();

                    isDeleted = Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }

            return isDeleted;
        }

        public List<PenaltyEntity> GetAllActiveFacilityRequestPenaltyBySystemUserID(int SystemUserID)
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
                    CommandText = "spGetAllActiveFacilityRequestPenaltyBySystemUserID",
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
                        cobj.RequestGUID = ds.Tables[0].Rows[i]["RequestFacilityGUID"].ToString();
                        cobj.RequestType = ds.Tables[0].Rows[i]["RequestType"].ToString();
                        cobj.PenaltyDetails = ds.Tables[0].Rows[i]["PenaltyDetails"].ToString();

                        result.Add(cobj);
                    }
                }
            }
            return result;
        }

        public RequestFacilityEntity CancelRequestFacility(RequestFacilityEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestFacilityCancel",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@FacilityRequestID", SqlDbType.VarChar);

                    cmd.Parameters["@FacilityRequestID"].Value = e.FacilityRequestID;

                    conn.Open();
                    e.IsSuccess = Convert.ToBoolean(cmd.ExecuteScalar());
                }
                return e;
            }
        }

        // Method GET Requested Facilities for Unclaimed and Claimed Statuses --- In RequestFacilityAdd
        public List<RequestFacilityEntity> SelectRequestFacilityGetByRequestDate(string StartTime, string EndTime)
        {

            DataSet ds = null;
            RequestFacilityEntity cobj = null;
            List<RequestFacilityEntity> result = new List<RequestFacilityEntity>();

            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestFacilityGetByRequestDate",
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
                        cobj = new RequestFacilityEntity();
                        cobj.FacilityRequestID = Convert.ToInt32(ds.Tables[0].Rows[i]["FacilityRequestID"].ToString());
                        cobj.FacilityRequestor = ds.Tables[0].Rows[i]["FacilityRequestor"].ToString();
                        cobj.FacilityID = Convert.ToInt32(ds.Tables[0].Rows[i]["FacilityID"].ToString());
                        cobj.RequestDate = ds.Tables[0].Rows[i]["RequestDate"].ToString();
                        cobj.StartTime = ds.Tables[0].Rows[i]["StartTime"].ToString();
                        cobj.EndTime = ds.Tables[0].Rows[i]["EndTime"].ToString();
                        cobj.Status = ds.Tables[0].Rows[i]["Status"].ToString();

                        result.Add(cobj);
                    }
                }
            }
            return result;
        }

        public List<RequestFacilityEntity> SelectRequestFacilityGetByStatusAndSystemUserID(string Status, int SystemUserID)
        {

            DataSet ds = null;
            RequestFacilityEntity cobj = null;
            List<RequestFacilityEntity> result = new List<RequestFacilityEntity>();

            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestFacilityGetByStatusAndSystemUserID",
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
                        cobj = new RequestFacilityEntity();
                        cobj.FacilityRequestorID = Convert.ToInt32(ds.Tables[0].Rows[i]["FacilityRequestorID"].ToString());
                        cobj.FacilityID = Convert.ToInt32(ds.Tables[0].Rows[i]["FacilityID"].ToString());
                        cobj.StartTime = ds.Tables[0].Rows[i]["StartTime"].ToString();
                        cobj.EndTime = ds.Tables[0].Rows[i]["EndTime"].ToString();
                        cobj.Status = ds.Tables[0].Rows[i]["Status"].ToString();
                        cobj.RoomNumber = ds.Tables[0].Rows[i]["RoomNumber"].ToString();

                        result.Add(cobj);
                    }
                }
            }
            return result;
        }

        // Method GET Requested Facilities for Claimed Status --- For the dashboard page
        public List<RequestFacilityEntity> GetClaimedFacilityByCurrentDate()
        {

            DataSet ds = null;
            RequestFacilityEntity cobj = null;
            List<RequestFacilityEntity> result = new List<RequestFacilityEntity>();

            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spGetClaimedFacilityByCurrentDate",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.AddWithValue("@GetDate", DateTime.Now);
                    conn.Open();

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    ds = new DataSet();
                    da.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        cobj = new RequestFacilityEntity();
                        cobj.FacilityID = Convert.ToInt32(ds.Tables[0].Rows[i]["FacilityID"].ToString());
                        cobj.StartTime = ds.Tables[0].Rows[i]["StartTime"].ToString();
                        cobj.EndTime = ds.Tables[0].Rows[i]["EndTime"].ToString();
                        cobj.Status = ds.Tables[0].Rows[i]["Status"].ToString();
                        cobj.RoomNumber = ds.Tables[0].Rows[i]["RoomNumber"].ToString();

                        result.Add(cobj);
                    }
                }
            }
            return result;
        }

        public RequestFacilityEntity Get(RequestFacilityEntity e)
        {
            RequestFacilityEntity result = null;

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
                    cmd.Parameters.Add("@FacilityRequestID", SqlDbType.BigInt);
                    cmd.Parameters.Add("@RequestFacilityGUID", SqlDbType.VarChar);

                    cmd.Parameters["@FacilityRequestID"].Value = e.FacilityRequestID;
                    cmd.Parameters["@RequestFacilityGUID"].Value = e.RequestFacilityGUID;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new RequestFacilityEntity();
                            result.FacilityRequestID = Convert.ToInt32(reader["FacilityRequestID"]);
                            result.FacilityID = Convert.ToInt32(reader["FacilityID"]);
                            result.FacilityRequestor = reader["FacilityRequestor"].ToString();
                            result.FacilityID = Convert.ToInt32(reader["FacilityID"]);
                            result.RequestDate = Convert.ToDateTime(reader["RequestDate"]).ToString("M/d/yyyy hh:mm tt");
                            result.StartTime = Convert.ToDateTime(reader["StartTime"]).ToString("M/d/yyyy hh:mm tt");
                            result.Status = reader["Status"].ToString();
                            result.EndTime = Convert.ToDateTime(reader["EndTime"]).ToString("M/d/yyyy hh:mm tt");
                            result.Remarks = reader["Remarks"].ToString();
                            result.RoomNumber = reader["RoomNumber"].ToString();
                            result.RequestFacilityGUID = reader["RequestFacilityGUID"].ToString();
                            result.ClaimedTime = reader["ClaimedTime"].ToString();
                            result.ReturnedTime = reader["ReturnedTime"].ToString();
                            result.Schedule = reader["Schedule"].ToString();
                        }
                        
                    }
                }
            }
            return result;
        }

        public List<RequestFacilityEntity> SelectRequestFacilityGetReservedVacantSchedulesByDate(string StartTime, string EndTime)
        {

            DataSet ds = null;
            RequestFacilityEntity cobj = null;
            List<RequestFacilityEntity> result = new List<RequestFacilityEntity>();

            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestFacilityGetReservedVacantSchedulesByDate",
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
                        cobj = new RequestFacilityEntity();
                        cobj.FacilityRequestID = Convert.ToInt32(ds.Tables[0].Rows[i]["FacilityRequestID"].ToString());
                        cobj.FacilityRequestor = ds.Tables[0].Rows[i]["FacilityRequestor"].ToString();
                        cobj.FacilityID = Convert.ToInt32(ds.Tables[0].Rows[i]["FacilityID"].ToString());
                        cobj.RequestDate = ds.Tables[0].Rows[i]["RequestDate"].ToString();
                        cobj.StartTime = ds.Tables[0].Rows[i]["StartTime"].ToString();
                        cobj.EndTime = ds.Tables[0].Rows[i]["EndTime"].ToString();
                        cobj.Status = ds.Tables[0].Rows[i]["Status"].ToString();
                        cobj.Schedule = ds.Tables[0].Rows[i]["Schedule"].ToString();

                        result.Add(cobj);
                    }
                }
            }
            return result;
        }

        public List<RequestFacilityEntity> SelectRequestFacilityGetReservedClassSchedulesByDate(string StartTime)
        {

            DataSet ds = null;
            RequestFacilityEntity cobj = null;
            List<RequestFacilityEntity> result = new List<RequestFacilityEntity>();

            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestFacilityGetReservedClassSchedulesByDate",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.AddWithValue("@StartTime", StartTime);

                    conn.Open();

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    ds = new DataSet();
                    da.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        cobj = new RequestFacilityEntity();
                        cobj.FacilityRequestID = Convert.ToInt32(ds.Tables[0].Rows[i]["FacilityRequestID"].ToString());
                        cobj.FacilityRequestor = ds.Tables[0].Rows[i]["FacilityRequestor"].ToString();
                        cobj.FacilityID = Convert.ToInt32(ds.Tables[0].Rows[i]["FacilityID"].ToString());
                        cobj.RequestDate = ds.Tables[0].Rows[i]["RequestDate"].ToString();
                        cobj.StartTime = ds.Tables[0].Rows[i]["StartTime"].ToString();
                        cobj.EndTime = ds.Tables[0].Rows[i]["EndTime"].ToString();
                        cobj.Status = ds.Tables[0].Rows[i]["Status"].ToString();
                        cobj.Schedule = ds.Tables[0].Rows[i]["Schedule"].ToString();

                        result.Add(cobj);
                    }
                }
            }
            return result;
        }

        public IEnumerable<RequestFacilityEntity> GetList()
        {
            throw new NotImplementedException();
        }

        public RequestFacilityEntity Insert(RequestFacilityEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestFacilityInsert",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@FacilityRequestID", SqlDbType.VarChar);
                    cmd.Parameters.Add("@RequestFacilityGUID", SqlDbType.VarChar);
                    cmd.Parameters.Add("@FacilityRequestor", SqlDbType.VarChar);
                    cmd.Parameters.Add("@FacilityID", SqlDbType.VarChar);
                    cmd.Parameters.Add("@StartTime", SqlDbType.VarChar);
                    cmd.Parameters.Add("@EndTime", SqlDbType.VarChar);
                    cmd.Parameters.Add("@Status", SqlDbType.VarChar);
                    cmd.Parameters.Add("@Remarks", SqlDbType.VarChar);
                    cmd.Parameters.Add("@FacilityRequestorID", SqlDbType.Int);
                    cmd.Parameters.Add("@GetDate", SqlDbType.DateTime);
                    cmd.Parameters.Add("@Schedule", SqlDbType.VarChar);


                    cmd.Parameters["@FacilityRequestID"].Value = e.FacilityRequestID;
                    cmd.Parameters["@RequestFacilityGUID"].Value = e.RequestFacilityGUID;
                    cmd.Parameters["@FacilityRequestor"].Value = e.FacilityRequestor;
                    cmd.Parameters["@FacilityID"].Value = e.FacilityID;
                    cmd.Parameters["@StartTime"].Value = e.StartTime;
                    cmd.Parameters["@EndTime"].Value = e.EndTime;
                    cmd.Parameters["@Status"].Value = e.Status;
                    cmd.Parameters["@Remarks"].Value = e.Remarks;
                    cmd.Parameters["@FacilityRequestorID"].Value = e.FacilityRequestorID;
                    cmd.Parameters["@GetDate"].Value = DateTime.Now;
                    cmd.Parameters["@Schedule"].Value = e.Schedule;
                    conn.Open();

                    e.FacilityRequestID = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            return e;
        }

        public bool Update(RequestFacilityEntity e)
        {
            bool isUpdated = false;
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestFacilityUpdate",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@FacilityRequestID", SqlDbType.VarChar);
                    cmd.Parameters.Add("@FacilityID", SqlDbType.VarChar);
                    cmd.Parameters.Add("@StartTime", SqlDbType.VarChar);
                    cmd.Parameters.Add("@EndTime", SqlDbType.VarChar);
                    cmd.Parameters.Add("@Status", SqlDbType.VarChar);
                    cmd.Parameters.Add("@Remarks", SqlDbType.VarChar);

                    cmd.Parameters["@FacilityRequestID"].Value = e.FacilityRequestID;
                    cmd.Parameters["@FacilityID"].Value = e.FacilityID;
                    cmd.Parameters["@StartTime"].Value = e.StartTime;
                    cmd.Parameters["@EndTime"].Value = e.EndTime;
                    cmd.Parameters["@Status"].Value = e.Status;
                    cmd.Parameters["@Remarks"].Value = e.Remarks;
                    conn.Open();

                    isUpdated = Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }

            return isUpdated;
        }

        public IEnumerable<RequestFacilityListEntity> GetFiltered(RequestFacilityListEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestFacilityListGetFiltered",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@RowStart", SqlDbType.Int);
                    cmd.Parameters.Add("@NoOfRecord", SqlDbType.Int);
                    cmd.Parameters.Add("@SortColumn", SqlDbType.VarChar);
                    cmd.Parameters.Add("@SortDirection", SqlDbType.VarChar);
                    cmd.Parameters.Add("@FacilityID", SqlDbType.VarChar);
                    cmd.Parameters.Add("@RequestFacilityGUID", SqlDbType.VarChar);

                    cmd.Parameters.Add("@RequestDate", SqlDbType.VarChar);
                    cmd.Parameters.Add("@StartTime", SqlDbType.VarChar);
                    cmd.Parameters.Add("@EndTime", SqlDbType.VarChar);
                    cmd.Parameters.Add("@Status", SqlDbType.VarChar);
                    cmd.Parameters.Add("@Remarks", SqlDbType.VarChar);
                    cmd.Parameters.Add("@FacilityRequestor", SqlDbType.VarChar);
                    cmd.Parameters.Add("@FacilityRequestorID", SqlDbType.VarChar);
                    cmd.Parameters.Add("@StatusMode", SqlDbType.VarChar);
                    cmd.Parameters["@RowStart"].Value = e.RowStart;
                    cmd.Parameters["@NoOfRecord"].Value = e.NoOfRecord;

                    string sortColumn = "";

                    switch (e.SortColumn)
                    {
                        case 1:
                            sortColumn = "Facility";
                            break;
                        case 2:
                            sortColumn = "RequestDate";
                            break;
                        case 3:
                            sortColumn = "StartTime";
                            break;
                        case 4:
                            sortColumn = "EndTime";
                            break;
                        case 5:
                            sortColumn = "FacilityRequestor";
                            break;
                        case 6:
                            sortColumn = "Status";
                            break;
                        case 7:
                            sortColumn = "Remarks";
                            break;
                        case 8:
                            sortColumn = "RequestFacilityGUID";
                            break;

                    }

                    cmd.Parameters["@SortColumn"].Value = sortColumn;
                    cmd.Parameters["@SortDirection"].Value = e.SortDirection;
                    cmd.Parameters["@RequestDate"].Value = e.RequestDate;
                    cmd.Parameters["@StartTime"].Value = e.StartTime;
                    cmd.Parameters["@EndTime"].Value = e.EndTime;
                    cmd.Parameters["@Remarks"].Value = e.Remarks;
                    cmd.Parameters["@Status"].Value = e.Status;
                    cmd.Parameters["@FacilityRequestor"].Value = e.FacilityRequestor;
                    cmd.Parameters["@FacilityID"].Value = e.FacilityID;
                    cmd.Parameters["@RequestFacilityGUID"].Value = e.RequestFacilityGUID;
                    cmd.Parameters["@FacilityRequestorID"].Value = e.FacilityRequestorID;
                    cmd.Parameters["@StatusMode"].Value = e.StatusMode;

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        int totalNoOfRecords = 0;

                        while (reader.Read())
                        {
                            if (totalNoOfRecords == 0)
                                totalNoOfRecords = Convert.ToInt32(reader["TotalNoOfRecord"]);

                            yield return new RequestFacilityListEntity()
                            {
                                TotalRecord = totalNoOfRecords,
                                FacilityRequestID = Convert.ToInt32(reader["FacilityRequestID"]),
                                RequestFacilityGUID = reader["RequestFacilityGUID"].ToString(),
                                FacilityRequestor = reader["FacilityRequestor"].ToString(),
                                FacilityID = Convert.ToInt32(reader["FacilityID"]),
                                RequestDate = reader["RequestDate"].ToString(),
                                StartTime = reader["StartTime"].ToString(),
                                EndTime = reader["EndTime"].ToString(),
                                Status = reader["Status"].ToString(),
                                Remarks = reader["Remarks"].ToString(),
                                Facility = reader["Facility"].ToString(),
                                FacilityRequestorID = Convert.ToInt32(reader["FacilityRequestorID"]),
                                PenaltyID = Convert.ToInt32(reader["PenaltyID"]),
                                ClaimedTime = reader["ClaimedTime"].ToString(),
                                ReturnedTime = reader["ReturnedTime"].ToString()
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
            dtIDs.Columns.Add("FacilityRequestID", typeof(int));
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
                    CommandText = "spRequestFacilityDelete",
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

        public static implicit operator RequestFacilityDAL(RequestFacilityEntity v)
        {
            throw new NotImplementedException();
        }

        public static implicit operator RequestFacilityDAL(RequestFacilityListEntity v)
        {
            throw new NotImplementedException();
        }
    }
}
