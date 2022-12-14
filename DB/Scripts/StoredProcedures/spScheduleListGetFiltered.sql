DROP PROCEDURE IF EXISTS spScheduleListGetFiltered
USE [ERS]
GO
/****** Object:  StoredProcedure [dbo].[spEquipmentListGetFiltered]    Script Date: 1/15/2022 6:06:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:	
-- Create date:
-- Description:
-- Modified Date: 
-- =============================================
CREATE PROCEDURE spScheduleListGetFiltered
	@RowStart INT = 1
	,@NoOfRecord INT = 10
	,@SortColumn VARCHAR(25) = ''
	,@SortDirection VARCHAR(4) = ''
	----------------------Filters
	,@FacilityID INT = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		IF @SortDirection = ''
			BEGIN
				SET @SortDirection = 'ASC'
			END
				
		SET @NoOfRecord = @NoOfRecord + @RowStart - 1
		
		SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
		WITH Result
		AS
		(
			SELECT
				ROW_NUMBER()
				OVER(
					ORDER BY 
					CASE WHEN @SortColumn = '' AND @SortDirection = 'ASC' THEN [FacilityID] END ASC,
					CASE WHEN @SortColumn = '' AND @SortDirection = 'DESC' THEN [FacilityID] END DESC,
					
					CASE WHEN @SortColumn = 'Day' AND @SortDirection = 'ASC' THEN Day END ASC,
					CASE WHEN @SortColumn = 'Day' AND @SortDirection = 'DESC' THEN Day END DESC,

					CASE WHEN @SortColumn = 'ReservedStatus' AND @SortDirection = 'ASC' THEN ReservedStatus END ASC,
					CASE WHEN @SortColumn = 'ReservedStatus' AND @SortDirection = 'DESC' THEN ReservedStatus END DESC
				) AS [Row]
				,ScheduleID
				,Day
				,TimeIn
				,TimeOut
				,SubjectCode
				,CourseName
				,ReservedStatus
			FROM Schedule
			WHERE (@FacilityID = 0 OR FacilityID = @FacilityID)
		)
		SELECT
		(SELECT COUNT(ScheduleID) FROM Result) AS [TotalNoOfRecord] -- Default Name
		,[Row]
		,ScheduleID
		,Day
		,TimeIn
		,TimeOut
		,SubjectCode
		,CourseName
		,ReservedStatus
		FROM Result
		WHERE
			Row >= @RowStart		
			AND
			Row <= @NoOfRecord
END
