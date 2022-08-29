DROP PROCEDURE IF EXISTS spRequestEquipmentItemReset
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
CREATE PROCEDURE spRequestEquipmentItemReset 
	@requestGUID varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @requestStatus varchar(50) = ''
	SELECT @requestStatus = Status
		FROM Requests
		WHERE RequestGUID = @requestGUID
	IF (@requestStatus = 'Claimed')
	BEGIN
		BEGIN TRY
			UPDATE RequestEquipmentItem
				SET Status = 'Claimed'
					,isClaimed = 1
				WHERE RequestGUID = @requestGUID
					AND (Status = 'Returned')
			SELECT 1
		END TRY
		BEGIN CATCH
			SELECT 0
		END CATCH
	END
	ELSE IF (@requestStatus = 'Unclaimed')
	BEGIN
		BEGIN TRY
		UPDATE RequestEquipmentItem	
			SET EquipmentItemID = NULL
				,isClaimed = 0
				,Status = 'Unclaimed'
			WHERE RequestGUID = @requestGUID
			SELECT 1
		END TRY
		BEGIN CATCH
			SELECT 0
		END CATCH
	END
	ELSE
	SELECT 1
END
GO
