DROP PROCEDURE IF EXISTS spRequestFacilityGet


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
CREATE PROCEDURE spRequestFacilityGet
	@FacilityRequestID INT = 0
	,@RequestFacilityGUID VARCHAR(100) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT FacilityRequestID
		,FacilityRequestor
		,A.FacilityID
		,RequestDate
		,StartTime
		,EndTime
		,Status
		,Schedule
		,Remarks
		,B.RoomNumber
		,RequestFacilityGUID
		,ClaimedTime = ISNULL(ClaimedTime,0)
		,ReturnedTime = ISNULL(ReturnedTime,0)
	FROM RequestFacility A
		INNER JOIN Facility B ON A.FacilityID = B.FacilityID
	WHERE (@FacilityRequestID = 0 OR FacilityRequestID = @FacilityRequestID)
		AND (@RequestFacilityGUID = '' OR RequestFacilityGUID = @RequestFacilityGUID)
END
GO