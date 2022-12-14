DROP PROCEDURE IF EXISTS [spEquipmentItemInsert]
USE [ERS]
GO
/****** Object:  StoredProcedure [dbo].[spEquipmentItemInsert]    Script Date: 1/13/2022 12:15:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spEquipmentItemInsert] 
	@Category varchar(50)
	,@ItemBrand varchar(50)
	,@ItemModel varchar(50) 
	,@ItemSerialNumber varchar(50)
	,@DateBought datetime
	,@WarrantyStatus bit
	,@CreatedBy bigint
	,@ModifiedBy bigint
	,@Comments varchar(250) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @equipmentCategoryID int, @itemNumber int, @categoryCode varchar(50)
	SELECT @equipmentCategoryID = EquipmentCategoryID
		,@itemNumber = QuantityTotal
		,@categoryCode = CategoryCode
		FROM EquipmentCategory
		WHERE Category = @Category
	INSERT into EquipmentItem (EquipmentCategoryID
	,EquipmentItemCode
	,ItemBrand
	,ItemModel
	,ItemSerialNumber
	,DateBought
	,WarrantyStatus
	,isUsable
	,Status
	,isActive
	,NoOfTimesBorrowed
	,CreatedBy
	,CreatedDate
	,ModifiedBy
	,ModifiedDate
	,Comments)
	VALUES (@equipmentCategoryID
	,@categoryCode + CAST(FORMAT(@itemNumber + 1, '0000') as varchar(5))
	,@ItemBrand
	,@ItemModel
	,@ItemSerialNumber
	,@DateBought
	,@WarrantyStatus
	,1
	,'Functioning'
	,1
	,0
	,@CreatedBy
	,GETDATE()
	,@ModifiedBy
	,GETDATE()
	,@Comments)
	
	EXEC spEquipmentCategoryCountUpdate @equipmentCategoryID
		
	SELECT SCOPE_IDENTITY()
END
