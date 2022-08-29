USE [ERS]


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
ALTER PROCEDURE spRequestFacilityGetByRequestDate
	@StartTime Varchar(50) = '',
	@EndTime Varchar(50) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT FacilityRequestID
		,FacilityRequestor
		,FacilityID
		,RequestDate
		,StartTime
		,EndTime
		,Status
		,Remarks
	FROM RequestFacility
	WHERE ((@StartTime BETWEEN StartTime AND EndTime) AND (Status = 'Unclaimed' OR Status = 'Claimed'))
		OR ((@EndTime BETWEEN StartTime AND EndTime) AND (Status = 'Unclaimed' OR Status = 'Claimed'))
END
GO