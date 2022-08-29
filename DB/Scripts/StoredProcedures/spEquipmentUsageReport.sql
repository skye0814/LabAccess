DROP PROCEDURE IF EXISTS spEquipmentUsageReport
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
	, D.EquipmentItemCode
	, D.ItemBrand
	, D.ItemModel
	, D.ItemSerialNumber
	,NoOfTimesBorrowed =
		(SELECT (COUNT(B.Status))
			FROM RequestEquipmentItem B
			INNER JOIN Requests C ON B.RequestGUID = C.RequestGUID
			WHERE (B.Status = 'Claimed' OR B.Status = 'Returned')
				AND D.EquipmentItemID = B.EquipmentItemID
				AND (@DateFrom = '' OR CAST(C.ClaimedTime as DATE) >= CAST(@DateFrom as DATE)) 
				AND (@DateTo = '' OR CAST(C.ClaimedTime as DATE) <= CAST(@DateTo as DATE)) )
	, D.Status
	, @DateFrom as DateFrom
	, @DateTo as DateTo
FROM EquipmentCategory A
INNER JOIN EquipmentItem D ON A.EquipmentCategoryID = D.EquipmentCategoryID
END
GO
