USE [ERS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE spLabPersonnelMoveToArchive 
	@SystemUserID int
AS
BEGIN
SET NOCOUNT ON;

	BEGIN TRY
		-- LabPersonnel to Archive
		SET IDENTITY_INSERT LabPersonnelArchive ON

		INSERT INTO LabPersonnelArchive(
			ID,
			FirstName,
			MiddleName,
			LastName,
			EmailAddress,
			SystemUserID,
			CreatedBy,
			CreatedDate,
			ModifiedBy,
			ModifiedDate)
		SELECT
			ID,
			FirstName,
			MiddleName,
			LastName,
			EmailAddress,
			SystemUserID,
			CreatedBy,
			CreatedDate,
			ModifiedBy,
			ModifiedDate
		FROM LabPersonnel WHERE SystemUserID = @SystemUserID

		SET IDENTITY_INSERT LabPersonnelArchive OFF

		-- SystemUser to Archive
		SET IDENTITY_INSERT SystemUserArchive ON

		INSERT INTO SystemUserArchive(
			 ID,
			 UserName,
			 Password,
			 FirstName,
			 MiddleName,
			 LastName,
			 FailedAttempt,
			 IsPasswordChanged,
			 IsLoggedIn,
			 IsActive,
			 CreatedBy,
			 CreatedDate,
			 ModifiedBy,
			 ModifiedDate)
		SELECT 
			 ID,
			 UserName,
			 Password,
			 FirstName,
			 MiddleName,
			 LastName,
			 FailedAttempt,
			 IsPasswordChanged,
			 IsLoggedIn,
			 IsActive,
			 CreatedBy,
			 CreatedDate,
			 ModifiedBy,
			 ModifiedDate
		 FROM SystemUser WHERE ID = @SystemUserID

		 SET IDENTITY_INSERT SystemUserArchive OFF

		 -- Penalty to Archive
		 SET IDENTITY_INSERT PenaltyArchive ON

		 INSERT INTO PenaltyArchive(
			PenaltyID,
			RequestID,
			FacilityRequestID,
			RequestorID,
			isActive,
			RequestType,
			CreatedBy,
			ModifiedBy,
			PenaltyDetails)
   		 SELECT
			PenaltyID,
			RequestID,
			FacilityRequestID,
			RequestorID,
			isActive,
			RequestType,
			CreatedBy,
			ModifiedBy,
			PenaltyDetails
		 FROM Penalty WHERE RequestorID = @SystemUserID

		 SET IDENTITY_INSERT PenaltyArchive OFF

		 -- RequestFacility to Archive
		 SET IDENTITY_INSERT RequestFacilityArchive ON

		 INSERT INTO RequestFacilityArchive(
			FacilityRequestID,
			RequestFacilityGUID,
			FacilityRequestor,
			FacilityRequestorID,
			FacilityID,
			RequestDate,
			StartTime,
			EndTime,
			Status,
			ClaimedTime,
			ReturnedTime,
			Remarks)
		 SELECT
			FacilityRequestID,
			RequestFacilityGUID,
			FacilityRequestor,
			FacilityRequestorID,
			FacilityID,
			RequestDate,
			StartTime,
			EndTime,
			Status,
			ClaimedTime,
			ReturnedTime,
			Remarks
		 FROM RequestFacility WHERE FacilityRequestorID = @SystemUserID

		 SET IDENTITY_INSERT RequestFacilityArchive OFF

		 -- RequestEquipment to Archive
		 SET IDENTITY_INSERT RequestsArchive ON

		 INSERT INTO RequestsArchive(
			RequestID,
			Requestor,
			RequestorID,
			RequestDateTime,
			RequestGUID,
			StartTime,
			EndTime,
			isApproved,
			Remarks,
			Status,
			ClaimedTime,
			ReturnedTime)
		 SELECT
			RequestID,
			Requestor,
			RequestorID,
			RequestDateTime,
			RequestGUID,
			StartTime,
			EndTime,
			isApproved,
			Remarks,
			Status,
			ClaimedTime,
			ReturnedTime
		 FROM Requests WHERE RequestorID = @SystemUserID

		 SET IDENTITY_INSERT RequestsArchive OFF

		 -- RequestDetails to Archive
		 SET IDENTITY_INSERT RequestDetailsArchive ON

		 INSERT INTO RequestDetailsArchive(
			RequestDetailsID,
			RequestGUID,
			EquipmentCategoryID,
			Quantity,
			StartTime,
			EndTime,
			Status,
			ClaimedTime,
			ReturnedTime,
			RequestorID)
		 SELECT
			RequestDetailsID,
			RequestGUID,
			EquipmentCategoryID,
			Quantity,
			StartTime,
			EndTime,
			Status,
			ClaimedTime,
			ReturnedTime,
			RequestorID
		 FROM RequestDetails WHERE RequestorID = @SystemUserID

		 SET IDENTITY_INSERT RequestDetailsArchive OFF



		 DELETE FROM LabPersonnel WHERE SystemUserID = @SystemUserID
		 DELETE FROM Penalty WHERE RequestorID = @SystemUserID
		 DELETE FROM RequestFacility WHERE FacilityRequestorID = @SystemUserID
		 DELETE FROM Requests WHERE RequestorID = @SystemUserID
		 DELETE FROM RequestDetails WHERE RequestorID = @SystemUserID
		 DELETE FROM SystemUser WHERE ID = @SystemUserID

		SELECT 1
	END TRY

	BEGIN CATCH
		SELECT 0
	END CATCH
END
GO
