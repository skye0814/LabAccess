USE [ERS]
GO
DROP PROCEDURE IF EXISTS spScheduleInsert
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
CREATE PROCEDURE spScheduleInsert
	@Day INT
	,@Timein datetime
	,@Timeout datetime
	,@FacilityID INT
	,@SubjectCode VARCHAR(50)
	,@CourseName VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @ID INT
	INSERT INTO Schedule(
		Day
		,TimeIn
		,TimeOut
		,FacilityID
		,SubjectCode
		,CourseName
		,ReservedStatus)
	VALUES (@Day
		,FORMAT(@Timein, 'hh:mm tt')
		,FORMAT(@Timeout, 'hh:mm tt')
		,@FacilityID
		,@SubjectCode
		,@CourseName
		,1)
	SELECT @ID = SCOPE_IDENTITY()
	SELECT @ID
END
GO
