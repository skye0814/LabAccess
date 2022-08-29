USE [ERS]
GO
DROP PROCEDURE IF EXISTS spRequestFacilityGetReservedClassSchedulesByDate

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE spRequestFacilityGetReservedClassSchedulesByDate
	@StartTime varchar(50) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT * 
	FROM RequestFacility
	WHERE ((StartTime LIKE '%' + @StartTime + '%') AND (Status = 'Unclaimed' OR Status = 'Claimed') AND NOT (Schedule = 'vacant'))
	
END
GO
