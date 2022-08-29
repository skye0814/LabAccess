DROP PROCEDURE IF EXISTS spEquipmentCategoryGet


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
CREATE PROCEDURE spEquipmentCategoryGet
	@EquipmentCategoryID INT = 0
	,@Category VARCHAR(50) = ''
	,@CategoryCode VARCHAR(50) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT EquipmentCategoryID
		,Category
		,CategoryCode
		,Comments
		,isActive
	FROM EquipmentCategory
	WHERE (@EquipmentCategoryID = 0 OR EquipmentCategoryID = @EquipmentCategoryID)
		AND (@Category = '' OR Category = @Category)
		AND (@CategoryCode = '' OR CategoryCode = @CategoryCode)

END
GO