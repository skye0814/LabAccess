USE [ERS]
GO
/****** Object:  StoredProcedure [dbo].[spRequestDetailsGetByRequestGUID]    Script Date: 4/16/2022 10:48:54 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spRequestsGetByRequestGUID]
	@RequestGUID varchar(100)=''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT RequestID
		,Requestor
		,RequestDateTime
		,StartTime
		,EndTime
		,Remarks
		,RequestGUID
		,isApproved
	FROM Requests
	WHERE (RequestGUID = @RequestGUID)

END
