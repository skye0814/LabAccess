DROP PROCEDURE IF EXISTS spStudentUsageReport
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
CREATE PROCEDURE spStudentUsageReport
	@DateFrom varchar(50)
	, @DateTo varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	SELECT YearLevel = YearCode + ' Year'
	,NoOfTimesBorrowed =
		(SELECT (COUNT(B.Status))
			FROM RequestEquipmentItem B
			INNER JOIN Requests C ON B.RequestGUID = C.RequestGUID
			INNER JOIN SystemUser D ON C.RequestorID = D.ID
			LEFT JOIN Students F ON D.ID = F.SystemUserID
			WHERE (B.Status = 'Claimed' OR B.Status = 'Returned')
				AND A.YearID = F.YearID
				AND (@DateFrom = '' OR CAST(C.RequestDateTime as DATE) >= CAST(@DateFrom as DATE)) 
				AND (@DateTo = '' OR CAST(C.RequestDateTime as DATE) <= CAST(@DateTo as DATE) ))
			+
		(SELECT (COUNT(B.Status))
			FROM RequestFacility B
			INNER JOIN SystemUser D ON B.FacilityRequestorID = D.ID
			LEFT JOIN Students F ON D.ID = F.SystemUserID
			WHERE (B.Status = 'Claimed' OR B.Status = 'Completed')
				AND A.YearID = F.YearID
				AND (@DateFrom = '' OR CAST(B.RequestDate as DATE) >= CAST(@DateFrom as DATE)) 
				AND (@DateTo = '' OR CAST(B.RequestDate as DATE) <= CAST(@DateTo as DATE) ))
	, @DateFrom as DateFrom
	, @DateTo as DateTo
FROM Year A
END
GO
