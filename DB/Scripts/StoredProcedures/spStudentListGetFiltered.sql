USE [ERS]
GO

/****** Object:  StoredProcedure [dbo].[spStudentListGetFiltered]    Script Date: 12/1/2021 9:38:00 PM ******/
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
ALTER PROCEDURE [dbo].[spStudentListGetFiltered]
	@RowStart INT = 1
	,@NoOfRecord INT = 10
	,@SortColumn VARCHAR(25) = ''
	,@SortDirection VARCHAR(4) = ''
	----------------------Filters
	, @StudentNumber Varchar(50)=''
	,@FirstName Varchar(50) =''
	, @LastName Varchar(50) = ''
	,@YearID INT = 0
	,@SectionID INT = 0
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
				,StudentNumber
				,A.SystemUserID
				,CourseDescription [Course]
				, YearDescription [Year]
				,EmailAddress
				,SectionDescription [Section]
				, UserName
			FROM Students A
			INNER JOIN Course B ON B.CourseID = A.CourseID
			INNER JOIN Section C ON A.SectionID = C.SectionID
			INNER JOIN [Year] D ON D.YEarID = A.YearID 
			INNER JOIN SystemUser E ON E.ID = A.SystemUserID
			WHERE (@StudentNumber = '' OR StudentNumber  LIKE '%' + @StudentNumber + '%')
				AND (@FirstName = '' OR A.FirstName  LIKE '%' + @FirstName + '%')
				AND (@LastName = '' OR A.LastNAme  LIKE '%' + @LastName + '%')
				AND (@YearID = 0 OR D.YEarID = @YearID)
				AND (@SectionID = 0 OR C.SectionID = @SectionID )
		)
		
		SELECT
			(SELECT COUNT(ID) FROM Result) AS [TotalNoOfRecord] -- Default Name
		 ,[Row]
		,A.ID
				,A.FirstName
				,A.MiddleName
				,A.LastName
				,StudentNumber
				,Course
				,[YEar]
				,A.EmailAddress
				,Section
				, UserName
				,A.SystemUserID
		FROM Result A
			LEFT JOIN LabPersonnel B on A.SystemUserID = B.SystemUserID
		WHERE
			B.SystemUserID IS NULL
			AND
			Row >= @RowStart		
			AND
			Row <= @NoOfRecord
END
GO


