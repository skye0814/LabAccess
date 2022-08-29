DROP PROCEDURE IF EXISTS spFacilityUsageReport
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
CREATE PROCEDURE spFacilityUsageReport
	@DateFrom varchar(50)
	, @DateTo varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	SELECT RoomType
	,RoomNumber
	,RoomDescription
	,NoOfTimesBorrowed =
		(SELECT (COUNT(B.Status))
			FROM RequestFacility B
			WHERE (B.Status = 'Claimed' OR B.Status = 'Completed')
				AND A.FacilityID = B.FacilityID
				AND (@DateFrom = '' OR (CAST(B.ClaimedTime as DATE) >= CAST(@DateFrom as DATE))) 
				AND (@DateTo = '' OR (CAST(B.ClaimedTime as DATE) <= CAST(@DateTo as DATE)) ))
	, @DateFrom as DateFrom
	, @DateTo as DateTo
FROM Facility A
END
GO
