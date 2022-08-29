DROP PROCEDURE IF EXISTS spPenaltyGet


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
CREATE PROCEDURE spPenaltyGet
	@PenaltyID INT = 0
	,@RequestGUID varchar(100) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT Requestor = CONCAT(D.Firstname, ' ', D.LastName)
		,RequestType
		,RequestGUID = ISNULL(RequestGUID, RequestFacilityGUID)
		,PenaltyDetails
		,A.isActive
	FROM Penalty A
		LEFT JOIN Requests B ON A.RequestID = B.RequestID
		LEFT JOIN RequestFacility C ON A.FacilityRequestID = C.FacilityRequestID
		INNER JOIN SystemUser D ON D.ID = A.RequestorID
	WHERE (@PenaltyID = 0 OR PenaltyID = @PenaltyID)
		AND ((@RequestGUID = '' OR B.RequestGUID = @RequestGUID
			OR C.RequestFacilityGUID = @RequestGUID))

END
GO