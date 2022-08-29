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
CREATE PROCEDURE spEquipmentUsageReport
	@DateFrom varchar(50)
	, @DateTo varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	SELECT A.Category
		, A.CategoryCode
		, B. EquipmentItemCode
		, B.ItemBrand
		, B.ItemModel
		, B.ItemSerialNumber
		, B.NoOfTimesBorrowed
		, B.Status
		, @DateFrom as DateFrom
			, @DateTo as DateTo
	FROM EquipmentCategory A
	INNER JOIN EquipmentItem B ON A.EquipmentCategoryID = B.EquipmentCategoryID
	WHERE (@DateFrom = '' OR (CAST(B.CreatedDate as DATE) >= CAST(@DateFrom as DATE)) ) AND (@DateTo = '' OR (CAST(B.CreatedDate as DATE) <= CAST(@DateTo as DATE)) )
END
GO
