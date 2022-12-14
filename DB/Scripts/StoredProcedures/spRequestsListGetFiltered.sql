USE [ERS]
GO
/****** Object:  StoredProcedure [dbo].[spRequestsListGetFiltered]    Script Date: 5/21/2022 9:05:44 PM ******/
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
ALTER PROCEDURE [dbo].[spRequestsListGetFiltered]
	@RowStart INT = 1
	,@NoOfRecord INT = 10
	,@SortColumn VARCHAR(25) = ''
	,@SortDirection VARCHAR(4) = ''
	----------------------Filters
	,@Requestor VARCHAR(50) = ''
	,@RequestDateTime VARCHAR(50) = ''
	,@StartTime VARCHAR(50) = ''
	,@EndTime VARCHAR(50) = ''
	,@isApproved INT = 0
	,@Remarks VARCHAR(200) = ''
	,@RequestGUID VARCHAR(50) = ''
	,@Status VARCHAR(50) = ''
	,@ClaimedTime VARCHAR(50) = ''
	,@ReturnedTime VARCHAR(50) = ''
	,@RequestorID VARCHAR(100) = ''
	,@StatusMode varchar (100) = ''
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
						CASE WHEN @SortColumn = '' AND @SortDirection = 'ASC' THEN A.[RequestID] END ASC,
						CASE WHEN @SortColumn = '' AND @SortDirection = 'DESC' THEN A.[RequestID] END DESC,

						CASE WHEN @SortColumn = 'RequestGUID' AND @SortDirection = 'ASC' THEN RequestGUID END ASC,
						CASE WHEN @SortColumn = 'RequestGUID' AND @SortDirection = 'DESC' THEN RequestGUID END DESC,

						CASE WHEN @SortColumn = 'Requestor' AND @SortDirection = 'ASC' THEN Requestor END ASC,
						CASE WHEN @SortColumn = 'Requestor' AND @SortDirection = 'DESC' THEN Requestor END DESC,

						CASE WHEN @SortColumn = 'RequestDateTime' AND @SortDirection = 'ASC' THEN RequestDateTime END ASC,
						CASE WHEN @SortColumn = 'RequestDateTime' AND @SortDirection = 'DESC' THEN RequestDateTime END DESC,
					
						CASE WHEN @SortColumn = 'StartTime' AND @SortDirection = 'ASC' THEN StartTime END ASC,
						CASE WHEN @SortColumn = 'StartTime' AND @SortDirection = 'DESC' THEN StartTime END DESC,

						CASE WHEN @SortColumn = 'EndTime' AND @SortDirection = 'ASC' THEN EndTime END ASC,
						CASE WHEN @SortColumn = 'EndTime' AND @SortDirection = 'DESC' THEN EndTime END DESC,

						CASE WHEN @SortColumn = 'isApproved' AND @SortDirection = 'ASC' THEN isApproved END ASC,
						CASE WHEN @SortColumn = 'isApproved' AND @SortDirection = 'DESC' THEN isApproved END DESC,

						CASE WHEN @SortColumn = 'Remarks' AND @SortDirection = 'ASC' THEN Remarks END ASC,
						CASE WHEN @SortColumn = 'Remarks' AND @SortDirection = 'DESC' THEN Remarks END DESC,

						CASE WHEN @SortColumn = 'Status' AND @SortDirection = 'ASC' THEN Status END ASC,
						CASE WHEN @SortColumn = 'Status' AND @SortDirection = 'DESC' THEN Status END DESC,

						CASE WHEN @SortColumn = 'ClaimedTime' AND @SortDirection = 'ASC' THEN ClaimedTime END ASC,
						CASE WHEN @SortColumn = 'ClaimedTime' AND @SortDirection = 'DESC' THEN ClaimedTime END DESC,

						CASE WHEN @SortColumn = 'ReturnedTime' AND @SortDirection = 'ASC' THEN ReturnedTime END ASC,
						CASE WHEN @SortColumn = 'ReturnedTime' AND @SortDirection = 'DESC' THEN ReturnedTime END DESC,

						CASE WHEN @SortColumn = 'RequestorID' AND @SortDirection = 'ASC' THEN A.RequestorID END ASC,
						CASE WHEN @SortColumn = 'RequestorID' AND @SortDirection = 'DESC' THEN A.RequestorID END DESC
					) AS [Row]
					,A.RequestID
					,Requestor
					,StartTime
					,EndTime
					,isApproved
					,Remarks
					,RequestGUID
					,RequestDateTime
					,Status
					,ClaimedTime
					,ReturnedTime
					,A.RequestorID
					,B.PenaltyID
				FROM Requests A
					LEFT JOIN Penalty B ON A.RequestID = B.RequestID
				WHERE (@RequestDateTime = '' OR RequestDateTime  LIKE '%' + @RequestDateTime + '%')
					AND (@StartTime = '' OR StartTime  LIKE '%' + @StartTime + '%')
					AND (@RequestGUID = '' OR RequestGUID  LIKE '%' + @RequestGUID + '%')
					AND (@Requestor = '' OR Requestor LIKE '%' + @Requestor + '%')
					AND (@ClaimedTime = '' OR ClaimedTime LIKE '%' + @ClaimedTime + '%')
					AND (@ReturnedTime = '' OR ReturnedTime LIKE '%' + @ReturnedTime + '%')
					AND (@Status = '' AND NOT Status = 'Cancelled' OR Status = @Status)
					AND (@Remarks = '' OR Remarks LIKE '%' + @Remarks + '%')
					AND (@RequestorID = '0' OR A.RequestorID = @RequestorID)
			)
			SELECT
				(SELECT COUNT(RequestID) FROM Result) AS [TotalNoOfRecord] -- Default Name
				,[Row]
				,RequestID
				,Requestor
				,StartTime
				,EndTime
				,isApproved
				,RequestGUID
				,Remarks
				,RequestDateTime
				,ClaimedTime
				,ReturnedTime
				,Status
				,RequestorID
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
						CASE WHEN @SortColumn = '' AND @SortDirection = 'ASC' THEN A.[RequestID] END ASC,
						CASE WHEN @SortColumn = '' AND @SortDirection = 'DESC' THEN A.[RequestID] END DESC,

						CASE WHEN @SortColumn = 'RequestGUID' AND @SortDirection = 'ASC' THEN RequestGUID END ASC,
						CASE WHEN @SortColumn = 'RequestGUID' AND @SortDirection = 'DESC' THEN RequestGUID END DESC,

						CASE WHEN @SortColumn = 'Requestor' AND @SortDirection = 'ASC' THEN Requestor END ASC,
						CASE WHEN @SortColumn = 'Requestor' AND @SortDirection = 'DESC' THEN Requestor END DESC,

						CASE WHEN @SortColumn = 'RequestDateTime' AND @SortDirection = 'ASC' THEN RequestDateTime END DESC,
						CASE WHEN @SortColumn = 'RequestDateTime' AND @SortDirection = 'DESC' THEN RequestDateTime END DESC,
					
						CASE WHEN @SortColumn = 'StartTime' AND @SortDirection = 'ASC' THEN StartTime END ASC,
						CASE WHEN @SortColumn = 'StartTime' AND @SortDirection = 'DESC' THEN StartTime END DESC,

						CASE WHEN @SortColumn = 'EndTime' AND @SortDirection = 'ASC' THEN EndTime END ASC,
						CASE WHEN @SortColumn = 'EndTime' AND @SortDirection = 'DESC' THEN EndTime END DESC,

						CASE WHEN @SortColumn = 'isApproved' AND @SortDirection = 'ASC' THEN isApproved END ASC,
						CASE WHEN @SortColumn = 'isApproved' AND @SortDirection = 'DESC' THEN isApproved END DESC,

						CASE WHEN @SortColumn = 'Remarks' AND @SortDirection = 'ASC' THEN Remarks END ASC,
						CASE WHEN @SortColumn = 'Remarks' AND @SortDirection = 'DESC' THEN Remarks END DESC,

						CASE WHEN @SortColumn = 'Status' AND @SortDirection = 'ASC' THEN Status END ASC,
						CASE WHEN @SortColumn = 'Status' AND @SortDirection = 'DESC' THEN Status END DESC,

						CASE WHEN @SortColumn = 'ClaimedTime' AND @SortDirection = 'ASC' THEN ClaimedTime END ASC,
						CASE WHEN @SortColumn = 'ClaimedTime' AND @SortDirection = 'DESC' THEN ClaimedTime END DESC,

						CASE WHEN @SortColumn = 'ReturnedTime' AND @SortDirection = 'ASC' THEN ReturnedTime END ASC,
						CASE WHEN @SortColumn = 'ReturnedTime' AND @SortDirection = 'DESC' THEN ReturnedTime END DESC,

						CASE WHEN @SortColumn = 'RequestorID' AND @SortDirection = 'ASC' THEN A.RequestorID END ASC,
						CASE WHEN @SortColumn = 'RequestorID' AND @SortDirection = 'DESC' THEN A.RequestorID END DESC
					) AS [Row]
					,A.RequestID
					,Requestor
					,StartTime
					,EndTime
					,isApproved
					,Remarks
					,RequestGUID
					,RequestDateTime
					,Status
					,ClaimedTime
					,ReturnedTime
					,A.RequestorID
					,B.PenaltyID
				FROM Requests A
					LEFT JOIN Penalty B ON A.RequestID = B.RequestID
				WHERE (@RequestDateTime = '' OR RequestDateTime  LIKE '%' + @RequestDateTime + '%')
					AND (@StartTime = '' OR StartTime  LIKE '%' + @StartTime + '%')
					AND (@RequestGUID = '' OR RequestGUID  LIKE '%' + @RequestGUID + '%')
					AND (@Requestor = '' OR Requestor LIKE '%' + @Requestor + '%')
					AND (@ClaimedTime = '' OR ClaimedTime LIKE '%' + @ClaimedTime + '%')
					AND (@ReturnedTime = '' OR ReturnedTime LIKE '%' + @ReturnedTime + '%')
					AND (Status = 'Unclaimed' OR Status = 'Claimed')
					AND (@Remarks = '' OR Remarks LIKE '%' + @Remarks + '%')
					AND (@RequestorID = '0' OR A.RequestorID = @RequestorID)
			)
			SELECT
				(SELECT COUNT(RequestID) FROM Result) AS [TotalNoOfRecord] -- Default Name
				,[Row]
				,RequestID
				,Requestor
				,StartTime
				,EndTime
				,isApproved
				,RequestGUID
				,Remarks
				,RequestDateTime
				,ClaimedTime
				,ReturnedTime
				,Status
				,RequestorID
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
						CASE WHEN @SortColumn = '' AND @SortDirection = 'ASC' THEN A.[RequestID] END ASC,
						CASE WHEN @SortColumn = '' AND @SortDirection = 'DESC' THEN A.[RequestID] END DESC,

						CASE WHEN @SortColumn = 'RequestGUID' AND @SortDirection = 'ASC' THEN RequestGUID END ASC,
						CASE WHEN @SortColumn = 'RequestGUID' AND @SortDirection = 'DESC' THEN RequestGUID END DESC,

						CASE WHEN @SortColumn = 'Requestor' AND @SortDirection = 'ASC' THEN Requestor END ASC,
						CASE WHEN @SortColumn = 'Requestor' AND @SortDirection = 'DESC' THEN Requestor END DESC,

						CASE WHEN @SortColumn = 'RequestDateTime' AND @SortDirection = 'ASC' THEN RequestDateTime END DESC,
						CASE WHEN @SortColumn = 'RequestDateTime' AND @SortDirection = 'DESC' THEN RequestDateTime END DESC,
					
						CASE WHEN @SortColumn = 'StartTime' AND @SortDirection = 'ASC' THEN StartTime END ASC,
						CASE WHEN @SortColumn = 'StartTime' AND @SortDirection = 'DESC' THEN StartTime END DESC,

						CASE WHEN @SortColumn = 'EndTime' AND @SortDirection = 'ASC' THEN EndTime END ASC,
						CASE WHEN @SortColumn = 'EndTime' AND @SortDirection = 'DESC' THEN EndTime END DESC,

						CASE WHEN @SortColumn = 'isApproved' AND @SortDirection = 'ASC' THEN isApproved END ASC,
						CASE WHEN @SortColumn = 'isApproved' AND @SortDirection = 'DESC' THEN isApproved END DESC,

						CASE WHEN @SortColumn = 'Remarks' AND @SortDirection = 'ASC' THEN Remarks END ASC,
						CASE WHEN @SortColumn = 'Remarks' AND @SortDirection = 'DESC' THEN Remarks END DESC,

						CASE WHEN @SortColumn = 'Status' AND @SortDirection = 'ASC' THEN Status END ASC,
						CASE WHEN @SortColumn = 'Status' AND @SortDirection = 'DESC' THEN Status END DESC,

						CASE WHEN @SortColumn = 'ClaimedTime' AND @SortDirection = 'ASC' THEN ClaimedTime END ASC,
						CASE WHEN @SortColumn = 'ClaimedTime' AND @SortDirection = 'DESC' THEN ClaimedTime END DESC,

						CASE WHEN @SortColumn = 'ReturnedTime' AND @SortDirection = 'ASC' THEN ReturnedTime END ASC,
						CASE WHEN @SortColumn = 'ReturnedTime' AND @SortDirection = 'DESC' THEN ReturnedTime END DESC,

						CASE WHEN @SortColumn = 'RequestorID' AND @SortDirection = 'ASC' THEN A.RequestorID END ASC,
						CASE WHEN @SortColumn = 'RequestorID' AND @SortDirection = 'DESC' THEN A.RequestorID END DESC
					) AS [Row]
					,A.RequestID
					,Requestor
					,StartTime
					,EndTime
					,isApproved
					,Remarks
					,RequestGUID
					,RequestDateTime
					,Status
					,ClaimedTime
					,ReturnedTime
					,A.RequestorID
					,B.PenaltyID
				FROM Requests A
					LEFT JOIN Penalty B ON A.RequestID = B.RequestID
				WHERE (@RequestDateTime = '' OR RequestDateTime  LIKE '%' + @RequestDateTime + '%')
					AND (@StartTime = '' OR StartTime  LIKE '%' + @StartTime + '%')
					AND (@RequestGUID = '' OR RequestGUID  LIKE '%' + @RequestGUID + '%')
					AND (@Requestor = '' OR Requestor LIKE '%' + @Requestor + '%')
					AND (@ClaimedTime = '' OR ClaimedTime LIKE '%' + @ClaimedTime + '%')
					AND (@ReturnedTime = '' OR ReturnedTime LIKE '%' + @ReturnedTime + '%')
					AND (Status = 'Completed' OR Status = 'Cancelled')
					AND (@Remarks = '' OR Remarks LIKE '%' + @Remarks + '%')
					AND (@RequestorID = '0' OR A.RequestorID = @RequestorID)
			)
			SELECT
				(SELECT COUNT(RequestID) FROM Result) AS [TotalNoOfRecord] -- Default Name
				,[Row]
				,RequestID
				,Requestor
				,StartTime
				,EndTime
				,isApproved
				,RequestGUID
				,Remarks
				,RequestDateTime
				,ClaimedTime
				,ReturnedTime
				,Status
				,RequestorID
				,PenaltyID = ISNULL(PenaltyID, 0)
			FROM Result
			WHERE
				Row >= @RowStart		
				AND
				Row <= @NoOfRecord
		END
END
