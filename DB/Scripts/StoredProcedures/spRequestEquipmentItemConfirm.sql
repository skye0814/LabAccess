DROP PROCEDURE IF EXISTS spRequestEquipmentItemConfirm
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
CREATE PROCEDURE spRequestEquipmentItemConfirm 
	@requestGUID varchar(100),
	@GetDate datetime
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
			UPDATE Requests
				SET Status = 'Completed'
					,ReturnedTime = FORMAT (@GetDate, 'M/d/yyyy hh:mm:ss tt')
				WHERE RequestGUID = @requestGUID
			UPDATE RequestDetails
				SET Status = 'Completed'
					,ReturnedTime = FORMAT (@GetDate, 'M/d/yyyy hh:mm:ss tt')
				WHERE RequestGUID = @requestGUID
			SELECT 1
		END TRY
		BEGIN CATCH
			SELECT 0
		END CATCH
	END
	ELSE IF (@requestStatus = 'Unclaimed')
	BEGIN
		BEGIN TRY
			UPDATE Requests
			SET Status = 'Claimed'
				,ClaimedTime = FORMAT (@GetDate, 'M/d/yyyy hh:mm:ss tt')
			WHERE RequestGUID = @requestGUID
			UPDATE RequestDetails
				SET Status = 'Claimed'
					,ClaimedTime = FORMAT (@GetDate, 'M/d/yyyy hh:mm:ss tt')
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
