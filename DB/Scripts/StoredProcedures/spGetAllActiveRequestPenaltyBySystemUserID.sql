USE [ERS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[spGetAllActiveRequestPenaltyBySystemUserID]
	@SystemUserID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT ISNULL(RequestGUID,'') + ISNULL(RequestFacilityGUID,'') as RequestGUID
		,RequestType
		,PenaltyDetails
	FROM Penalty A
	LEFT JOIN Requests B ON A.RequestID = B.RequestID 
	LEFT JOIN RequestFacility C ON A.FacilityRequestID = C.FacilityRequestID
	WHERE isActive = 1 AND A.RequestorID = @SystemUserID

END
