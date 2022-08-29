USE [ERS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE spGetClaimedFacilityByCurrentDate
	@GetDate datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT A.FacilityID
		,B.RoomNumber
		,StartTime
		,EndTime
		,Status
	FROM RequestFacility A
	INNER JOIN Facility B ON A.FacilityID = B.FacilityID
	WHERE ((@GetDate BETWEEN A.StartTime AND A.EndTime) AND (A.Status = 'Claimed'))
END
GO