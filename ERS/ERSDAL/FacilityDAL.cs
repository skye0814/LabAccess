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
    public class FacilityDAL : IBaseDAL<FacilityEntity, int>
    {
        public bool Delete(FacilityEntity e)
        {
            throw new NotImplementedException();
        }

        public FacilityEntity Get(FacilityEntity e)
        {
            FacilityEntity result = null;

            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spFacilityGet",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@FacilityID", SqlDbType.BigInt);
                    cmd.Parameters.Add("@RoomNumber", SqlDbType.VarChar);
                    cmd.Parameters.Add("@RoomType", SqlDbType.VarChar);
                    cmd.Parameters.Add("@isActive", SqlDbType.VarChar);
                    cmd.Parameters.Add("@isAvailable", SqlDbType.VarChar);
                    cmd.Parameters.Add("@Comments", SqlDbType.VarChar);

                    if (e.Mode == "check")
                        cmd.Parameters["@FacilityID"].Value = 0;
                    else
                        cmd.Parameters["@FacilityID"].Value = e.FacilityID;
                    cmd.Parameters["@RoomNumber"].Value = e.RoomNumber;
                    cmd.Parameters["@RoomType"].Value = e.RoomType;
                    cmd.Parameters["@isActive"].Value = e.isActive;
                    cmd.Parameters["@isAvailable"].Value = e.isAvailable;
                    cmd.Parameters["@Comments"].Value = e.Comments;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new FacilityEntity();
                            result.FacilityID = Convert.ToInt32(reader["FacilityID"]);
                            result.RoomNumber = reader["RoomNumber"].ToString();
                            result.RoomDescription = reader["RoomDescription"].ToString();
                            result.RoomType = reader["RoomType"].ToString();
                            result.isActive = Convert.ToBoolean(reader["isActive"]);
                            result.isAvailable = Convert.ToBoolean(reader["isAvailable"]);
                            result.Comments = reader["Comments"].ToString();
                        }
                        if (e.FacilityID != 0)
                        {
                            if (result != null)
                            {
                                if (e.RoomNumber == result.RoomNumber)
                                    result = null;
                            }
                            else
                            {
                                result = new FacilityEntity();
                                result.Status = "recheck";
                            }

                        }
                        if (e.Mode == "check")
                        {
                            if (result != null && result.FacilityID != 0)
                            {
                                if (e.FacilityID == result.FacilityID)
                                {
                                    result = new FacilityEntity();
                                }
                                else
                                {
                                    if (e.RoomNumber == result.RoomNumber)
                                        result = null;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        public IEnumerable<FacilityEntity> GetList()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FacilityEntity> GetListOnlyAvailable()
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestFacilityGetOnlyAvailable",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            yield return new FacilityEntity()
                            {
                                FacilityID = Convert.ToInt32(reader["FacilityID"]),
                                RoomNumber = reader["RoomNumber"].ToString()
                            };
                        }
                    }
                }
            }
        }

        public FacilityEntity Insert(FacilityEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spFacilityInsert",
                    CommandType = CommandType.StoredProcedure
                })
                {
                   
                    cmd.Parameters.Add("@RoomType", SqlDbType.VarChar);
                    cmd.Parameters.Add("@RoomDescription", SqlDbType.VarChar);
                    cmd.Parameters.Add("@RoomNumber", SqlDbType.VarChar);
                    cmd.Parameters.Add("@Comments", SqlDbType.VarChar);
                    cmd.Parameters.Add("@CreatedBy", SqlDbType.Int);

                    
                    cmd.Parameters["@RoomType"].Value = e.RoomType;
                    cmd.Parameters["@RoomNumber"].Value = e.RoomNumber.ToUpper();
                    cmd.Parameters["@RoomDescription"].Value = e.RoomDescription;
                    cmd.Parameters["@Comments"].Value = e.Comments;
                    cmd.Parameters["@CreatedBy"].Value = e.CreatedBy;
                    conn.Open();
                    e.FacilityID = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            return e;
        }
        public IEnumerable<FacilityListEntity> GetFiltered(FacilityListEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spFacilityListGetFiltered",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@RowStart", SqlDbType.Int);
                    cmd.Parameters.Add("@NoOfRecord", SqlDbType.Int);
                    cmd.Parameters.Add("@SortColumn", SqlDbType.VarChar);
                    cmd.Parameters.Add("@SortDirection", SqlDbType.VarChar);

                    cmd.Parameters.Add("@RoomNumber", SqlDbType.VarChar);
                    cmd.Parameters.Add("@RoomType", SqlDbType.VarChar);
                    cmd.Parameters["@RowStart"].Value = e.RowStart;
                    cmd.Parameters["@NoOfRecord"].Value = e.NoOfRecord;

                    string sortColumn = "";

                    switch (e.SortColumn)
                    {
                        case 1:
                            sortColumn = "isActive";
                            break;
                        case 2:
                            sortColumn = "RoomNumber";
                            break;
                        case 3:
                            sortColumn = "RoomType";
                            break;
                        case 4:
                            sortColumn = "RoomDescription";
                            break;
                        case 5:
                            sortColumn = "isAvailable";
                            break;
                        case 6:
                            sortColumn = "NoOfTimesBooked";
                            break;
                        case 7:
                            sortColumn = "Comments";
                            break;
                        case 8:
                            sortColumn = "TimeIn";
                            break;
                        case 9:
                            sortColumn = "TimeOut";
                            break;
                        case 10:
                            sortColumn = "NextSchedule";
                            break;
                    }

                    cmd.Parameters["@SortColumn"].Value = sortColumn;
                    cmd.Parameters["@SortDirection"].Value = e.SortDirection;

                    cmd.Parameters["@RoomNumber"].Value = e.RoomNumber;
                    cmd.Parameters["@RoomType"].Value = e.RoomType;


                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        int totalNoOfRecords = 0;

                        while (reader.Read())
                        {
                            if (totalNoOfRecords == 0)
                                totalNoOfRecords = Convert.ToInt32(reader["TotalNoOfRecord"]);

                            yield return new FacilityListEntity()
                            {
                                TotalRecord = totalNoOfRecords,
                                FacilityID = Convert.ToInt32(reader["FacilityID"]),
                                RoomNumber = reader["RoomNumber"].ToString(),
                                RoomType = reader["RoomType"].ToString(),
                                RoomDescription = reader["RoomDescription"].ToString(),
                                isAvailable = Convert.ToBoolean(reader["isAvailable"]),
                                isActive = Convert.ToBoolean(reader["isActive"]),
                                TimeIn = reader["TimeIn"].ToString(),
                                TimeOut = reader["TimeOut"].ToString(),
                                NextSchedule = reader["NextSchedule"].ToString(),
                                Comments = reader["Comments"].ToString(),
                            };
                        }
                    }
                }
            }
        }

        public bool Update(FacilityEntity e)
        {
            bool isUpdated = false;
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spFacilityUpdate",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@FacilityID", SqlDbType.Int);
                    cmd.Parameters.Add("@RoomType", SqlDbType.VarChar);
                    cmd.Parameters.Add("@RoomNumber", SqlDbType.VarChar);
                    cmd.Parameters.Add("@isActive", SqlDbType.Bit);
                    cmd.Parameters.Add("@Comments", SqlDbType.VarChar);
                    cmd.Parameters.Add("@ModifiedBy", SqlDbType.Int);
                   
                    cmd.Parameters["@FacilityID"].Value = e.FacilityID;
                    cmd.Parameters["@RoomType"].Value = e.RoomType;
                    cmd.Parameters["@RoomNumber"].Value = e.RoomNumber.ToUpper();
                    cmd.Parameters["@isActive"].Value = e.isActive; 
                    cmd.Parameters["@Comments"].Value = e.Comments; 
                    cmd.Parameters["@ModifiedBy"].Value = e.ModifiedBy;
                    conn.Open();

                    isUpdated = Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }

            return isUpdated;
        }
        public FacilityEntity InsertSchedule(FacilityEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spScheduleInsert",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@Day", SqlDbType.Int);
                    cmd.Parameters.Add("@TimeIn", SqlDbType.VarChar);
                    cmd.Parameters.Add("@TimeOut", SqlDbType.VarChar);
                    cmd.Parameters.Add("@FacilityID", SqlDbType.Int);
                    cmd.Parameters.Add("@SubjectCode", SqlDbType.VarChar);
                    cmd.Parameters.Add("@CourseName", SqlDbType.VarChar);


                    cmd.Parameters["@Day"].Value = e.ScheduleDay;
                    cmd.Parameters["@TimeIn"].Value = e.TimeIn;
                    cmd.Parameters["@TimeOut"].Value = e.TimeOut;
                    cmd.Parameters["@FacilityID"].Value = e.FacilityID;
                    cmd.Parameters["@SubjectCode"].Value = e.SubjectCode;
                    cmd.Parameters["@CourseName"].Value = e.CourseName;
                    conn.Open();
                    e.ScheduleID = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            return e;
        }
        public FacilityEntity DeleteSchedule(FacilityEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spScheduleDelete",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@ScheduleID", SqlDbType.Int);


                    cmd.Parameters["@ScheduleID"].Value = e.ScheduleID;
                    conn.Open();
                    e.IsSuccess = Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }
            return e;
        }
        public IEnumerable<FacilityListEntity> GetScheduleList(FacilityListEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spScheduleListGetFiltered",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@RowStart", SqlDbType.Int);
                    cmd.Parameters.Add("@NoOfRecord", SqlDbType.Int);
                    cmd.Parameters.Add("@SortColumn", SqlDbType.VarChar);
                    cmd.Parameters.Add("@SortDirection", SqlDbType.VarChar);

                    cmd.Parameters.Add("@FacilityID", SqlDbType.Int);

                    cmd.Parameters["@RowStart"].Value = e.RowStart;
                    cmd.Parameters["@NoOfRecord"].Value = e.NoOfRecord;

                    string sortColumn = "";

                    switch (e.SortColumn)
                    {
                        case 1:
                            sortColumn = "Day";
                            break;
                        case 4:
                            sortColumn = "Faculty";
                            break;
                        case 5:
                            sortColumn = "Reserved";
                            break;
                    }

                    cmd.Parameters["@SortColumn"].Value = sortColumn;
                    cmd.Parameters["@SortDirection"].Value = e.SortDirection;

                    cmd.Parameters["@FacilityID"].Value = e.FacilityID;


                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        int totalNoOfRecords = 0;

                        while (reader.Read())
                        {
                            if (totalNoOfRecords == 0)
                                totalNoOfRecords = Convert.ToInt32(reader["TotalNoOfRecord"]);

                            yield return new FacilityListEntity()
                            {
                                TotalRecord = totalNoOfRecords,
                                ScheduleID = Convert.ToInt32(reader["ScheduleID"]),
                                ScheduleDay = Convert.ToInt32(reader["Day"]),
                                TimeIn = reader["TimeIn"].ToString(),
                                TimeOut = reader["TimeOut"].ToString(),
                                SubjectCode = reader["SubjectCode"].ToString(),
                                CourseName = reader["CourseName"].ToString(),
                                ReservedStatus = Convert.ToBoolean(reader["ReservedStatus"]),
                            };
                        }
                    }
                }
            }
        }
    }
}

