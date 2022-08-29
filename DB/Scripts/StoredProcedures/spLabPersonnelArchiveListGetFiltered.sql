USE [ERS]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spLabPersonnelArchiveListGetFiltered]
	@RowStart INT = 1
	,@NoOfRecord INT = 10
	,@SortColumn VARCHAR(25) = ''
	,@SortDirection VARCHAR(4) = ''
	----------------------Filters
	, @MiddleName Varchar(50)=''
	,@FirstName Varchar(50) =''
	, @LastName Varchar(50) = ''
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
					CASE WHEN @SortColumn = '' AND @SortDirection = 'ASC' THEN A.[ID] END ASC,
					CASE WHEN @SortColumn = '' AND @SortDirection = 'DESC' THEN A.[ID] END DESC,
					
					CASE WHEN @SortColumn = 'FirstName' AND @SortDirection = 'ASC' THEN A.FirstName END ASC,
					CASE WHEN @SortColumn = 'FirstName' AND @SortDirection = 'DESC' THEN A.FirstName END DESC
					
				) AS [Row]
				,A.ID
				,A.FirstName
				,A.MiddleName
				,A.LastName
				,A.SystemUserID
				,EmailAddress
				, UserName
			FROM LabPersonnelArchive A
			INNER JOIN SystemUserArchive E ON E.ID = A.SystemUserID 
			WHERE (@MiddleName = '' OR A.MiddleName  LIKE '%' + @MiddleName + '%')
				AND (@FirstName = '' OR A.FirstName  LIKE '%' + @FirstName + '%')
				AND (@LastName = '' OR A.LastName  LIKE '%' + @LastName + '%')
		)
		
		SELECT
			(SELECT COUNT(ID) FROM Result) AS [TotalNoOfRecord] -- Default Name
		 ,[Row]
		,ID
				,FirstName
				,MiddleName
				,LastName
				,EmailAddress
				, UserName
				,SystemUserID
		FROM Result
		WHERE
			Row >= @RowStart		
			AND
			Row <= @NoOfRecord
END
GO
