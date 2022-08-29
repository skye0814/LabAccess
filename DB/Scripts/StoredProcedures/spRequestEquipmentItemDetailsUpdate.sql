DROP PROCEDURE IF EXISTS spRequestEquipmentItemDetailsUpdate
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
CREATE PROCEDURE spRequestEquipmentItemDetailsUpdate 
	@equipmentCategoryID int
	,@quantity int
	,@requestGUID varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @i int = 0
	WHILE (@i < @quantity)
	BEGIN
		INSERT INTO RequestEquipmentItem (RequestGUID
		,EquipmentCategoryID
		,isClaimed
		,Status)
		VALUES (@requestGUID
		,@equipmentCategoryID
		,0
		,'Unclaimed')
		SET @i += 1
	END
	SELECT SCOPE_IDENTITY()
END
GO
