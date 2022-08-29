DROP PROCEDURE IF EXISTS spEquipmentItemGet
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
CREATE PROCEDURE spEquipmentItemGet
	@EquipmentItemID INT = 0
	,@ItemSerialNumber VARCHAR(50) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT 
		A.EquipmentItemID
		,A.EquipmentItemCode
		,Category
		,A.ItemBrand
		,A.ItemModel
		,A.ItemSerialNumber
		,A.DateBought
		,A.WarrantyStatus
		,A.isUsable
		,A.Status
		,A.isActive
		,A.Comments
	FROM EquipmentItem A
	INNER JOIN EquipmentCategory B ON A.EquipmentCategoryID = B.EquipmentCategoryID
	WHERE (@EquipmentItemID = 0 OR EquipmentItemID = @EquipmentItemID)
		AND (@ItemSerialNumber = '' OR ItemSerialNumber = @ItemSerialNumber)
END
GO
