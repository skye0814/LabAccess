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
    public class EquipmentItemDAL : IBaseDAL<EquipmentItemEntity, int>
    {
        public bool Delete(EquipmentItemEntity e)
        {
            throw new NotImplementedException();
        }

        public EquipmentItemEntity Get(EquipmentItemEntity e)
        {
            EquipmentItemEntity result = null;

            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spEquipmentItemGet",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@EquipmentItemID", SqlDbType.BigInt);
                    cmd.Parameters.Add("@ItemSerialNumber", SqlDbType.VarChar);

                    cmd.Parameters["@EquipmentItemID"].Value = e.EquipmentItemID;
                    cmd.Parameters["@ItemSerialNumber"].Value = e.ItemSerialNumber;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new EquipmentItemEntity();
                            result.EquipmentItemID = Convert.ToInt32(reader["EquipmentItemID"]);
                            result.EquipmentItemCode = reader["EquipmentItemCode"].ToString();
                            result.ItemBrand = reader["ItemBrand"].ToString();
                            result.ItemModel = reader["ItemModel"].ToString();
                            result.ItemSerialNumber = reader["ItemSerialNumber"].ToString();
                            result.DateBought = Convert.ToDateTime(reader["DateBought"].ToString()).ToString("MMMM dd, yyyy");
                            result.WarrantyStatus = Convert.ToBoolean(reader["WarrantyStatus"]);
                            result.isUsable = Convert.ToBoolean(reader["isUsable"]);
                            result.isActive = Convert.ToBoolean(reader["isActive"]);
                            result.Status = reader["Status"].ToString();
                            result.Category = reader["Category"].ToString();
                            result.Comments = reader["Comments"].ToString();
                        }
                        if (e.EquipmentItemID !=0)
                        {
                            if (result != null)
                            {
                                if (e.ItemSerialNumber == result.ItemSerialNumber)
                                    result = null;
                            }
                            else
                            {
                                result = new EquipmentItemEntity();
                                result.Status = "recheck";
                            }
                                
                        }
                    }
                }
            }
            return result;
        }

        public IEnumerable<EquipmentItemEntity> GetList()
        {
            throw new NotImplementedException();
        }

        public EquipmentItemEntity Insert(EquipmentItemEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spEquipmentItemInsert",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@Category", SqlDbType.VarChar);
                    cmd.Parameters.Add("@ItemBrand", SqlDbType.VarChar);
                    cmd.Parameters.Add("@ItemModel", SqlDbType.VarChar);
                    cmd.Parameters.Add("@ItemSerialNumber", SqlDbType.VarChar);
                    cmd.Parameters.Add("@DateBought", SqlDbType.Date);
                    cmd.Parameters.Add("@WarrantyStatus", SqlDbType.Bit);
                    cmd.Parameters.Add("@CreatedBy", SqlDbType.Int);
                    cmd.Parameters.Add("@ModifiedBy", SqlDbType.Int);
                    cmd.Parameters.Add("@Comments", SqlDbType.VarChar);

                    cmd.Parameters["@Category"].Value = e.Category;
                    cmd.Parameters["@ItemBrand"].Value = e.ItemBrand;
                    cmd.Parameters["@ItemModel"].Value = e.ItemModel;
                    cmd.Parameters["@ItemSerialNumber"].Value = e.ItemSerialNumber;
                    cmd.Parameters["@DateBought"].Value = Convert.ToDateTime(e.DateBought).ToString("MMMM, dd, yyyy");
                    cmd.Parameters["@WarrantyStatus"].Value = e.WarrantyStatus;
                    cmd.Parameters["@CreatedBy"].Value = e.CreatedBy;
                    cmd.Parameters["@ModifiedBy"].Value = e.ModifiedBy;
                    cmd.Parameters["@Comments"].Value = e.Comments;

                    conn.Open();
                    e.EquipmentItemID = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            return e;
        }

        public bool Update(EquipmentItemEntity e)
        {
            bool isUpdated = false;
            if (e.Status != "Functioning")
            {
                e.isUsable = false;
            }
            else
                e.isUsable = true;
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spEquipmentItemUpdate",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@EquipmentItemID", SqlDbType.Int);
                    cmd.Parameters.Add("@isActive", SqlDbType.Bit);
                    cmd.Parameters.Add("@ItemBrand", SqlDbType.VarChar);
                    cmd.Parameters.Add("@ItemModel", SqlDbType.VarChar);
                    cmd.Parameters.Add("@ItemSerialNumber", SqlDbType.VarChar);
                    cmd.Parameters.Add("@WarrantyStatus", SqlDbType.Bit);
                    cmd.Parameters.Add("@isUsable", SqlDbType.Bit);
                    cmd.Parameters.Add("@Status", SqlDbType.VarChar);
                    cmd.Parameters.Add("@Comments", SqlDbType.VarChar);

                    cmd.Parameters["@EquipmentItemID"].Value = e.EquipmentItemID;
                    cmd.Parameters["@isActive"].Value = e.isActive;
                    cmd.Parameters["@ItemBrand"].Value = e.ItemBrand;
                    cmd.Parameters["@ItemModel"].Value = e.ItemModel;
                    cmd.Parameters["@ItemSerialNumber"].Value = e.ItemSerialNumber;
                    cmd.Parameters["@WarrantyStatus"].Value = e.WarrantyStatus;
                    cmd.Parameters["@isUsable"].Value = e.isUsable;
                    cmd.Parameters["@Status"].Value = e.Status;
                    cmd.Parameters["@Comments"].Value = e.Comments;
                    conn.Open();

                    isUpdated = Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }

            return isUpdated;
        }
        public IEnumerable<EquipmentItemListEntity> GetFiltered(EquipmentItemListEntity e)
        {
            using (SqlConnection conn = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = DBConnection.GetConnectionTimeOut(),
                    CommandText = "spEquipmentItemListGetFiltered",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@RowStart", SqlDbType.Int);
                    cmd.Parameters.Add("@NoOfRecord", SqlDbType.Int);
                    cmd.Parameters.Add("@SortColumn", SqlDbType.VarChar);
                    cmd.Parameters.Add("@SortDirection", SqlDbType.VarChar);

                    cmd.Parameters.Add("@EquipmentCategoryID", SqlDbType.Int);
                    cmd.Parameters.Add("@EquipmentItemCode", SqlDbType.VarChar);
                    cmd.Parameters.Add("@Category", SqlDbType.VarChar);
                    cmd.Parameters["@RowStart"].Value = e.RowStart;
                    cmd.Parameters["@NoOfRecord"].Value = e.NoOfRecord;

                    string sortColumn = "";

                    switch (e.SortColumn)
                    {
                        case 1:
                            sortColumn = "isActive";
                            break;
                        case 2:
                            sortColumn = "Category";
                            break;
                        case 3:
                            sortColumn = "EquipmentItemCode";
                            break;
                        case 4:
                            sortColumn = "ItemBrand";
                            break;
                        case 5:
                            sortColumn = "ItemModel";
                            break;
                        case 6:
                            sortColumn = "ItemSerialNumber";
                            break;
                        case 7:
                            sortColumn = "DateBought";
                            break;
                        case 8:
                            sortColumn = "WarrantyStatus";
                            break;
                        case 9:
                            sortColumn = "isUsable";
                            break;
                        case 10:
                            sortColumn = "Status";
                            break;
                        case 11:
                            sortColumn = "NoOfTimesBorrwed";
                            break;
                        case 12:
                            sortColumn = "Comments";
                            break;

                    }

                    cmd.Parameters["@SortColumn"].Value = sortColumn;
                    cmd.Parameters["@SortDirection"].Value = e.SortDirection;

                    cmd.Parameters["@EquipmentCategoryID"].Value = e.EquipmentCategoryID;
                    cmd.Parameters["@EquipmentItemCode"].Value = e.EquipmentItemCode;
                    cmd.Parameters["@Category"].Value = e.Category;

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        int totalNoOfRecords = 0;

                        while (reader.Read())
                        {
                            if (totalNoOfRecords == 0)
                                totalNoOfRecords = Convert.ToInt32(reader["TotalNoOfRecord"]);

                            yield return new EquipmentItemListEntity()
                            {
                                TotalRecord = totalNoOfRecords,
                                isActive = Convert.ToBoolean(reader["isActive"]),
                                EquipmentCategoryID = Convert.ToInt32(reader["EquipmentCategoryID"]),
                                EquipmentItemID = Convert.ToInt32(reader["EquipmentItemID"]),
                                EquipmentItemCode = reader["EquipmentItemCode"].ToString(),
                                Category = reader["Category"].ToString(),
                                ItemBrand = reader["ItemBrand"].ToString(),
                                ItemModel = reader["ItemModel"].ToString(),
                                ItemSerialNumber = reader["ItemSerialNumber"].ToString(),
                                DateBought = Convert.ToDateTime(reader["DateBought"].ToString()).ToString("yyyy, MMMM dd"),
                                WarrantyStatus = Convert.ToBoolean(reader["WarrantyStatus"]),
                                isUsable = Convert.ToBoolean(reader["isUsable"]),
                                Status = reader["Status"].ToString(),
                                NoOfTimesBorrowed = Convert.ToInt32(reader["NoOfTimesBorrowed"]),
                                Comments = reader["Comments"].ToString(),
                            };
                        }
                    }
                }
            }
        }
    }

    internal class EquipmentItemBLL
    {
    }
}
