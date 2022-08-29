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
CREATE PROCEDURE spEquipmentInventoryReport
	@DateFrom varchar(50)
	, @DateTo varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	SELECT A.EquipmentCategoryID
			,A.isActive
			,A.Category
			,A.CategoryCode
			,A.QuantityTotal
			,A.QuantityUsable
			,A.QuantityDefective
			,A.QuantityMissing
			,A.NoOfTimesBorrowed 
			, @DateFrom as DateFrom
			, @DateTo as DateTo
	FROM EquipmentCategory A
	WHERE (@DateFrom = '' OR (CAST(CreatedDate as DATE) >= CAST(@DateFrom as DATE)) ) AND (@DateTo = '' OR (CAST(CreatedDate as DATE) <= CAST(@DateTo as DATE)) )
END
GO
