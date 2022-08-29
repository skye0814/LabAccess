USE [ERS]
GO

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
ALTER PROCEDURE [dbo].[spRequestEquipmentListGetFiltered]
	@RowStart INT = 1
	,@NoOfRecord INT = 10
	,@SortColumn VARCHAR(25) = ''
	,@SortDirection VARCHAR(4) = ''
	----------------------Filters
	, @Date Varchar(50)=''
	,@Time Varchar(50) =''
	, @Remarks Varchar(50) = ''
	,@isApproved bit
	,@RequestorID varchar(100) = ''
	,@Quantity int
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
					CASE WHEN @SortColumn = '' AND @SortDirection = 'ASC' THEN A.[RequestID] END ASC,
					CASE WHEN @SortColumn = '' AND @SortDirection = 'DESC' THEN A.[RequestID] END DESC,
					
					CASE WHEN @SortColumn = 'Date' AND @SortDirection = 'ASC' THEN A.Date END ASC,
					CASE WHEN @SortColumn = 'Date' AND @SortDirection = 'DESC' THEN A.Date END DESC
					
				) AS [Row]
				,A.RequestID
				,A.Quantity
				,A.RequestorID
				,A.Date
				,A.Time
				,A.isApproved
				,A.Remarks
				,Category [EquipmentCategory]
			FROM RequestEquipment A
			INNER JOIN EquipmentCategory B ON B.ID = A.EquipmentCategoryID
			WHERE (@Date = '' OR Date  LIKE '%' + @Date + '%')
				AND (@Time = '' OR A.Time  LIKE '%' + @Time + '%')
				AND (@Remarks = '' OR A.Remarks  LIKE '%' + @Remarks + '%')
		)
		
		SELECT
			(SELECT COUNT(RequestID) FROM Result) AS [TotalNoOfRecord] -- Default Name
		 ,[Row]
		,RequestID
				,Date
				,Time
				,isApproved
				,Remarks
				,Quantity
				,RequestorID
				,EquipmentCategory
		FROM Result
		WHERE
			Row >= @RowStart		
			AND
			Row <= @NoOfRecord
END
GO