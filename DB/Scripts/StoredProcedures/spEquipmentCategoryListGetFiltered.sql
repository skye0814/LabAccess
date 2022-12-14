DROP PROCEDURE IF EXISTS [spEquipmentCategoryListGetFiltered]
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
CREATE PROCEDURE [dbo].[spEquipmentCategoryListGetFiltered]
	@RowStart INT = 1
	,@NoOfRecord INT = 10
	,@SortColumn VARCHAR(25) = ''
	,@SortDirection VARCHAR(4) = ''
	----------------------Filters
	,@Category Varchar(50)=''
	,@CategoryCode Varchar(50)=''
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
					CASE WHEN @SortColumn = '' AND @SortDirection = 'ASC' THEN EquipmentCategoryID END ASC,
					CASE WHEN @SortColumn = '' AND @SortDirection = 'DESC' THEN EquipmentCategoryID END DESC,

					CASE WHEN @SortColumn = 'isActive' AND @SortDirection = 'ASC' THEN isActive END ASC,
					CASE WHEN @SortColumn = 'isActive' AND @SortDirection = 'DESC' THEN isActive END DESC,
					
					CASE WHEN @SortColumn = 'Category' AND @SortDirection = 'ASC' THEN Category END ASC,
					CASE WHEN @SortColumn = 'Category' AND @SortDirection = 'DESC' THEN Category END DESC,

					CASE WHEN @SortColumn = 'CategoryCode' AND @SortDirection = 'ASC' THEN CategoryCode END ASC,
					CASE WHEN @SortColumn = 'CategoryCode' AND @SortDirection = 'DESC' THEN CategoryCode END DESC,

					CASE WHEN @SortColumn = 'QuantityTotal' AND @SortDirection = 'ASC' THEN QuantityTotal END ASC,
					CASE WHEN @SortColumn = 'QuantityTotal' AND @SortDirection = 'DESC' THEN QuantityTotal END DESC,

					CASE WHEN @SortColumn = 'QuantityUsable' AND @SortDirection = 'ASC' THEN QuantityUsable END ASC,
					CASE WHEN @SortColumn = 'QuantityUsable' AND @SortDirection = 'DESC' THEN QuantityUsable END DESC,

					CASE WHEN @SortColumn = 'QuantityDefective' AND @SortDirection = 'ASC' THEN QuantityDefective END ASC,
					CASE WHEN @SortColumn = 'QuantityDefective' AND @SortDirection = 'DESC' THEN QuantityDefective END DESC,

					CASE WHEN @SortColumn = 'QuantityMissing' AND @SortDirection = 'ASC' THEN QuantityMissing END ASC,
					CASE WHEN @SortColumn = 'QuantityMissing' AND @SortDirection = 'DESC' THEN QuantityMissing END DESC,

					CASE WHEN @SortColumn = 'NoOfTimesBorrowed' AND @SortDirection = 'ASC' THEN NoOfTimesBorrowed END ASC,
					CASE WHEN @SortColumn = 'NoOfTimesBorrowed' AND @SortDirection = 'DESC' THEN NoOfTimesBorrowed END DESC,

					CASE WHEN @SortColumn = 'Comments' AND @SortDirection = 'ASC' THEN Comments END ASC,
					CASE WHEN @SortColumn = 'Comments' AND @SortDirection = 'DESC' THEN Comments END DESC
					
				) AS [Row]
				,EquipmentCategoryID
				,isActive
				,Category
				,CategoryCode
				,QuantityTotal
				,QuantityUsable
				,QuantityDefective
				,QuantityMissing
				,NoOfTimesBorrowed
				,Comments
			FROM EquipmentCategory
			WHERE (@Category = '' OR Category  LIKE '%' + @Category + '%')
				AND (@CategoryCode = '' OR CategoryCode  LIKE '%' + @CategoryCode + '%')

		)
		SELECT
			(SELECT COUNT(EquipmentCategoryID) FROM Result) AS [TotalNoOfRecord] -- Default Name
			,[Row]
			,EquipmentCategoryID
			,isActive
			,Category
			,CategoryCode
			,QuantityTotal
			,QuantityUsable
			,QuantityDefective
			,QuantityMissing
			,NoOfTimesBorrowed
			,Comments
		FROM Result
		WHERE
			Row >= @RowStart		
			AND
			Row <= @NoOfRecord
END
