DROP PROCEDURE IF EXISTS spFacilityGet
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
CREATE PROCEDURE spFacilityGet
	@FacilityID INT = 0
	,@RoomNumber VARCHAR (50) = ''
	,@RoomType VARCHAR (50) = ''
	,@isActive bit 
	,@isAvailable bit
	,@Comments VARCHAR (50) = ''

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT 
		FacilityID
		,RoomNumber
		,RoomType
		,RoomDescription
		,isActive
		,isAvailable
		,Comments
	FROM Facility
	WHERE (@FacilityID = 0 OR FacilityID = @FacilityID)
		AND (@RoomNumber = '' OR RoomNumber = @RoomNumber)
END
GO
