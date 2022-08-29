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
    public class LabPersonnelDAL : IBaseDAL<LabPersonnelEntity, int>
    {
        // Delete method for LabPersonnelArchive and SystemUserArchive
        public bool Delete(LabPersonnelEntity e)
        {
            bool isDeleted = false;
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spLabPersonnelDeleteFromArchive",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@SystemUserID", SqlDbType.Int);
                    cmd.Parameters["@SystemUserID"].Value = e.SystemUserID;
                    conn.Open();

                    isDeleted = Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }

            return isDeleted;
        }

        public LabPersonnelEntity Get(LabPersonnelEntity e)
        {
            LabPersonnelEntity result = null;

            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                if (e.isArchive == false)
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = conn,
                        CommandTimeout = DBConnection.GetConnectionTimeOut(),
                        CommandText = "spLabPersonnelGet",
                        CommandType = CommandType.StoredProcedure
                    })
                    {
                        cmd.Parameters.Add("@ID", SqlDbType.BigInt);
                        cmd.Parameters.Add("@EmailAddress", SqlDbType.VarChar);
                        cmd.Parameters.Add("@Username", SqlDbType.VarChar);

                        if (e.Mode == "check")
                            cmd.Parameters["@Id"].Value = 0;
                        else
                            cmd.Parameters["@Id"].Value = e.ID;
                        cmd.Parameters["@EmailAddress"].Value = e.EmailAddress;
                        cmd.Parameters["@Username"].Value = e.UserName;

                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                result = new LabPersonnelEntity();
                                result.ID = Convert.ToInt32(reader["Id"]);
                                result.FirstName = reader["FirstName"].ToString();
                                result.MiddleName = reader["MiddleName"].ToString();
                                result.LastName = reader["LastName"].ToString();
                                result.EmailAddress = reader["EmailAddress"].ToString();
                                result.UserName = reader["UserName"].ToString();
                                result.SystemUserID = Convert.ToInt32(reader["SystemUserID"]);
                            }
                            if (e.ID != 0)
                            {
                                if (result != null)
                                {
                                    if (e.UserName == result.UserName && e.EmailAddress == result.EmailAddress)
                                        result = null;
                                }
                                else
                                {
                                    result = new LabPersonnelEntity();
                                    result.Status = "recheck";
                                }

                            }
                            if (e.Mode == "check")
                            {
                                if (result != null && result.ID != 0)
                                {
                                    if (e.ID == result.ID)
                                    {
                                        result = new LabPersonnelEntity();
                                    }
                                    else
                                    {
                                        if (e.UserName == result.UserName)
                                            result = null;
                                        else if (e.EmailAddress == result.EmailAddress)
                                            result = null;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = conn,
                        CommandTimeout = DBConnection.GetConnectionTimeOut(),
                        CommandText = "spLabPersonnelArchiveGet",
                        CommandType = CommandType.StoredProcedure
                    })
                    {
                        cmd.Parameters.Add("@ID", SqlDbType.BigInt);
                        cmd.Parameters.Add("@EmailAddress", SqlDbType.VarChar);
                        cmd.Parameters.Add("@Username", SqlDbType.VarChar);

                        if (e.Mode == "check")
                            cmd.Parameters["@Id"].Value = 0;
                        else
                            cmd.Parameters["@Id"].Value = e.ID;
                        cmd.Parameters["@EmailAddress"].Value = e.EmailAddress;
                        cmd.Parameters["@Username"].Value = e.UserName;

                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                result = new LabPersonnelEntity();
                                result.ID = Convert.ToInt32(reader["Id"]);
                                result.FirstName = reader["FirstName"].ToString();
                                result.MiddleName = reader["MiddleName"].ToString();
                                result.LastName = reader["LastName"].ToString();
                                result.EmailAddress = reader["EmailAddress"].ToString();
                                result.UserName = reader["UserName"].ToString();
                                result.SystemUserID = Convert.ToInt32(reader["SystemUserID"]);
                            }
                            if (e.ID != 0)
                            {
                                if (result != null)
                                {
                                    if (e.UserName == result.UserName && e.EmailAddress == result.EmailAddress)
                                        result = null;
                                }
                                else
                                {
                                    result = new LabPersonnelEntity();
                                    result.Status = "recheck";
                                }

                            }
                            if (e.Mode == "check")
                            {
                                if (result != null && result.ID != 0)
                                {
                                    if (e.ID == result.ID)
                                    {
                                        result = new LabPersonnelEntity();
                                    }
                                    else
                                    {
                                        if (e.UserName == result.UserName)
                                            result = null;
                                        else if (e.EmailAddress == result.EmailAddress)
                                            result = null;
                                    }
                                }
                            }
                        }
                    }
                }
                
            }
            return result;
        }

        public IEnumerable<LabPersonnelEntity> GetList()
        {
            throw new NotImplementedException();
        }

        public LabPersonnelEntity Insert(LabPersonnelEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spLabPersonnelInsert",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    
                    cmd.Parameters.Add("@FirstName", SqlDbType.VarChar);
                    cmd.Parameters.Add("@MiddleName", SqlDbType.VarChar);
                    cmd.Parameters.Add("@LastName", SqlDbType.VarChar);
                    cmd.Parameters.Add("@EmailAddress", SqlDbType.VarChar);
                    cmd.Parameters.Add("@Username", SqlDbType.VarChar);
                    cmd.Parameters.Add("@Password", SqlDbType.VarChar);
                    cmd.Parameters.Add("@CreatedBy", SqlDbType.Int);

                    // Ito yung para sa archive to orig table, ito ay 0 from LabPersonnelRegistrationAdd
                    // Ito ay may value kapag galing sa LabPersonnelArchiveEdit
                    cmd.Parameters.Add("@SystemUserID_Archive", SqlDbType.Int);
                    cmd.Parameters.Add("@ID", SqlDbType.Int);
                    cmd.Parameters.Add("@SystemUserID_Archive_bigint", SqlDbType.BigInt);
                    cmd.Parameters.Add("@ID_bigint", SqlDbType.BigInt);
                    /*
                    cmd.Parameters.Add("@Year", SqlDbType.VarChar);
                    cmd.Parameters.Add("@Course", SqlDbType.VarChar);
                    cmd.Parameters.Add("@EmailAddress", SqlDbType.VarChar);
                    cmd.Parameters.Add("@UserName", SqlDbType.VarChar);
                    */


                    cmd.Parameters["@FirstName"].Value = e.FirstName;
                    cmd.Parameters["@MiddleName"].Value = e.MiddleName;
                    cmd.Parameters["@LastName"].Value = e.LastName;
                    cmd.Parameters["@EmailAddress"].Value = e.EmailAddress;
                    cmd.Parameters["@UserName"].Value = e.UserName;
                    cmd.Parameters["@Password"].Value = e.Password;
                    cmd.Parameters["@CreatedBy"].Value = e.CreatedBy;

                    // Ito yung para sa archive to orig table, ito ay 0 from LabPersonnelRegistrationAdd
                    // Ito ay may value kapag galing sa LabPersonnelArchiveEdit
                    cmd.Parameters["@SystemUserID_Archive"].Value = e.SystemUserID;
                    cmd.Parameters["@ID"].Value = e.ID;
                    cmd.Parameters["@SystemUserID_Archive_bigint"].Value = e.SystemUserID;
                    cmd.Parameters["@ID_bigint"].Value = e.ID;
                    /*
                    cmd.Parameters["@Year"].Value = e.Year;
                    cmd.Parameters["@Course"].Value = e.Course;
                    cmd.Parameters["@EmailAddress"].Value = e.EmailAddress;
                    cmd.Parameters["@UserName"].Value = e.UserName; */

                    conn.Open();

                    e.ID = Convert.ToInt32(cmd.ExecuteScalar());
                    // cmd.ExecuteNonQuery();
                }
            }

            return e;
        }

        public bool Update(LabPersonnelEntity e)
        {
            bool isUpdated = false;
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spLabPersonnelUpdate",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@ID", SqlDbType.Int);
                    cmd.Parameters.Add("@FirstName", SqlDbType.VarChar);
                    cmd.Parameters.Add("@MiddleName", SqlDbType.VarChar);
                    cmd.Parameters.Add("@LastName", SqlDbType.VarChar);
                    cmd.Parameters.Add("@EmailAddress", SqlDbType.VarChar);
                    cmd.Parameters.Add("@ModifiedBy", SqlDbType.Int);
                    cmd.Parameters.Add("@SystemUserID", SqlDbType.Int);

                    cmd.Parameters["@ID"].Value = e.ID;
                    cmd.Parameters["@FirstName"].Value = e.FirstName;
                    cmd.Parameters["@MiddleName"].Value = e.MiddleName;
                    cmd.Parameters["@LastName"].Value = e.LastName;
                    cmd.Parameters["@EmailAddress"].Value = e.EmailAddress;
                    cmd.Parameters["@ModifiedBy"].Value = e.CreatedBy;
                    cmd.Parameters["@SystemUserID"].Value = e.SystemUserID;
                    conn.Open();

                    isUpdated = Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }

            return isUpdated;
        }

        public bool MoveToArchive(int SystemUserID)
        {
            bool isMoved = false;
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spLabPersonnelMoveToArchive",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@SystemUserID", SqlDbType.Int);
                    cmd.Parameters["@SystemUserID"].Value = SystemUserID;
                    conn.Open();

                    isMoved = Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }

            return isMoved;
        }
        public bool BulkDelete(List<int> IDs)
        {
            bool result = false;
            DataTable dtIDs = null;
            dtIDs = new DataTable();
            dtIDs.Columns.Add("ID", typeof(int));
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
                    CommandText = "spLabPersonnelDelete",
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

        public IEnumerable<LabPersonnelListEntity> GetFiltered(LabPersonnelListEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spLabPersonnelListGetFiltered",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@RowStart", SqlDbType.Int);
                    cmd.Parameters.Add("@NoOfRecord", SqlDbType.Int);
                    cmd.Parameters.Add("@SortColumn", SqlDbType.VarChar);
                    cmd.Parameters.Add("@SortDirection", SqlDbType.VarChar);

                    cmd.Parameters.Add("@FirstName", SqlDbType.VarChar);
                    cmd.Parameters.Add("@MiddleName", SqlDbType.VarChar);
                    cmd.Parameters.Add("@LastName", SqlDbType.VarChar);
                    cmd.Parameters["@RowStart"].Value = e.RowStart;
                    cmd.Parameters["@NoOfRecord"].Value = e.NoOfRecord;

                    string sortColumn = "";

                    switch (e.SortColumn)
                    {
                        case 1:
                            sortColumn = "FirstName";
                            break;
                        case 2:
                            sortColumn = "MiddleName";
                            break;
                        case 3:
                            sortColumn = "LastName";
                            break;

                    }

                    cmd.Parameters["@SortColumn"].Value = sortColumn;
                    cmd.Parameters["@SortDirection"].Value = e.SortDirection;

                    cmd.Parameters["@MiddleName"].Value = e.MiddleName;
                    cmd.Parameters["@FirstName"].Value = e.FirstName;
                    cmd.Parameters["@LastName"].Value = e.LastName;

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        int totalNoOfRecords = 0;

                        while (reader.Read())
                        {
                            if (totalNoOfRecords == 0)
                                totalNoOfRecords = Convert.ToInt32(reader["TotalNoOfRecord"]);

                            yield return new LabPersonnelListEntity()
                            {
                                TotalRecord = totalNoOfRecords,
                                ID = Convert.ToInt32(reader["ID"]),
                                FirstName = reader["FirstName"].ToString(),
                                MiddleName = reader["MiddleName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                EmailAddress = reader["EmailAddress"].ToString(),
                                UserName = reader["UserName"].ToString(),
                                SystemUserID = Convert.ToInt32(reader["SystemUserID"])
                            };
                        }
                    }
                }
            }
        }

        public IEnumerable<LabPersonnelListEntity> ArchiveGetFiltered(LabPersonnelListEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spLabPersonnelArchiveListGetFiltered",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@RowStart", SqlDbType.Int);
                    cmd.Parameters.Add("@NoOfRecord", SqlDbType.Int);
                    cmd.Parameters.Add("@SortColumn", SqlDbType.VarChar);
                    cmd.Parameters.Add("@SortDirection", SqlDbType.VarChar);

                    cmd.Parameters.Add("@FirstName", SqlDbType.VarChar);
                    cmd.Parameters.Add("@MiddleName", SqlDbType.VarChar);
                    cmd.Parameters.Add("@LastName", SqlDbType.VarChar);
                    cmd.Parameters["@RowStart"].Value = e.RowStart;
                    cmd.Parameters["@NoOfRecord"].Value = e.NoOfRecord;

                    string sortColumn = "";

                    switch (e.SortColumn)
                    {
                        case 1:
                            sortColumn = "FirstName";
                            break;
                        case 2:
                            sortColumn = "MiddleName";
                            break;
                        case 3:
                            sortColumn = "LastName";
                            break;

                    }

                    cmd.Parameters["@SortColumn"].Value = sortColumn;
                    cmd.Parameters["@SortDirection"].Value = e.SortDirection;

                    cmd.Parameters["@MiddleName"].Value = e.MiddleName;
                    cmd.Parameters["@FirstName"].Value = e.FirstName;
                    cmd.Parameters["@LastName"].Value = e.LastName;

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        int totalNoOfRecords = 0;

                        while (reader.Read())
                        {
                            if (totalNoOfRecords == 0)
                                totalNoOfRecords = Convert.ToInt32(reader["TotalNoOfRecord"]);

                            yield return new LabPersonnelListEntity()
                            {
                                TotalRecord = totalNoOfRecords,
                                ID = Convert.ToInt32(reader["ID"]),
                                FirstName = reader["FirstName"].ToString(),
                                MiddleName = reader["MiddleName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                EmailAddress = reader["EmailAddress"].ToString(),
                                UserName = reader["UserName"].ToString(),
                                SystemUserID = Convert.ToInt32(reader["SystemUserID"])
                            };
                        }
                    }
                }
            }
        }
    }
}
