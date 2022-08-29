DROP PROCEDURE IF EXISTS spRequestFacilityReturn
USE [ERS]
GO
/****** Object:  StoredProcedure [dbo].[spEquipmentInsert]    Script Date: 1/17/2022 5:40:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].spRequestFacilityReturn
-- Add the parameters for the stored procedure here
	@requestFacilityGUID VARCHAR(100) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	-- Insert statements for procedure here
	DECLARE @requestStatus varchar(50) = ''
	SELECT @requestStatus = Status
		FROM RequestFacility
		WHERE RequestFacilityGUID = @requestFacilityGUID
	IF (@requestStatus = 'Claimed')
	BEGIN
		UPDATE RequestFacility
			SET Status = 'Returned'
			WHERE RequestFacilityGUID = @requestFacilityGUID
		SELECT 1
	END
	ELSE
	BEGIN
		SELECT 0
	END
END

	
