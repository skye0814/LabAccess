
DROP PROCEDURE IF EXISTS spFacilityUpdate
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
CREATE PROCEDURE spFacilityUpdate 
	@FacilityID INT
	,@RoomNumber VARCHAR(50) = ''
	,@RoomType  VARCHAR(50) = ''
	,@RoomDescription VARCHAR(50) = 'Room'
	,@isActive  BIT
	,@Comments  VARCHAR(50) = ''
	,@ModifiedBy INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY
		UPDATE Facility
		SET RoomNumber = @RoomNumber
			, RoomType = @RoomType
			, RoomDescription = @RoomDescription
			, isActive = @isActive
			, Comments = @Comments
			, ModifiedBy = @ModifiedBy
			, ModifiedDate = GETDATE()
		WHERE FacilityID = @FacilityID

		SELECT 1
	END TRY
	BEGIN CATCH
		SELECT 0
	END CATCH
END
GO
