USE [ERS]
GO
/****** Object:  StoredProcedure [dbo].[spRequestsInsert]    Script Date: 3/5/2022 7:02:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[spRequestDetailsInsert]
	@RequestDetailsID int
	,@EquipmentCategoryID int
	,@Quantity int
	,@RequestGUID varchar(100) = ''
	,@StartTime varchar(50) = ''
	,@EndTime varchar(50) = ''
	,@Status varchar(50) = 'Unclaimed'
	,@RequestorID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT into RequestDetails(
	EquipmentCategoryID
	,Quantity
	,RequestGUID
	,StartTime
	,EndTime
	,Status
	,RequestorID)
	VALUES (
	@EquipmentCategoryID
	,@Quantity
	,@RequestGUID
	,@StartTime
	,@EndTime
	,@Status
	,@RequestorID)

END