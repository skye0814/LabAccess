USE [ERS]
GO
/****** Object:  StoredProcedure [dbo].[spRequestDetailsGetByRequestGUID]    Script Date: 4/23/2022 9:59:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spRequestFacilityGetByStatusAndSystemUserID]
	@DateToday datetime,
	@Status varchar(50),
	@SystemUserID int
AS
BEGIN
	
	IF (@Status = 'Unclaimed')
	BEGIN
		SET NOCOUNT ON;
		SELECT FacilityRequestorID
		,StartTime
		,EndTime
		,Status
		,A.FacilityID
		,B.RoomNumber
		FROM RequestFacility A
		INNER JOIN Facility B ON A.FacilityID = B.FacilityID 
		WHERE (FacilityRequestorID = @SystemUserID AND Status = 'Unclaimed' AND (@DateToday BETWEEN StartTime AND EndTime))
	END
	ELSE IF (@Status = 'Claimed')
	BEGIN
		SET NOCOUNT ON;
		SELECT FacilityRequestorID
		,StartTime
		,EndTime
		,Status
		,A.FacilityID
		,B.RoomNumber
		FROM RequestFacility A
		INNER JOIN Facility B ON A.FacilityID = B.FacilityID 
		WHERE (FacilityRequestorID = @SystemUserID AND Status = 'Claimed' AND (@DateToday BETWEEN StartTime AND EndTime))
	END
END
