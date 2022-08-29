DROP PROCEDURE IF EXISTS spRequestEquipmentItemUpdateItem
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
CREATE PROCEDURE spRequestEquipmentItemUpdateItem 
	@equipmentItemID int = 0
	,@requestGUID varchar(100) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @status VARCHAR(100) = '', @EquipmentCategoryID INT
	SELECT @EquipmentCategoryID = EquipmentCategoryID
		FROM EquipmentItem
		WHERE EquipmentItemID = @equipmentItemID
	SELECT @status = Status
		FROM Requests
		WHERE RequestGUID = @requestGUID
	IF @status = 'COMPLETED'
	BEGIN
		BEGIN TRY
			UPDATE EquipmentItem
				SET NoOfTimesBorrowed =  NoOfTimesBorrowed + 1
				WHERE EquipmentItemID = @equipmentItemID
			EXEC spEquipmentCategoryCountUpdate
				@EquipmentCategoryID
			SELECT 1
		END TRY
		BEGIN CATCH
			SELECT 0
		END CATCH
	END
	ELSE
	BEGIN
		SELECT 0
	END
END
GO
