DROP PROCEDURE IF EXISTS spRequestEquipmentItemClaim
USE [ERS]
GO
/****** Object:  StoredProcedure [dbo].[spEquipmentInsert]    Script Date: 1/17/2022 5:40:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].spRequestEquipmentItemClaim
-- Add the parameters for the stored procedure here
	@EquipmentItemCode varchar(50)
	,@requestEquipmentItemID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	-- Insert statements for procedure here
	DECLARE @equipmentCategoryID int = 0
		, @requestEquipmentCategoryID int = 0
		, @requestGUID varchar(100) = ''
		, @requestStatus varchar(50) = ''
		, @equipmentItemID int = 0
	SELECT @equipmentCategoryID = EquipmentCategoryID
			,@equipmentItemID = EquipmentItemID
		FROM EquipmentItem
		WHERE EquipmentItemCode = @EquipmentItemCode
	SELECT @requestEquipmentCategoryID = EquipmentCategoryID
		,@requestGUID = RequestGUID
		FROM RequestEquipmentItem
		WHERE RequestEquipmentItemID = @requestEquipmentItemID
	SELECT @requestStatus = Status
		FROM Requests
		WHERE RequestGUID = @requestGUID
	IF (@requestStatus = 'Unclaimed')
	BEGIN
		IF (@equipmentCategoryID = @requestEquipmentCategoryID)
		BEGIN
			UPDATE RequestEquipmentItem 
				SET EquipmentItemID = @equipmentItemID
					,isClaimed = 1
					,Status = 'Claimed'
				WHERE (RequestEquipmentItemID = @requestEquipmentItemID)
					AND EquipmentCategoryID = @equipmentCategoryID
			SELECT 1
		END
		ELSE
		BEGIN
			SELECT 2
		END
	END
	ELSE
	BEGIN
		SELECT 3
	END
	
END

	
