USE [ERS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE spLabPersonnelDeleteFromArchive
	@SystemUserID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY
		DELETE FROM LabPersonnelArchive WHERE SystemUserID = @SystemUserID
		DELETE FROM PenaltyArchive WHERE RequestorID = @SystemUserID
		DELETE FROM RequestFacilityArchive WHERE FacilityRequestorID = @SystemUserID
		DELETE FROM RequestDetailsArchive WHERE RequestorID = @SystemUserID
		DELETE FROM RequestsArchive WHERE RequestorID = @SystemUserID
		DELETE FROM SystemUserArchive WHERE ID = @SystemUserID

		SELECT 1
	END TRY
	BEGIN CATCH
		SELECT ERROR_MESSAGE()
		SELECT 0
	END CATCH
END
GO
