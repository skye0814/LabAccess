DROP PROCEDURE IF EXISTS spEquipmentCategoryReport
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
CREATE PROCEDURE spEquipmentCategoryReport
	@DateFrom varchar(50)
	, @DateTo varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	SELECT EquipmentCategoryID
	,isActive
	,Category
	,CategoryCode
	,QuantityTotal
	,QuantityUsable
	,QuantityDefective
	,QuantityMissing
	,NoOfTimesBorrowed =
		(SELECT (COUNT(B.Status))
			FROM RequestEquipmentItem B
			INNER JOIN Requests C ON B.RequestGUID = C.RequestGUID
			WHERE (B.Status = 'Claimed' OR B.Status = 'Returned')
				AND A.EquipmentCategoryID = B.EquipmentCategoryID
				AND (@DateFrom = '' OR CAST(C.ClaimedTime as DATE) >= CAST(@DateFrom as DATE)) 
				AND (@DateTo = '' OR CAST(C.ClaimedTime as DATE) <= CAST(@DateTo as DATE)) )
	, @DateFrom as DateFrom
	, @DateTo as DateTo
FROM EquipmentCategory A
END
GO
