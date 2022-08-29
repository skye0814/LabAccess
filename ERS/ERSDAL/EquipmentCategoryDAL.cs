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
    public class EquipmentCategoryDAL : IBaseDAL<EquipmentCategoryEntity, int>
    {
        public bool Delete(EquipmentCategoryEntity e)
        {
            throw new NotImplementedException();
        }

        public EquipmentCategoryEntity Get(EquipmentCategoryEntity e)
        {
            EquipmentCategoryEntity result = null;

            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spEquipmentCategoryGet",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@EquipmentCategoryID", SqlDbType.BigInt);
                    cmd.Parameters.Add("@Category", SqlDbType.VarChar);
                    cmd.Parameters.Add("@CategoryCode", SqlDbType.VarChar);

                    if (e.Mode == "check")
                        cmd.Parameters["@EquipmentCategoryID"].Value = 0;
                    else
                        cmd.Parameters["@EquipmentCategoryID"].Value = e.EquipmentCategoryID;
                    cmd.Parameters["@Category"].Value = e.Category;
                    cmd.Parameters["@CategoryCode"].Value = e.CategoryCode;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new EquipmentCategoryEntity();
                            result.EquipmentCategoryID = Convert.ToInt32(reader["EquipmentCategoryID"]);
                            result.Category = reader["Category"].ToString();
                            result.CategoryCode = reader["CategoryCode"].ToString();
                            result.Comments = reader["Comments"].ToString();
                            result.isActive = Convert.ToBoolean(reader["isActive"]);
                        }
                        if (e.EquipmentCategoryID != 0)
                        {
                            if (result != null)
                            {
                                if (e.Category == result.Category && e.CategoryCode == result.CategoryCode)
                                    result = null;
                            }
                            else
                            {
                                result = new EquipmentCategoryEntity();
                                result.Status = "recheck";
                            }

                        }
                        if (e.Mode == "check")
                        {
                            if (result != null && result.EquipmentCategoryID != 0)
                            {
                                if (e.EquipmentCategoryID == result.EquipmentCategoryID)
                                {
                                    result = new EquipmentCategoryEntity();
                                }
                                else
                                {
                                    if (e.Category == result.Category)
                                        result = null;
                                    else if (e.CategoryCode == result.CategoryCode)
                                        result = null;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        public IEnumerable<EquipmentCategoryEntity> GetList()
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spEquipmentCategoryGet",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            yield return new EquipmentCategoryEntity()
                            {
                                EquipmentCategoryID = Convert.ToInt32(reader["EquipmentCategoryID"]),
                                Category = reader["Category"].ToString(),
                                CategoryCode = reader["CategoryCode"].ToString()
                            };
                        }
                    }
                }
            }
        }

        public IEnumerable<EquipmentCategoryEntity> GetListOnlyAvailable()
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spRequestEquipmentGetOnlyAvailable",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            yield return new EquipmentCategoryEntity()
                            {
                                EquipmentCategoryID = Convert.ToInt32(reader["EquipmentCategoryID"]),
                                Category = reader["Category"].ToString(),
                                CategoryCode = reader["CategoryCode"].ToString(),
                                QuantityUsable = Convert.ToInt32(reader["QuantityUsable"])
                            };
                        }
                    }
                }
            }
        }

        public EquipmentCategoryEntity Insert(EquipmentCategoryEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spEquipmentCategoryInsert",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@Category", SqlDbType.VarChar);
                    cmd.Parameters.Add("@CategoryCode", SqlDbType.VarChar);
                    cmd.Parameters.Add("@CreatedBy", SqlDbType.BigInt);
                    cmd.Parameters.Add("@Comments", SqlDbType.VarChar);


                    cmd.Parameters["@Category"].Value = e.Category.ToUpper();
                    cmd.Parameters["@CategoryCode"].Value = e.CategoryCode.ToUpper();
                    cmd.Parameters["@CreatedBy"].Value = e.CreatedBy;
                    cmd.Parameters["@Comments"].Value = e.Comments;

                    conn.Open();
                    e.EquipmentCategoryID = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            return e;
        }

        public bool Update(EquipmentCategoryEntity e)
        {
            bool isUpdated = false;
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spEquipmentCategoryUpdate",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@EquipmentCategoryID", SqlDbType.Int);
                    cmd.Parameters.Add("@Category", SqlDbType.VarChar);
                    cmd.Parameters.Add("@CategoryCode", SqlDbType.VarChar);
                    cmd.Parameters.Add("@isActive", SqlDbType.Bit);
                    cmd.Parameters.Add("@ModifiedBy", SqlDbType.Int);
                    cmd.Parameters.Add("@Comments", SqlDbType.VarChar);

                    cmd.Parameters["@EquipmentCategoryID"].Value = e.EquipmentCategoryID;
                    cmd.Parameters["@Category"].Value = e.Category.ToUpper();
                    cmd.Parameters["@CategoryCode"].Value = e.CategoryCode.ToUpper();
                    cmd.Parameters["@isActive"].Value = e.isActive;
                    cmd.Parameters["@ModifiedBy"].Value = e.ModifiedBy;
                    cmd.Parameters["@Comments"].Value = e.Comments;
                    conn.Open();

                    isUpdated = Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }

            return isUpdated;
        }
        public IEnumerable<EquipmentCategoryListEntity> GetFiltered(EquipmentCategoryListEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spEquipmentCategoryListGetFiltered",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@RowStart", SqlDbType.Int);
                    cmd.Parameters.Add("@NoOfRecord", SqlDbType.Int);
                    cmd.Parameters.Add("@SortColumn", SqlDbType.VarChar);
                    cmd.Parameters.Add("@SortDirection", SqlDbType.VarChar);

                    cmd.Parameters.Add("@Category", SqlDbType.VarChar);
                    cmd.Parameters.Add("@CategoryCode", SqlDbType.VarChar);
                    cmd.Parameters["@RowStart"].Value = e.RowStart;
                    cmd.Parameters["@NoOfRecord"].Value = e.NoOfRecord;

                    string sortColumn = "";

                    switch (e.SortColumn)
                    {
                        case 1:
                            sortColumn = "Category";
                            break;
                        case 2:
                            sortColumn = "CategoryCode";
                            break;
                        case 3:
                            sortColumn = "isActive";
                            break;
                        case 4:
                            sortColumn = "QuantityTotal";
                            break;
                        case 5:
                            sortColumn = "QuantityActive";
                            break;
                        case 6:
                            sortColumn = "Comments";
                            break;

                    }

                    cmd.Parameters["@SortColumn"].Value = sortColumn;
                    cmd.Parameters["@SortDirection"].Value = e.SortDirection;

                    cmd.Parameters["@Category"].Value = e.Category;
                    cmd.Parameters["@CategoryCode"].Value = e.CategoryCode;


                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        int totalNoOfRecords = 0;

                        while (reader.Read())
                        {
                            if (totalNoOfRecords == 0)
                                totalNoOfRecords = Convert.ToInt32(reader["TotalNoOfRecord"]);

                            yield return new EquipmentCategoryListEntity()
                            {
                                TotalRecord = totalNoOfRecords,
                                EquipmentCategoryID = Convert.ToInt32(reader["EquipmentCategoryID"]),
                                Category = reader["Category"].ToString(),
                                CategoryCode = reader["CategoryCode"].ToString(),
                                QuantityTotal = int.Parse(reader["QuantityTotal"].ToString()),
                                QuantityUsable = int.Parse(reader["QuantityUsable"].ToString()),
                                QuantityDefective = int.Parse(reader["QuantityDefective"].ToString()),
                                QuantityMissing = int.Parse(reader["QuantityMissing"].ToString()),
                                NoOfTimesBorrowed = int.Parse(reader["NoOfTimesBorrowed"].ToString()),
                                isActive = Convert.ToBoolean(reader["isActive"]),
                                Comments = reader["Comments"].ToString(),
                            };
                        }
                    }
                }
            }
        }
    }
}