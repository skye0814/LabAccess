DROP PROCEDURE IF EXISTS spFacilityInsert
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
CREATE PROCEDURE spFacilityInsert
	@RoomNumber VARCHAR(50) = ''
	,@RoomType VARCHAR(50) = ''
	,@RoomDescription VARCHAR(50) = 'Room'
	,@Comments VARCHAR(250) = ''
	,@CreatedBy INT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @ID INT
	INSERT INTO Facility (
		RoomNumber
		,RoomType
		,RoomDescription
		,isAvailable
		,isActive
		,NoOfTimesBooked
		,Comments
		,CreatedBy
		,CreatedDate)
	VALUES (@RoomNumber
		,@RoomType
		,@RoomDescription
		,1
		,1
		,0
		,@Comments
		,@CreatedBy
		,GETDATE())
	SELECT @ID = SCOPE_IDENTITY()
	SELECT @ID
END
GO
