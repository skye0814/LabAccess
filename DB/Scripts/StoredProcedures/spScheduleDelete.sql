DROP PROCEDURE IF EXISTS spScheduleDelete
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
CREATE PROCEDURE spScheduleDelete
	@ScheduleID INT = 0
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @ID INT
	BEGIN TRY
		DELETE FROM Schedule
			WHERE ScheduleID = @ScheduleID
		SELECT 1
	END TRY
	BEGIN CATCH
		SELECT 0
	END CATCH
	
END
GO
