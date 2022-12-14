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
ALTER PROCEDURE [dbo].[spRequestDetailsGetByDate]
	@StartTime varchar(50)='',
	@EndTime varchar(50)=''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT RequestDetailsID
		,EquipmentCategoryID
		,Quantity
		,RequestGUID
		,StartTime
		,EndTime
		,Status
	FROM RequestDetails
	WHERE ((@StartTime BETWEEN StartTime AND EndTime) AND (Status = 'Unclaimed' OR Status = 'Claimed'))
		OR ((@EndTime BETWEEN StartTime AND EndTime) AND (Status = 'Unclaimed' OR Status = 'Claimed'))

END
