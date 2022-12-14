DROP PROCEDURE IF EXISTS [dbo].[spPenaltyListGetFiltered]
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
CREATE PROCEDURE [dbo].[spPenaltyListGetFiltered]
	@RowStart INT = 1
	,@NoOfRecord INT = 10
	,@SortColumn VARCHAR(25) = ''
	,@SortDirection VARCHAR(4) = ''
	----------------------Filters
	,@RequestorID int = 0
	,@RequestType varchar(50) = ''
	,@StudentName varchar(100) = ''
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
					CASE WHEN @SortColumn = '' AND @SortDirection = 'ASC' THEN PenaltyID END ASC,
					CASE WHEN @SortColumn = '' AND @SortDirection = 'DESC' THEN PenaltyID END DESC,

					CASE WHEN @SortColumn = 'isActive' AND @SortDirection = 'ASC' THEN A.isActive END ASC,
					CASE WHEN @SortColumn = 'isActive' AND @SortDirection = 'DESC' THEN A.isActive END DESC,

					CASE WHEN @SortColumn = 'Requestor' AND @SortDirection = 'ASC' THEN Requestor END ASC,
					CASE WHEN @SortColumn = 'Requestor' AND @SortDirection = 'DESC' THEN Requestor END DESC,

					CASE WHEN @SortColumn = 'RequestType' AND @SortDirection = 'ASC' THEN RequestType END ASC,
					CASE WHEN @SortColumn = 'RequestType' AND @SortDirection = 'DESC' THEN RequestType END DESC,

					CASE WHEN @SortColumn = 'RequestGUID' AND @SortDirection = 'ASC' THEN RequestGUID END ASC,
					CASE WHEN @SortColumn = 'RequestGUID' AND @SortDirection = 'DESC' THEN RequestGUID END DESC
				) AS [Row]
				,PenaltyID
				,A.RequestID
				,A.FacilityRequestID
				,A.RequestorID
				,A.isActive
				,RequestType
				,B.RequestGUID
				,C.RequestFacilityGUID
				,Requestor = CONCAT(D.Firstname, ' ', D.LastName)
			FROM Penalty A
				LEFT JOIN Requests B ON A.RequestID = B.RequestID
				LEFT JOIN RequestFacility C ON A.FacilityRequestID = C.FacilityRequestID
				INNER JOIN SystemUser D ON D.ID = A.RequestorID
			WHERE (@RequestorID = 0 OR A.RequestorID = @RequestorID)
				AND (@RequestType = '' OR A.RequestType LIKE '%' + @RequestType + '%')
		)
		SELECT
			(SELECT COUNT(FacilityRequestID) FROM Result) AS [TotalNoOfRecord] -- Default Name
			,[Row]
			,PenaltyID
			,RequestID = ISNULL(RequestID,0)
			,FacilityRequestID = ISNULL(FacilityRequestID,0)
			,RequestorID
			,isActive
			,RequestType
			,RequestGUID = ISNULL(RequestGUID, RequestFacilityGUID)
			,Requestor
		FROM Result
		WHERE
			Row >= @RowStart		
			AND
			Row <= @NoOfRecord
			AND (@StudentName = '' OR Requestor  LIKE '%' + @StudentName + '%')
END
