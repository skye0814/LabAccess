USE [ERS]
GO

DROP PROCEDURE IF EXISTS spRequestFacilityConfirm
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
CREATE PROCEDURE spRequestFacilityConfirm 
	@requestGUID varchar(100),
	@GetDate datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @requestStatus varchar(50) = ''
	SELECT @requestStatus = Status
		FROM RequestFacility
		WHERE RequestFacilityGUID = @requestGUID
	IF (@requestStatus = 'Claimed')
	BEGIN
		BEGIN TRY
			UPDATE RequestFacility
				SET ClaimedTime = FORMAT (@GetDate, 'M/d/yyyy hh:mm:ss tt')
				WHERE RequestFacilityGUID = @requestGUID
					AND (ClaimedTime IS NULL)
			SELECT 1
		END TRY
		BEGIN CATCH
			SELECT 0
		END CATCH
	END
	ELSE IF (@requestStatus = 'Returned')
	BEGIN
		BEGIN TRY
			UPDATE RequestFacility
			SET Status = 'Completed'
				,ReturnedTime = FORMAT (@GetDate, 'M/d/yyyy hh:mm:ss tt')
			WHERE RequestFacilityGUID = @requestGUID
				AND (ClaimedTime IS NOT NULL)
			EXEC spRequestFacilityUpdateFacility
				@requestGUID
			SELECT 1
		END TRY
		BEGIN CATCH
			SELECT 0
		END CATCH
	END
	ELSE
	BEGIN
		SELECT 1
	END
END
GO
