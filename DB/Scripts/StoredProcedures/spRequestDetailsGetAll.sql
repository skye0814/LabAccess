USE [ERS]
GO
/****** Object:  StoredProcedure [dbo].[spRequestEquipmentGetOnlyAvailable]    Script Date: 4/15/2022 11:50:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spRequestDetailsGetAll]
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
	FROM RequestDetails

END