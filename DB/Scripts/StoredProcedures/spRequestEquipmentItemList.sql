DROP PROCEDURE IF EXISTS spRequestEquipmentItemList


-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE spRequestEquipmentItemList
	@RowStart INT = 1
	,@NoOfRecord INT = 10
	,@SortColumn VARCHAR(25) = ''
	,@SortDirection VARCHAR(4) = ''
	----------------------Filters
	,@RequestGUID varchar(100) = ''
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
					CASE WHEN @SortColumn = '' AND @SortDirection = 'ASC' THEN RequestEquipmentItemID END ASC,
					CASE WHEN @SortColumn = '' AND @SortDirection = 'DESC' THEN RequestEquipmentItemID END DESC,

					CASE WHEN @SortColumn = 'RequestGUID' AND @SortDirection = 'ASC' THEN RequestGUID END ASC,
					CASE WHEN @SortColumn = 'RequestGUID' AND @SortDirection = 'DESC' THEN RequestGUID END DESC,

					CASE WHEN @SortColumn = 'Category' AND @SortDirection = 'ASC' THEN Category END ASC,
					CASE WHEN @SortColumn = 'Category' AND @SortDirection = 'DESC' THEN Category END DESC,

					CASE WHEN @SortColumn = 'EquipmentItemID' AND @SortDirection = 'ASC' THEN A.EquipmentItemID END ASC,
					CASE WHEN @SortColumn = 'EquipmentItemID' AND @SortDirection = 'DESC' THEN A.EquipmentItemID END DESC,

					CASE WHEN @SortColumn = 'EquipmentItemCode' AND @SortDirection = 'ASC' THEN EquipmentItemCode END ASC,
					CASE WHEN @SortColumn = 'EquipmentItemCode' AND @SortDirection = 'DESC' THEN EquipmentItemCode END DESC,

					CASE WHEN @SortColumn = 'isClaimed' AND @SortDirection = 'ASC' THEN isClaimed END ASC,
					CASE WHEN @SortColumn = 'isClaimed' AND @SortDirection = 'DESC' THEN isClaimed END DESC,

					CASE WHEN @SortColumn = 'Status' AND @SortDirection = 'ASC' THEN A.Status END ASC,
					CASE WHEN @SortColumn = 'Status' AND @SortDirection = 'DESC' THEN A.Status END DESC		
				) AS [Row]
				,A.RequestEquipmentItemID
				,A.RequestGUID
				,D.Category 
				,A.EquipmentItemID
				,E.EquipmentItemCode
				,A.isClaimed
				,A.Status
				FROM RequestEquipmentItem A
				INNER JOIN EquipmentCategory D ON A.EquipmentCategoryID = D.EquipmentCategoryID
				LEFT JOIN EquipmentItem E ON E.EquipmentItemID = A.EquipmentItemID
				WHERE (@RequestGUID = '' OR A.RequestGUID = @RequestGUID)
					AND (A.EquipmentItemID IS NULL OR A.EquipmentItemID = E.EquipmentItemID)
		)

		SELECT
			(SELECT COUNT(RequestEquipmentItemID) FROM Result) AS [TotalNoOfRecord] -- Default Name
			,[Row]
			,RequestEquipmentItemID
			,RequestGUID
			,Category
			,EquipmentItemID = ISNULL(EquipmentItemID,0)
			,EquipmentItemCode = ISNULL(EquipmentItemCode, 'Unassigned')
			,isClaimed
			,Status
		FROM Result
		WHERE
			Row >= @RowStart		
			AND
			Row <= @NoOfRecord
END
GO