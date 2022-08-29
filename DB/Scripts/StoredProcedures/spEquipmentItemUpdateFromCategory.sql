DROP PROCEDURE IF EXISTS spEquipmentItemUpdateFromCategory
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
CREATE PROCEDURE spEquipmentItemUpdateFromCategory
	@EquipmentCategoryID int
	,@oldCategoryCode varchar(50)
	,@newCategory varchar(50)
	,@newCategoryCode varchar(50)
	,@ModifiedBy int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY
		DECLARE @totalCount int, @counter int = 1
		SELECT @totalCount = COUNT(*)
			FROM EquipmentItem
			WHERE EquipmentCategoryID = @EquipmentCategoryID
		WHILE (@counter - 1 < @totalCount)
			BEGIN
				UPDATE EquipmentItem
					SET EquipmentItemCode = @newCategoryCode + CAST (@counter as VARCHAR(5))
					, ModifiedBy = @ModifiedBy
				WHERE EquipmentItemCode = @oldCategoryCode + CAST (@counter as VARCHAR(5))
				SET @counter += 1
			END
		SELECT 1
	END TRY
	BEGIN CATCH
		SELECT 0
	END CATCH
END
GO
