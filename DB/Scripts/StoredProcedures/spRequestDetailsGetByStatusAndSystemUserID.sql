USE [ERS]
GO
/****** Object:  StoredProcedure [dbo].[spRequestDetailsGetByRequestGUID]    Script Date: 4/23/2022 9:59:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[spRequestDetailsGetByStatusAndSystemUserID]
	@DateToday datetime,
	@Status varchar(50),
	@SystemUserID int
AS
BEGIN
	
	IF (@Status = 'Unclaimed')
	BEGIN
		SET NOCOUNT ON;
		SELECT A.EquipmentCategoryID,
		A.RequestGUID,
		A.Quantity,
		A.StartTime,
		A.EndTime,
		A.Status,
		B.Category

		FROM RequestDetails A
		INNER JOIN EquipmentCategory B ON A.EquipmentCategoryID = B.EquipmentCategoryID
		WHERE (RequestorID = @SystemUserID AND Status = 'Unclaimed' AND (@DateToday BETWEEN StartTime AND EndTime))
	END
	ELSE IF (@Status = 'Claimed')
	BEGIN
		SET NOCOUNT ON;
		SELECT A.EquipmentCategoryID,
		A.RequestGUID,
		A.Quantity,
		A.StartTime,
		A.EndTime,
		A.Status,
		B.Category

		FROM RequestDetails A
		INNER JOIN EquipmentCategory B ON A.EquipmentCategoryID = B.EquipmentCategoryID
		WHERE (RequestorID = @SystemUserID AND Status = 'Claimed' AND (@DateToday BETWEEN StartTime AND EndTime))
	END
END
