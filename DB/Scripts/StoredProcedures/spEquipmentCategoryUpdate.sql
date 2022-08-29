
DROP PROCEDURE IF EXISTS spEquipmentCategoryUpdate
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
CREATE PROCEDURE spEquipmentCategoryUpdate 
	@EquipmentCategoryID INT
	,@Category VARCHAR(50) = ''
	,@CategoryCode  VARCHAR(50) = ''
	,@isActive BIT
	,@ModifiedBy INT
	,@Comments  VARCHAR(50) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @oldCategory VARCHAR(50)
		,@oldCategoryCode VARCHAR(50)
		,@oldEquipmentItemCode VARCHAR(50), @newEquipmentItemCode VARCHAR(50)
	BEGIN TRY
		SELECT @oldCategory = [Category]
				,@oldCategoryCode = [CategoryCode]
				FROM EquipmentCategory
				WHERE EquipmentCategoryID = @EquipmentCategoryID
		UPDATE EquipmentCategory
				SET Category = @Category
					, CategoryCode = @CategoryCode
					, ModifiedBy = @ModifiedBy
					, Comments = @Comments
					, isActive = @isActive
					, ModifiedDate = GETDATE()
				WHERE EquipmentCategoryID = @EquipmentCategoryID
		EXEC spEquipmentItemUpdateFromCategory
			@EquipmentCategoryID
			,@oldCategoryCode
			,@Category
			,@CategoryCode
			,@ModifiedBy
		SELECT 1
	END TRY
	BEGIN CATCH
		SELECT 0
	END CATCH
END
GO
