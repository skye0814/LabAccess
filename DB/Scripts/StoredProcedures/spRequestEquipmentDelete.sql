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
ALTER PROCEDURE spRequestEquipmentDelete
	@IDs udtblID readonly
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY
		DECLARE @RequestIDs udtblID

		INSERT INTO @RequestIDs
		SELECT RequestID FROM RequestEquipment WHERE RequestID IN (SELECT ID FROM @IDs)

		DELETE RequestEquipment  WHERE RequestID IN (SELECT RequestID FROM @RequestIDs)
		SELECT 1
	END TRY
	BEGIN CATCH
		SELECT ERROR_MESSAGE()
		SELECT 0
	END CATCH
END
GO