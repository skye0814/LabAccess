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
    public class StudentRegistrationDAL :IBaseDAL<StudentEntity,int>
    {

        // Delete method for StudentsArchive and SystemUserArchive
        public bool Delete(StudentEntity e)
        {
            bool isDeleted = false;
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spStudentDeleteFromArchive",
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

        public bool MoveToArchive(int SystemUserID)
        {
            bool isMoved = false;
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spStudentMoveToArchive",
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

        public StudentEntity GetStudentExistingCheck(StudentEntity e)
        {
            StudentEntity result = null;

            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spStudentAccountExistingCheck",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@EmailAddress", SqlDbType.VarChar);
                    cmd.Parameters.Add("@ResetPasswordCode", SqlDbType.VarChar);

                    cmd.Parameters["@EmailAddress"].Value = e.EmailAddress;
                    cmd.Parameters["@ResetPasswordCode"].Value = e.ResetPasswordCode;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new StudentEntity();
                            result.EmailAddress = reader["EmailAddress"].ToString();
                            result.SystemUserID = Convert.ToInt32(reader["SystemUserID"].ToString());
                            result.Password = reader["Password"].ToString();
                            result.UserName = reader["Username"].ToString();
                            result.ResetPasswordCode = reader["ResetPasswordCode"].ToString();
                        }
                    }
                }
            }
            return result;
        }

        public StudentEntity UpdateStudentAccountSetResetPasswordCodeByEmailAddress(StudentEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spStudentAccountUpdateResetPasswordCode",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@ResetPasswordCode", SqlDbType.VarChar);
                    cmd.Parameters.Add("@EmailAddress", SqlDbType.VarChar);

                    cmd.Parameters["@ResetPasswordCode"].Value = e.ResetPasswordCode;
                    cmd.Parameters["@EmailAddress"].Value = e.EmailAddress;

                    conn.Open();
                    e.IsSuccess = Convert.ToBoolean(cmd.ExecuteScalar());
                }
                return e;
            }
        }

        public StudentEntity Get(StudentEntity e)
        {
            StudentEntity result = null;

            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                if (e.isArchive == false)
                {
                    using (SqlCommand cmd = new SqlCommand()
                    {
                        Connection = conn,
                        CommandTimeout = DBConnection.GetConnectionTimeOut(),
                        CommandText = "spStudentGet",
                        CommandType = CommandType.StoredProcedure
                    })
                    {
                        cmd.Parameters.Add("@ID", SqlDbType.BigInt);
                        cmd.Parameters.Add("@StudentNumber", SqlDbType.VarChar);
                        cmd.Parameters.Add("@EmailAddress", SqlDbType.VarChar);
                        cmd.Parameters.Add("@Username", SqlDbType.VarChar);

                        if (e.Mode == "check")
                            cmd.Parameters["@Id"].Value = 0;
                        else
                            cmd.Parameters["@Id"].Value = e.ID;
                        cmd.Parameters["@StudentNumber"].Value = e.StudentNumber;
                        cmd.Parameters["@EmailAddress"].Value = e.EmailAddress;
                        cmd.Parameters["@Username"].Value = e.UserName;

                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                result = new StudentEntity();
                                result.ID = Convert.ToInt32(reader["Id"]);
                                result.StudentNumber = reader["StudentNumber"].ToString();
                                result.FirstName = reader["FirstName"].ToString();
                                result.MiddleName = reader["MiddleName"].ToString();
                                result.LastName = reader["LastName"].ToString();
                                result.CourseID = Convert.ToInt32(reader["CourseID"]);
                                result.SectionID = Convert.ToInt32(reader["SectionID"]);
                                result.YearID = Convert.ToInt32(reader["YearID"]);
                                result.EmailAddress = reader["EmailAddress"].ToString();
                                result.UserName = reader["UserName"].ToString();
                                result.SystemUserID = Convert.ToInt32(reader["SystemUserID"]);
                            }
                            if (e.ID != 0)
                            {
                                if (result != null)
                                {
                                    if (e.UserName == result.UserName && e.EmailAddress == result.EmailAddress && e.StudentNumber == result.StudentNumber)
                                        result = null;
                                }
                                else
                                {
                                    result = new StudentEntity();
                                    result.Status = "recheck";
                                }

                            }
                            if (e.Mode == "check")
                            {
                                if (result != null && result.ID != 0)
                                {
                                    if (e.ID == result.ID)
                                    {
                                        result = new StudentEntity();
                                    }
                                    else
                                    {
                                        if (e.UserName == result.UserName)
                                            result = null;
                                        else if (e.EmailAddress == result.EmailAddress)
                                            result = null;
                                        else if (e.StudentNumber == result.StudentNumber)
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
                        CommandText = "spStudentsArchiveGet",
                        CommandType = CommandType.StoredProcedure
                    })
                    {
                        cmd.Parameters.Add("@ID", SqlDbType.BigInt);
                        cmd.Parameters.Add("@StudentNumber", SqlDbType.VarChar);
                        cmd.Parameters.Add("@EmailAddress", SqlDbType.VarChar);
                        cmd.Parameters.Add("@Username", SqlDbType.VarChar);

                        if (e.Mode == "check")
                            cmd.Parameters["@Id"].Value = 0;
                        else
                            cmd.Parameters["@Id"].Value = e.ID;
                        cmd.Parameters["@StudentNumber"].Value = e.StudentNumber;
                        cmd.Parameters["@EmailAddress"].Value = e.EmailAddress;
                        cmd.Parameters["@Username"].Value = e.UserName;

                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                result = new StudentEntity();
                                result.ID = Convert.ToInt32(reader["Id"]);
                                result.StudentNumber = reader["StudentNumber"].ToString();
                                result.FirstName = reader["FirstName"].ToString();
                                result.MiddleName = reader["MiddleName"].ToString();
                                result.LastName = reader["LastName"].ToString();
                                result.CourseID = Convert.ToInt32(reader["CourseID"]);
                                result.SectionID = Convert.ToInt32(reader["SectionID"]);
                                result.YearID = Convert.ToInt32(reader["YearID"]);
                                result.EmailAddress = reader["EmailAddress"].ToString();
                                result.UserName = reader["UserName"].ToString();
                                result.SystemUserID = Convert.ToInt32(reader["SystemUserID"]);
                            }
                            if (e.ID != 0)
                            {
                                if (result != null)
                                {
                                    if (e.UserName == result.UserName && e.EmailAddress == result.EmailAddress && e.StudentNumber == result.StudentNumber)
                                        result = null;
                                }
                                else
                                {
                                    result = new StudentEntity();
                                    result.Status = "recheck";
                                }

                            }
                            if (e.Mode == "check")
                            {
                                if (result != null && result.ID != 0)
                                {
                                    if (e.ID == result.ID)
                                    {
                                        result = new StudentEntity();
                                    }
                                    else
                                    {
                                        if (e.UserName == result.UserName)
                                            result = null;
                                        else if (e.EmailAddress == result.EmailAddress)
                                            result = null;
                                        else if (e.StudentNumber == result.StudentNumber)
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

        public IEnumerable<StudentEntity> GetList()
        {
            throw new NotImplementedException();
        }

        public StudentEntity Insert(StudentEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spStudentInsert",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@StudentNumber", SqlDbType.VarChar);
                    cmd.Parameters.Add("@FirstName", SqlDbType.VarChar);
                    cmd.Parameters.Add("@MiddleName", SqlDbType.VarChar);
                    cmd.Parameters.Add("@LastName", SqlDbType.VarChar);
                    cmd.Parameters.Add("@YearID", SqlDbType.VarChar);
                    cmd.Parameters.Add("@CourseID", SqlDbType.VarChar);
                    cmd.Parameters.Add("@SectionID", SqlDbType.VarChar);
                    cmd.Parameters.Add("@EmailAddress", SqlDbType.VarChar);
                    cmd.Parameters.Add("@UserName", SqlDbType.VarChar);
                    cmd.Parameters.Add("@Password", SqlDbType.VarChar);
                    cmd.Parameters.Add("@CreatedBy", SqlDbType.Int);

                    // Meron lang ito para sa pag restore ng data from archive to orig table
                    // Pag ito ay 0, ibig sabihin nag aadd lang sa StudentRegistrationAdd
                    // Pag may value naman, ibig sabihin nagrerestore sa StudentsArchiveEdit
                    cmd.Parameters.Add("@SystemUserID_Archive", SqlDbType.Int);
                    cmd.Parameters.Add("@ID", SqlDbType.Int);

                    cmd.Parameters["@StudentNumber"].Value = e.StudentNumber;
                    cmd.Parameters["@FirstName"].Value = e.FirstName;
                    cmd.Parameters["@MiddleName"].Value = e.MiddleName;
                    cmd.Parameters["@LastName"].Value = e.LastName;
                    cmd.Parameters["@YearID"].Value = e.YearID;
                    cmd.Parameters["@CourseID"].Value = e.CourseID;
                    cmd.Parameters["@SectionID"].Value = e.SectionID;
                    cmd.Parameters["@EmailAddress"].Value = e.EmailAddress;
                    cmd.Parameters["@UserName"].Value = e.UserName;
                    cmd.Parameters["@Password"].Value = e.Password;
                    cmd.Parameters["@CreatedBy"].Value = e.CreatedBy;

                    // Meron lang ito para sa pag restore ng data from archive to orig table
                    // Pag ito ay 0, ibig sabihin nag aadd lang sa StudentRegistrationAdd
                    // Pag may value naman, ibig sabihin nagrerestore sa StudentsArchiveEdit
                    cmd.Parameters["@SystemUserID_Archive"].Value = e.SystemUserID;
                    cmd.Parameters["@ID"].Value = e.ID;


                    conn.Open();

                    e.ID = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            return e;
        }

        public bool Update(StudentEntity e)
        {
            bool isUpdated = false;
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spStudentUpdate",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@ID", SqlDbType.Int);
                    cmd.Parameters.Add("@StudentNumber", SqlDbType.VarChar);
                    cmd.Parameters.Add("@FirstName", SqlDbType.VarChar);
                    cmd.Parameters.Add("@MiddleName", SqlDbType.VarChar);
                    cmd.Parameters.Add("@LastName", SqlDbType.VarChar);
                    cmd.Parameters.Add("@YearID", SqlDbType.VarChar);
                    cmd.Parameters.Add("@CourseID", SqlDbType.VarChar);
                    cmd.Parameters.Add("@SectionID", SqlDbType.VarChar);
                    cmd.Parameters.Add("@EmailAddress", SqlDbType.VarChar);
                    cmd.Parameters.Add("@ModifiedBy", SqlDbType.Int);
                    cmd.Parameters.Add("@SystemUserID", SqlDbType.Int);

                    cmd.Parameters["@ID"].Value = e.ID;
                    cmd.Parameters["@StudentNumber"].Value = e.StudentNumber;
                    cmd.Parameters["@FirstName"].Value = e.FirstName;
                    cmd.Parameters["@MiddleName"].Value = e.MiddleName;
                    cmd.Parameters["@LastName"].Value = e.LastName;
                    cmd.Parameters["@YearID"].Value = e.YearID;
                    cmd.Parameters["@CourseID"].Value = e.CourseID;
                    cmd.Parameters["@SectionID"].Value = e.SectionID;
                    cmd.Parameters["@EmailAddress"].Value = e.EmailAddress;
                    cmd.Parameters["@ModifiedBy"].Value = e.CreatedBy;
                    cmd.Parameters["@SystemUserID"].Value = e.SystemUserID;

                    conn.Open();

                    isUpdated = Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }

            return isUpdated;
        }

        public IEnumerable<StudentListEntity> GetFiltered(StudentListEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spStudentListGetFiltered",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@RowStart", SqlDbType.Int);
                    cmd.Parameters.Add("@NoOfRecord", SqlDbType.Int);
                    cmd.Parameters.Add("@SortColumn", SqlDbType.VarChar);
                    cmd.Parameters.Add("@SortDirection", SqlDbType.VarChar);
                    cmd.Parameters.Add("@YearID", SqlDbType.Int);
                    cmd.Parameters.Add("@SectionID", SqlDbType.Int);

                    cmd.Parameters.Add("@StudentNumber", SqlDbType.VarChar);
                    cmd.Parameters.Add("@FirstName", SqlDbType.VarChar);
                    cmd.Parameters.Add("@LastName", SqlDbType.VarChar);

                    cmd.Parameters["@YearID"].Value = Convert.ToInt32(e.Year);
                    cmd.Parameters["@SectionID"].Value = Convert.ToInt32(e.Section);
                    cmd.Parameters["@RowStart"].Value = e.RowStart;
                    cmd.Parameters["@NoOfRecord"].Value = e.NoOfRecord;

                    string sortColumn = "";

                    switch (e.SortColumn)
                    {
                        case 1:
                            sortColumn = "StudentNumber";
                            break;
                        case 2:
                            sortColumn = "FirstName";
                            break;
                        case 3:
                            sortColumn = "LastName";
                            break;

                    }

                    cmd.Parameters["@SortColumn"].Value = sortColumn;
                    cmd.Parameters["@SortDirection"].Value = e.SortDirection;

                    cmd.Parameters["@StudentNumber"].Value = e.StudentNumber;
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

                            yield return new StudentListEntity()
                            {
                                TotalRecord = totalNoOfRecords,
                                ID = Convert.ToInt32(reader["ID"]),
                                StudentNumber = reader["StudentNumber"].ToString(),
                                FirstName = reader["FirstName"].ToString(),
                                MiddleName = reader["MiddleName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Course = reader["Course"].ToString(),
                                Section = reader["Section"].ToString(),
                                Year = reader["Year"].ToString(),
                                EmailAddress = reader["EmailAddress"].ToString(),
                                UserName = reader["UserName"].ToString(),
                                SystemUserID = Convert.ToInt32(reader["SystemUserID"])
                            };
                        }
                    }
                }
            }
        }

        public IEnumerable<StudentListEntity> ArchiveGetFiltered(StudentListEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spStudentArchiveListGetFiltered",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@RowStart", SqlDbType.Int);
                    cmd.Parameters.Add("@NoOfRecord", SqlDbType.Int);
                    cmd.Parameters.Add("@SortColumn", SqlDbType.VarChar);
                    cmd.Parameters.Add("@SortDirection", SqlDbType.VarChar);
                    cmd.Parameters.Add("@YearID", SqlDbType.Int);
                    cmd.Parameters.Add("@SectionID", SqlDbType.Int);

                    cmd.Parameters.Add("@StudentNumber", SqlDbType.VarChar);
                    cmd.Parameters.Add("@FirstName", SqlDbType.VarChar);
                    cmd.Parameters.Add("@LastName", SqlDbType.VarChar);

                    cmd.Parameters["@YearID"].Value = Convert.ToInt32(e.Year);
                    cmd.Parameters["@SectionID"].Value = Convert.ToInt32(e.Section);
                    cmd.Parameters["@RowStart"].Value = e.RowStart;
                    cmd.Parameters["@NoOfRecord"].Value = e.NoOfRecord;

                    string sortColumn = "";

                    switch (e.SortColumn)
                    {
                        case 1:
                            sortColumn = "StudentNumber";
                            break;
                        case 2:
                            sortColumn = "FirstName";
                            break;
                        case 3:
                            sortColumn = "LastName";
                            break;

                    }

                    cmd.Parameters["@SortColumn"].Value = sortColumn;
                    cmd.Parameters["@SortDirection"].Value = e.SortDirection;

                    cmd.Parameters["@StudentNumber"].Value = e.StudentNumber;
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

                            yield return new StudentListEntity()
                            {
                                TotalRecord = totalNoOfRecords,
                                ID = Convert.ToInt32(reader["ID"]),
                                StudentNumber = reader["StudentNumber"].ToString(),
                                FirstName = reader["FirstName"].ToString(),
                                MiddleName = reader["MiddleName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Course = reader["Course"].ToString(),
                                Section = reader["Section"].ToString(),
                                Year = reader["Year"].ToString(),
                                EmailAddress = reader["EmailAddress"].ToString(),
                                UserName = reader["UserName"].ToString(),
                                SystemUserID = Convert.ToInt32(reader["SystemUserID"])
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
                    CommandText = "spStudentDelete",
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

        public List<string> ValidateStudentRegistration(DataTable dtStudent)
        {
            List<string> errorMessages = new List<string>();

            using(SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spStudentUploadValidation",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@tblStudent", SqlDbType.Structured);
                    cmd.Parameters["@tblStudent"].Value = dtStudent;

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            errorMessages.Add(reader["message"].ToString());
                        }
                    }
                }
            }
            return errorMessages;
        }

        public StudentEntity UploadStudent(DataTable dtStudent, int UserID = 0)
        {
            int Count = 0;
            StudentEntity e = new StudentEntity();
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spStudentUpload",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@tblStudent", SqlDbType.Structured);
                    cmd.Parameters.Add("@UserID", SqlDbType.Int);
                    cmd.Parameters["@tblStudent"].Value = dtStudent;
                    cmd.Parameters["@UserID"].Value = UserID;
                    conn.Open();

                   Count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (Count > 0)
                        e.IsSuccess = true;
                }
            }

            return e;
        }
    }
}
