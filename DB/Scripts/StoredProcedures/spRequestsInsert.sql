USE [ERS]
GO
/****** Object:  StoredProcedure [dbo].[spRequestsInsert]    Script Date: 3/4/2022 8:17:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[spRequestsInsert]
	@RequestID int
	,@Requestor varchar(100) = ''
	,@RequestDateTime varchar(100) = ''
	,@RequestGUID varchar(100) = ''
	,@StartTime varchar(50) = ''
	,@EndTime varchar(50) = ''
	,@isApproved int
	,@Remarks varchar(250) = ''
	,@Status varchar(50) = 'Unclaimed'
	,@RequestorID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT into Requests (
	Requestor
	,RequestDateTime
	,RequestGUID
	,StartTime
	,EndTime
	,isApproved
	,Remarks
	,Status
	,RequestorID)
	VALUES (
	@Requestor
	,@RequestDateTime
	,@RequestGUID
	,@StartTime
	,@EndTime
	,@isApproved
	,@Remarks
	,@Status
	,@RequestorID)

END