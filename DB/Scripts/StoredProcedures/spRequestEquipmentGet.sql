DROP PROCEDURE IF EXISTS spRequestEquipmentGet


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
CREATE PROCEDURE spRequestEquipmentGet
	@RequestGUID varchar(100) = ''
	,@requestEquipmentItemID int = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT A.RequestGUID 
		,B.Requestor
		,B.RequestDateTime
		,B.StartTime
		,B.EndTime
		,B.Status
	FROM RequestEquipmentItem A
	INNER JOIN Requests B ON A.RequestGUID = B.RequestGUID
	INNER JOIN RequestDetails C ON A.RequestGUID = C.RequestGUID
	WHERE (@RequestGUID = '' OR A.RequestGUID = @RequestGUID)
		AND (@requestEquipmentItemID = 0 OR A.RequestEquipmentItemID = @requestEquipmentItemID)
END
GO