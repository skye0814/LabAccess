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
CREATE PROCEDURE [dbo].[spGetAllActiveEquipmentRequestPenaltyBySystemUserID]
	@SystemUserID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT B.RequestGUID
		,A.RequestType
		,A.PenaltyDetails
	FROM Penalty A
	INNER JOIN Requests B ON A.RequestID = B.RequestID
	WHERE (isActive = 1) AND (RequestType = 'Equipment') AND (A.RequestorID = @SystemUserID)

END
