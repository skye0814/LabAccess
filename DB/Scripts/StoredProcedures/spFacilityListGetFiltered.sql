DROP PROCEDURE IF EXISTS [spFacilityListGetFiltered]
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
CREATE PROCEDURE [dbo].[spFacilityListGetFiltered]
	@RowStart INT = 1
	,@NoOfRecord INT = 10
	,@SortColumn VARCHAR(25) = ''
	,@SortDirection VARCHAR(4) = ''
	----------------------Filters
	,@RoomNumber Varchar(50)=''
	,@RoomType Varchar(50)=''
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
					
					CASE WHEN @SortColumn = 'RoomNumber' AND @SortDirection = 'ASC' THEN RoomNumber END ASC,
					CASE WHEN @SortColumn = 'RoomNumber' AND @SortDirection = 'DESC' THEN RoomNumber END DESC,

					CASE WHEN @SortColumn = 'RoomType' AND @SortDirection = 'ASC' THEN RoomType END ASC,
					CASE WHEN @SortColumn = 'RoomType' AND @SortDirection = 'DESC' THEN RoomType END DESC,

					CASE WHEN @SortColumn = 'RoomDescription' AND @SortDirection = 'ASC' THEN RoomDescription END ASC,
					CASE WHEN @SortColumn = 'RoomDescription' AND @SortDirection = 'DESC' THEN RoomDescription END DESC,

					CASE WHEN @SortColumn = 'isActive' AND @SortDirection = 'ASC' THEN isActive END ASC,
					CASE WHEN @SortColumn = 'isActive' AND @SortDirection = 'DESC' THEN isActive END DESC,

					CASE WHEN @SortColumn = 'isAvailable' AND @SortDirection = 'ASC' THEN isAvailable END ASC,
					CASE WHEN @SortColumn = 'isAvailable' AND @SortDirection = 'DESC' THEN isAvailable END DESC,

					CASE WHEN @SortColumn = 'NoOfTimesBooked' AND @SortDirection = 'ASC' THEN NoOfTimesBooked END ASC,
					CASE WHEN @SortColumn = 'NoOfTimesBooked' AND @SortDirection = 'DESC' THEN NoOfTimesBooked END DESC,

					CASE WHEN @SortColumn = 'Comments' AND @SortDirection = 'ASC' THEN Comments END ASC,
					CASE WHEN @SortColumn = 'Comments' AND @SortDirection = 'DESC' THEN Comments END DESC,

					CASE WHEN @SortColumn = 'TimeIn' AND @SortDirection = 'ASC' THEN TimeIn END ASC,
					CASE WHEN @SortColumn = 'TimeIn' AND @SortDirection = 'DESC' THEN TimeIn END DESC,

					CASE WHEN @SortColumn = 'TimeOut' AND @SortDirection = 'ASC' THEN TimeOut END ASC,
					CASE WHEN @SortColumn = 'TimeOut' AND @SortDirection = 'DESC' THEN TimeOut END DESC,

					CASE WHEN @SortColumn = 'NextSchedule' AND @SortDirection = 'ASC' THEN NextSchedule END ASC,
					CASE WHEN @SortColumn = 'NextSchedule' AND @SortDirection = 'DESC' THEN NextSchedule END DESC
				) AS [Row]
				,FacilityID
				,RoomNumber
				,RoomType
				,RoomDescription
				,isActive
				,isAvailable
				,TimeIn
				,TimeOut
				,NextSchedule
				,NoOfTimesBooked
				,Comments
			FROM Facility
			WHERE (@RoomNumber = '' OR RoomNumber  LIKE '%' + @RoomNumber + '%')
				AND (@RoomType = '' OR RoomType  LIKE '%' + @RoomType + '%')
		)
		SELECT
			(SELECT COUNT(FacilityID) FROM Result) AS [TotalNoOfRecord] -- Default Name
		 ,[Row]
		,FacilityID
		,RoomNumber
		,RoomType
		,RoomDescription
		,isActive
		,isAvailable
		,TimeIn
		,TimeOut
		,NextSchedule
		,NoOfTimesBooked
		,Comments
		FROM Result
		WHERE
			Row >= @RowStart		
			AND
			Row <= @NoOfRecord
END
