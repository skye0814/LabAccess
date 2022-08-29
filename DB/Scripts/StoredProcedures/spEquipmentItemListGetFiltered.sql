/****** Object:  StoredProcedure [dbo].[spStudentListGetFiltered]    Script Date: 12/1/2021 9:38:00 PM ******/
DROP PROCEDURE IF EXISTS[dbo].[spEquipmentItemListGetFiltered]
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
CREATE PROCEDURE [dbo].[spEquipmentItemListGetFiltered]
	@RowStart INT = 1
	,@NoOfRecord INT = 10
	,@SortColumn VARCHAR(25) = ''
	,@SortDirection VARCHAR(4) = ''
	----------------------Filters
	,@EquipmentCategoryID INT = 0
	,@EquipmentItemCode VARCHAR(50)=''
	,@Category VARCHAR(50)=''
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
					CASE WHEN @SortColumn = '' AND @SortDirection = 'ASC' THEN [EquipmentItemID] END ASC,
					CASE WHEN @SortColumn = '' AND @SortDirection = 'DESC' THEN [EquipmentItemID] END DESC,

					CASE WHEN @SortColumn = 'isActive' AND @SortDirection = 'ASC' THEN A.isActive END ASC,
					CASE WHEN @SortColumn = 'isActive' AND @SortDirection = 'DESC' THEN A.isActive END DESC,
					
					CASE WHEN @SortColumn = 'EquipmentItemCode' AND @SortDirection = 'ASC' THEN EquipmentItemCode END ASC,
					CASE WHEN @SortColumn = 'EquipmentItemCode' AND @SortDirection = 'DESC' THEN EquipmentItemCode END DESC,

					CASE WHEN @SortColumn = 'Category' AND @SortDirection = 'ASC' THEN Category END ASC,
					CASE WHEN @SortColumn = 'Category' AND @SortDirection = 'DESC' THEN Category END DESC,

					CASE WHEN @SortColumn = 'isUsable' AND @SortDirection = 'ASC' THEN isUsable END ASC,
					CASE WHEN @SortColumn = 'isUsable' AND @SortDirection = 'DESC' THEN isUsable END DESC,

					CASE WHEN @SortColumn = 'ItemBrand' AND @SortDirection = 'ASC' THEN ItemBrand END ASC,
					CASE WHEN @SortColumn = 'ItemBrand' AND @SortDirection = 'DESC' THEN ItemBrand END DESC,

					CASE WHEN @SortColumn = 'ItemModel' AND @SortDirection = 'ASC' THEN ItemModel END ASC,
					CASE WHEN @SortColumn = 'ItemModel' AND @SortDirection = 'DESC' THEN ItemModel END DESC,

					CASE WHEN @SortColumn = 'ItemSerialNumber' AND @SortDirection = 'ASC' THEN ItemSerialNumber END ASC,
					CASE WHEN @SortColumn = 'ItemSerialNumber' AND @SortDirection = 'DESC' THEN ItemSerialNumber END DESC,

					CASE WHEN @SortColumn = 'DateBought' AND @SortDirection = 'ASC' THEN DateBought END ASC,
					CASE WHEN @SortColumn = 'DateBought' AND @SortDirection = 'DESC' THEN DateBought END DESC,

					CASE WHEN @SortColumn = 'WarrantyStatus' AND @SortDirection = 'ASC' THEN WarrantyStatus END ASC,
					CASE WHEN @SortColumn = 'WarrantyStatus' AND @SortDirection = 'DESC' THEN WarrantyStatus END DESC, 

					CASE WHEN @SortColumn = 'Status' AND @SortDirection = 'ASC' THEN Status END ASC,
					CASE WHEN @SortColumn = 'Status' AND @SortDirection = 'DESC' THEN Status END DESC, 

					CASE WHEN @SortColumn = 'NoOfTimesBorrowed' AND @SortDirection = 'ASC' THEN A.NoOfTimesBorrowed END ASC,
					CASE WHEN @SortColumn = 'NoOfTimesBorrowed' AND @SortDirection = 'DESC' THEN A.NoOfTimesBorrowed END DESC, 

					CASE WHEN @SortColumn = 'Comments' AND @SortDirection = 'ASC' THEN A.Comments END ASC,
					CASE WHEN @SortColumn = 'Comments' AND @SortDirection = 'DESC' THEN A.Comments END DESC

				) AS [Row]
				,A.isActive
				,A.EquipmentCategoryID
				,EquipmentItemID
				,Category
				,EquipmentItemCode
				,isUsable
				,ItemBrand
				,ItemModel
				,ItemSerialNumber
				,DateBought
				,WarrantyStatus
				,A.Status
				,A.NoOfTimesBorrowed
				,A.Comments
			FROM EquipmentItem A
			INNER JOIN EquipmentCategory B ON B.EquipmentCategoryID = A.EquipmentCategoryID
			WHERE  (@EquipmentCategoryID = 0 OR A.EquipmentCategoryID = @EquipmentCategoryID)
				AND (@EquipmentItemCode = '' OR EquipmentItemCode  LIKE '%' + @EquipmentItemCode + '%')
				AND (@Category = '' OR Category  LIKE '%' + @Category + '%')
		)
		SELECT
			(SELECT COUNT(EquipmentItemCode) FROM Result) AS [TotalNoOfRecord] -- Default Name
			,[Row]
			,isActive
			,EquipmentCategoryID
			,EquipmentItemID
			,Category
			,EquipmentItemCode
			,isUsable
			,ItemBrand
			,ItemModel
			,ItemSerialNumber
			,DateBought
			,WarrantyStatus
			,Status
			,NoOfTimesBorrowed
			,Comments
		FROM Result
		WHERE
			Row >= @RowStart		
			AND
			Row <= @NoOfRecord
END
GO


