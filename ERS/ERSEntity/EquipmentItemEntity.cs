using ERSEntity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSEntity
{
    public class EquipmentItemEntity : CommonEntity

    {
		public int EquipmentItemID { get; set; }
		public bool isActive { get; set; }
		public int EquipmentCategoryID { get; set; }
		public string EquipmentItemCode { get; set; }
		public string Category { get; set; }
		public string ItemBrand { get; set; }
		public string ItemModel { get; set; }
		public string ItemSerialNumber { get; set; }
		public string DateBought { get; set; }
		public bool WarrantyStatus { get; set; }
		public bool isUsable { get; set; }
		public string Status { get; set; }
		public int NoOfTimesBorrowed { get; set; }
		public int ModifiedBy { get; set; }
		public string Comments { get; set; }
	}
	public class EquipmentItemListEntity : TableDisplayCommonEntity
    {
		public int EquipmentItemID { get; set; }
		public bool isActive { get; set; }
		public int EquipmentCategoryID { get; set; }
		public string EquipmentItemCode { get; set; }
		public string Category { get; set; }
		public string ItemBrand { get; set; }
		public string ItemModel { get; set; }
		public string ItemSerialNumber { get; set; }
		public string DateBought { get; set; }
		public bool WarrantyStatus { get; set; }
		public bool isUsable { get; set; }
		public string Status { get; set; }
		public int NoOfTimesBorrowed { get; set; }
		public int ModifiedBy { get; set; }
		public string Comments { get; set; }
	}
}
