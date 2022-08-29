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
    public class EquipmentDAL : IBaseDAL<EquipmentEntity, int>
    {
        public bool Delete(EquipmentEntity e)
        {
            throw new NotImplementedException();
        }

        public EquipmentEntity Get(EquipmentEntity e)
        {
            EquipmentEntity result = null;

            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spEquipmentGet",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@EquipmentID", SqlDbType.BigInt);

                    cmd.Parameters["@EquipmentID"].Value = e.EquipmentID;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())

                            result = new EquipmentEntity();
                        result.EquipmentID = Convert.ToInt32(reader["EquipmentID"]);
                        result.EquipmentCode = reader["EquipmentCode"].ToString();
                        result.Description = reader["Description"].ToString();
                        result.QuantityTotal = Convert.ToInt32(reader["QuantityTotal"]);
                        result.QuantityActive = Convert.ToInt32(reader["QuantityActive"]);
                        result.isActive = Convert.ToBoolean(reader["isActive"]);
                        result.CreatedBy = Convert.ToInt32(reader["QuantityActive"]);
                        result.ModifiedBy = Convert.ToInt32(reader["ModifiedBy"]);
                        result.Comments = reader["Comments"].ToString();
                    }
                }
            }
            return result;
        }

        public IEnumerable<EquipmentEntity> GetList()
        {
            throw new NotImplementedException();
        }

        public EquipmentEntity Insert(EquipmentEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spEquipmentInsert",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@EquipmentCode", SqlDbType.VarChar);
                    cmd.Parameters.Add("@Description", SqlDbType.VarChar);
                    cmd.Parameters.Add("@QuantityTotal", SqlDbType.Int);
                    cmd.Parameters.Add("@QuantityActive", SqlDbType.Int);
                    cmd.Parameters.Add("@isActive", SqlDbType.Bit);
                    cmd.Parameters.Add("@CreatedBy", SqlDbType.BigInt);
                    cmd.Parameters.Add("@ModifiedBy", SqlDbType.BigInt);
                    cmd.Parameters.Add("@Comments", SqlDbType.VarChar);


                    cmd.Parameters["@EquipmentCode"].Value = e.EquipmentCode;
                    cmd.Parameters["@Description"].Value = e.Description;
                    cmd.Parameters["@QuantityTotal"].Value = e.QuantityTotal;
                    cmd.Parameters["@QuantityActive"].Value = e.QuantityTotal; // Assumed to be working when registered
                    cmd.Parameters["@isActive"].Value = 1; // Assumed to be working when registered
                    cmd.Parameters["@CreatedBy"].Value = e.CreatedBy;
                    cmd.Parameters["@ModifiedBy"].Value = e.ModifiedBy;
                    cmd.Parameters["@Comments"].Value = e.Comments;

                    conn.Open();
                    e.EquipmentID = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            return e;
        }

        public bool Update(EquipmentEntity e)
        {
            bool isUpdated = false;
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spEquipmentUpdate",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@EquipmentID", SqlDbType.Int);
                    cmd.Parameters.Add("@EquipmentCode", SqlDbType.VarChar);
                    cmd.Parameters.Add("@Description", SqlDbType.VarChar);
                    cmd.Parameters.Add("@QuantityTotal", SqlDbType.Int);
                    cmd.Parameters.Add("@QuantityActive", SqlDbType.Int);
                    cmd.Parameters.Add("@isActive", SqlDbType.Bit);
                    cmd.Parameters.Add("@ModifiedBy", SqlDbType.Int);
                    cmd.Parameters.Add("@CreatedBy", SqlDbType.Int);
                    cmd.Parameters.Add("@Comments", SqlDbType.VarChar);

                    cmd.Parameters["@EquipmentID"].Value = e.EquipmentID;
                    cmd.Parameters["@EquipmentCode"].Value = e.EquipmentCode;
                    cmd.Parameters["@Description"].Value = e.Description;
                    cmd.Parameters["@QuantityTotal"].Value = e.QuantityTotal; // Assumed to be working when registered
                    cmd.Parameters["@QuantityActive"].Value = e.QuantityTotal; // Assumed to be working when registered
                    cmd.Parameters["@isActive"].Value = e.isActive;
                    cmd.Parameters["@ModifiedBy"].Value = e.ModifiedBy;
                    cmd.Parameters["@CreatedBy"].Value = e.CreatedBy;
                    cmd.Parameters["@Comments"].Value = e.Comments;
                    conn.Open();

                    isUpdated = Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }

            return isUpdated;
        }
        public IEnumerable<EquipmentListEntity> GetFiltered(EquipmentListEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spEquipmentListGetFiltered",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@RowStart", SqlDbType.Int);
                    cmd.Parameters.Add("@NoOfRecord", SqlDbType.Int);
                    cmd.Parameters.Add("@SortColumn", SqlDbType.VarChar);
                    cmd.Parameters.Add("@SortDirection", SqlDbType.VarChar);

                    cmd.Parameters.Add("@EquipmentCode", SqlDbType.VarChar);
                    cmd.Parameters.Add("@Description", SqlDbType.VarChar);
                    cmd.Parameters["@RowStart"].Value = e.RowStart;
                    cmd.Parameters["@NoOfRecord"].Value = e.NoOfRecord;

                    string sortColumn = "";

                    switch (e.SortColumn)
                    {
                        case 1:
                            sortColumn = "EquipmentCode";
                            break;
                        case 2:
                            sortColumn = "Description";
                            break;
                        case 3:
                            sortColumn = "QuantityTotal";
                            break;
                        case 4:
                            sortColumn = "QuantityActive";
                            break;
                        case 5:
                            sortColumn = "isActive";
                            break;
                        case 6:
                            sortColumn = "Comments";
                            break;

                    }

                    cmd.Parameters["@SortColumn"].Value = sortColumn;
                    cmd.Parameters["@SortDirection"].Value = e.SortDirection;

                    cmd.Parameters["@EquipmentCode"].Value = e.EquipmentCode;
                    cmd.Parameters["@Description"].Value = e.Description;


                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        int totalNoOfRecords = 0;

                        while (reader.Read())
                        {
                            if (totalNoOfRecords == 0)
                                totalNoOfRecords = Convert.ToInt32(reader["TotalNoOfRecord"]);

                            yield return new EquipmentListEntity()
                            {
                                TotalRecord = totalNoOfRecords,
                                EquipmentID = Convert.ToInt32(reader["EquipmentID"]),
                                EquipmentCode = reader["EquipmentCode"].ToString(),
                                Description = reader["Description"].ToString(),
                                QuantityTotal = int.Parse(reader["QuantityTotal"].ToString()),
                                QuantityActive = int.Parse(reader["QuantityActive"].ToString()),
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
