DROP PROCEDURE IF EXISTS spRequestEquipmentCheckRequestGUID


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
CREATE PROCEDURE spRequestEquipmentCheckRequestGUID
	@RequestGUID varchar(100) = ''
	,@requestEquipmentItemID int = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT A.RequestGUID 
		,A.Requestor
		,A.RequestDateTime
		,B.StartTime
		,B.EndTime
		,B.Status
	FROM Requests A
	INNER JOIN RequestDetails B ON A.RequestGUID = B.RequestGUID
	WHERE (@RequestGUID = '' OR A.RequestGUID = @RequestGUID)
END
GO