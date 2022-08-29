
DROP PROCEDURE IF EXISTS spEquipmentCategoryCountUpdate
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
CREATE PROCEDURE spEquipmentCategoryCountUpdate 
	@EquipmentCategoryID varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @activeCount int = 0, @usableCount int = 0, @defectiveCount int = 0, @missingCount int = 0, @totalNoOfTimesBorrowed int = 0
	SELECT @activeCount = COUNT(isActive)
		FROM EquipmentItem
		WHERE EquipmentCategoryID = @equipmentCategoryID AND isActive = 1
	SELECT @usableCount = COUNT(isUsable)
		FROM EquipmentItem
		WHERE EquipmentCategoryID = @equipmentCategoryID AND isUsable = 1
	SELECT @defectiveCount = COUNT(Status)
		FROM EquipmentItem
		WHERE EquipmentCategoryID = @equipmentCategoryID AND Status = 'Defective'
	SELECT @missingCount = COUNT(Status)
		FROM EquipmentItem
		WHERE EquipmentCategoryID = @equipmentCategoryID AND Status = 'Missing'
	SELECT @totalNoOfTimesBorrowed = SUM(NoOfTimesBorrowed)
		FROM EquipmentItem
		WHERE EquipmentCategoryID = @EquipmentCategoryID
	UPDATE EquipmentCategory
		SET QuantityTotal = @activeCount
			,QuantityUsable = @usableCount
			,QuantityDefective = @defectiveCount
			,QuantityMissing = @missingCount
			,NoOfTimesBorrowed = @totalNoOfTimesBorrowed
		WHERE EquipmentCategoryID = @EquipmentCategoryID
END
GO
