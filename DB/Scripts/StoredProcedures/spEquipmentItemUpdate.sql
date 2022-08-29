DROP PROCEDURE IF EXISTS spEquipmentItemUpdate
-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE spEquipmentItemUpdate
	@EquipmentItemID INT
	,@isActive BIT
	,@itemBrand VARCHAR(50) = ''
	,@itemModel VARCHAR(50) = ''
	,@itemSerialNumber VARCHAR(50) = ''
	,@WarrantyStatus BIT
	,@isUsable BIT
	,@Status VARCHAR(50) = ''
	,@Comments VARCHAR(50) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY
		DECLARE @EquipmentCategoryID int
		SELECT @EquipmentCategoryID = EquipmentCategoryID
			FROM EquipmentItem
			WHERE EquipmentItemID = @EquipmentItemID
		UPDATE EquipmentItem
		SET isActive = @isActive
			, ItemBrand = @itemBrand
			, ItemModel = @itemModel
			, ItemSerialNumber = @itemSerialNumber
			, WarrantyStatus = @WarrantyStatus
			, isUsable = @isUsable
			, Status = @Status
			, Comments = @Comments
			, ModifiedDate = GETDATE()
		WHERE EquipmentItemID = @EquipmentItemID
		EXEC spEquipmentCategoryCountUpdate
			@EquipmentCategoryID
		SELECT 1
	END TRY
	BEGIN CATCH
		SELECT 0
	END CATCH
END
GO
