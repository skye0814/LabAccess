USE [ERS]
GO
/****** Object:  StoredProcedure [dbo].[spRequestFacilityListGetFiltered]    Script Date: 06/03/2022 2:18:13 pm ******/
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
ALTER PROCEDURE [dbo].[spRequestFacilityListGetFiltered]
	@RowStart INT = 1
	,@NoOfRecord INT = 10
	,@SortColumn VARCHAR(25) = ''
	,@SortDirection VARCHAR(4) = ''
	----------------------Filters
	,@FacilityRequestID int = 0
	,@RequestFacilityGUID Varchar(50)=''
	,@RequestDate Varchar(50)=''
	,@StartTime Varchar(50) =''
	,@EndTime Varchar(50) =''
	,@Remarks Varchar(50) = ''
	,@Status Varchar(50) = ''
	,@FacilityRequestor varchar(100) = ''
	,@FacilityID Varchar (50) = ''
	,@FacilityRequestorID varchar (100) = ''
	,@StatusMode varchar(100) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.

	IF (@StatusMode = '')
	BEGIN
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
					CASE WHEN @SortColumn = '' AND @SortDirection = 'ASC' THEN A.[FacilityRequestID] END ASC,
					CASE WHEN @SortColumn = '' AND @SortDirection = 'DESC' THEN A.[FacilityRequestID] END DESC,
					
					CASE WHEN @SortColumn = 'RequestDate' AND @SortDirection = 'ASC' THEN A.RequestDate END ASC,
					CASE WHEN @SortColumn = 'RequestDate' AND @SortDirection = 'DESC' THEN A.RequestDate END DESC
					
				) AS [Row]
				,A.FacilityRequestID
				,A.RequestFacilityGUID
				,A.FacilityRequestor
				,A.FacilityRequestorID
				,A.RequestDate
				,A.StartTime
				,A.EndTime
				,A.Status
				,A.FacilityID
				,A.Remarks
				,A.ClaimedTime
				,A.ReturnedTime
				,B.RoomNumber [Facility]
				,C.PenaltyID
			FROM RequestFacility A
				LEFT JOIN Penalty C ON A.FacilityRequestID = C.FacilityRequestID
				INNER JOIN Facility B ON B.FacilityID = A.FacilityID
				
			WHERE (@RequestDate = '' OR RequestDate  LIKE '%' + @RequestDate + '%')
				AND (@StartTime = '' OR A.StartTime  LIKE '%' + @StartTime + '%')
				AND (@Remarks = '' OR A.Remarks  LIKE '%' + @Remarks + '%')
				AND (@FacilityRequestorID = '0' OR A.FacilityRequestorID = @FacilityRequestorID)
				AND (@Status = '' AND NOT Status = 'Cancelled' OR Status = @Status)
				AND (@RequestFacilityGUID = '' OR RequestFacilityGUID  LIKE '%' + @RequestFacilityGUID + '%')
		)
		
		SELECT
			(SELECT COUNT(FacilityRequestID) FROM Result) AS [TotalNoOfRecord] -- Default Name
		 ,[Row]
		,FacilityRequestID
				,RequestFacilityGUID
				,RequestDate
				,StartTime
				,EndTime
				,Status
				,Remarks
				,FacilityRequestor
				,FacilityRequestorID
				,Facility
				,FacilityID
				,ClaimedTime
				,ReturnedTime
				,PenaltyID = ISNULL(PenaltyID, 0)
		FROM Result
		WHERE
			Row >= @RowStart		
			AND
			Row <= @NoOfRecord
	END

	ELSE IF (@StatusMode = 'Unclaimed&Claimed')
	BEGIN
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
					CASE WHEN @SortColumn = '' AND @SortDirection = 'ASC' THEN A.[FacilityRequestID] END ASC,
					CASE WHEN @SortColumn = '' AND @SortDirection = 'DESC' THEN A.[FacilityRequestID] END DESC,
					
					CASE WHEN @SortColumn = 'RequestDate' AND @SortDirection = 'ASC' THEN A.RequestDate END DESC,
					CASE WHEN @SortColumn = 'RequestDate' AND @SortDirection = 'DESC' THEN A.RequestDate END DESC
					
				) AS [Row]
				,A.FacilityRequestID
				,A.RequestFacilityGUID
				,A.FacilityRequestor
				,A.FacilityRequestorID
				,A.RequestDate
				,A.StartTime
				,A.EndTime
				,A.Status
				,A.FacilityID
				,A.Remarks
				,A.ClaimedTime
				,A.ReturnedTime
				,B.RoomNumber [Facility]
				,C.PenaltyID
			FROM RequestFacility A
				LEFT JOIN Penalty C ON A.FacilityRequestID = C.FacilityRequestID
				INNER JOIN Facility B ON B.FacilityID = A.FacilityID
				
			WHERE (@RequestDate = '' OR RequestDate  LIKE '%' + @RequestDate + '%')
				AND (@StartTime = '' OR A.StartTime  LIKE '%' + @StartTime + '%')
				AND (@Remarks = '' OR A.Remarks  LIKE '%' + @Remarks + '%')
				AND (@FacilityRequestorID = '0' OR A.FacilityRequestorID = @FacilityRequestorID)
				AND (Status = 'Unclaimed' OR Status = 'Claimed')
				AND (@RequestFacilityGUID = '' OR RequestFacilityGUID  LIKE '%' + @RequestFacilityGUID + '%')
		)
		
		SELECT
			(SELECT COUNT(FacilityRequestID) FROM Result) AS [TotalNoOfRecord] -- Default Name
		 ,[Row]
		,FacilityRequestID
				,RequestFacilityGUID
				,RequestDate
				,StartTime
				,EndTime
				,Status
				,Remarks
				,FacilityRequestor
				,FacilityRequestorID
				,Facility
				,FacilityID
				,ClaimedTime
				,ReturnedTime
				,PenaltyID = ISNULL(PenaltyID, 0)
		FROM Result
		WHERE
			Row >= @RowStart		
			AND
			Row <= @NoOfRecord
	END

	ELSE IF (@StatusMode = 'Completed&Cancelled')
	BEGIN
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
					CASE WHEN @SortColumn = '' AND @SortDirection = 'ASC' THEN A.[FacilityRequestID] END ASC,
					CASE WHEN @SortColumn = '' AND @SortDirection = 'DESC' THEN A.[FacilityRequestID] END DESC,
					
					CASE WHEN @SortColumn = 'RequestDate' AND @SortDirection = 'ASC' THEN A.RequestDate END DESC,
					CASE WHEN @SortColumn = 'RequestDate' AND @SortDirection = 'DESC' THEN A.RequestDate END DESC
					
				) AS [Row]
				,A.FacilityRequestID
				,A.RequestFacilityGUID
				,A.FacilityRequestor
				,A.FacilityRequestorID
				,A.RequestDate
				,A.StartTime
				,A.EndTime
				,A.Status
				,A.FacilityID
				,A.Remarks
				,A.ClaimedTime
				,A.ReturnedTime
				,B.RoomNumber [Facility]
				,C.PenaltyID
			FROM RequestFacility A
				LEFT JOIN Penalty C ON A.FacilityRequestID = C.FacilityRequestID
				INNER JOIN Facility B ON B.FacilityID = A.FacilityID
				
			WHERE (@RequestDate = '' OR RequestDate  LIKE '%' + @RequestDate + '%')
				AND (@StartTime = '' OR A.StartTime  LIKE '%' + @StartTime + '%')
				AND (@Remarks = '' OR A.Remarks  LIKE '%' + @Remarks + '%')
				AND (@FacilityRequestorID = '0' OR A.FacilityRequestorID = @FacilityRequestorID)
				AND (Status = 'Completed' OR Status = 'Cancelled')
				AND (@RequestFacilityGUID = '' OR RequestFacilityGUID  LIKE '%' + @RequestFacilityGUID + '%')
		)
		
		SELECT
			(SELECT COUNT(FacilityRequestID) FROM Result) AS [TotalNoOfRecord] -- Default Name
		 ,[Row]
		,FacilityRequestID
				,RequestFacilityGUID
				,RequestDate
				,StartTime
				,EndTime
				,Status
				,Remarks
				,FacilityRequestor
				,FacilityRequestorID
				,Facility
				,FacilityID
				,ClaimedTime
				,ReturnedTime
				,PenaltyID = ISNULL(PenaltyID, 0)
		FROM Result
		WHERE
			Row >= @RowStart		
			AND
			Row <= @NoOfRecord
	END
END
