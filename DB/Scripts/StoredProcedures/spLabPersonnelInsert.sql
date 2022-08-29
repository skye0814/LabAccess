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
ALTER PROCEDURE spLabPersonnelInsert
	@FirstName VARCHAR(50) = ''
	,@MiddleName  VARCHAR(50) = ''
	,@LastName  VARCHAR(50) = ''
	,@EmailAddress  VARCHAR(50) = ''
	,@CreatedBy INT
	,@UserName VARCHAR(50) = ''
	,@Password VARCHAR(500) = ''
	,@SystemUserID_Archive int
	,@ID int
	,@SystemUserID_Archive_bigint bigint
	,@ID_bigint bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
BEGIN TRAN
BEGIN TRY
	DECLARE @SystemUserID TABLE (ID INT)
	DECLARE @UserID INT

	IF (@SystemUserID_Archive = 0) -- Dito dapat yung from LabPersonnelRegistrationAdd
	BEGIN
		INSERT INTO @SystemUserID EXEC spSystemUserInsert @UserName
		,@Password
		,@FirstName 
		,@MiddleName 
		,@LastName
		,0
		,0
		,0
		,1
		,@CreatedBy


		SELECT @UserID = ID  FROM @SystemUserID

		INSERT INTO LabPersonnel(FirstName
				,MiddleName
				,LastName
				,EmailAddress
				, SystemUserID
				,CreatedBy
				,CreatedDate)
		VALUES (@FirstName
				,@MiddleName
				,@LastName
				,@EmailAddress
				,@UserID
				,@CreatedBy
				,GETDATE())

		SELECT SCOPE_IDENTITY()
		COMMIT TRAN
	END
	ELSE -- Dito naman papasok yung from LabPersonnelArchiveEdit (RESTORE ACCOUNT)
	BEGIN
		-- Archive to SystemUser
		SET IDENTITY_INSERT SystemUser ON

		INSERT INTO SystemUser(
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
			CreatedDate)
		VALUES(
			@SystemUserID_Archive_bigint,
			@UserName,
			@Password,
			@FirstName,
			@MiddleName,
			@LastName,
			0,
			0,
			0,
			1,
			@CreatedBy,
			GETDATE())

		SET IDENTITY_INSERT SystemUser OFF

		-- Archive to LabPersonnel
		SET IDENTITY_INSERT LabPersonnel ON

		INSERT INTO LabPersonnel(ID
				,FirstName
				,MiddleName
				,LastName
				,EmailAddress
				, SystemUserID
				,CreatedBy
				,CreatedDate)
		VALUES (@ID_bigint
				,@FirstName
				,@MiddleName
				,@LastName
				,@EmailAddress
				,@SystemUserID_Archive_bigint
				,@CreatedBy
				,GETDATE())

		 SET IDENTITY_INSERT LabPersonnel OFF

		-- Archive to Penalty
		 SET IDENTITY_INSERT Penalty ON

		 INSERT INTO Penalty(
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
		 FROM PenaltyArchive WHERE RequestorID = @SystemUserID_Archive

		 SET IDENTITY_INSERT Penalty OFF

		 -- Archive to RequestFacility
		 SET IDENTITY_INSERT RequestFacility ON

		 INSERT INTO RequestFacility(
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
		 FROM RequestFacilityArchive WHERE FacilityRequestorID = @SystemUserID_Archive

		 SET IDENTITY_INSERT RequestFacility OFF

		 --  Archive to RequestEquipment
		 SET IDENTITY_INSERT Requests ON

		 INSERT INTO Requests(
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
		 FROM RequestsArchive WHERE RequestorID = @SystemUserID_Archive

		 SET IDENTITY_INSERT Requests OFF

		 -- Archive to RequestDetails
		 SET IDENTITY_INSERT RequestDetails ON

		 INSERT INTO RequestDetails(
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
		 FROM RequestDetailsArchive WHERE RequestorID = @SystemUserID_Archive

		 SET IDENTITY_INSERT RequestDetails OFF

		DELETE FROM SystemUserArchive WHERE ID = @SystemUserID_Archive_bigint
		DELETE FROM PenaltyArchive WHERE RequestorID = @SystemUserID_Archive_bigint
		DELETE FROM RequestFacilityArchive WHERE FacilityRequestorID = @SystemUserID_Archive
		DELETE FROM RequestDetailsArchive WHERE RequestorID = @SystemUserID_Archive
		DELETE FROM RequestsArchive WHERE RequestorID = @SystemUserID_Archive
		DELETE FROM LabPersonnelArchive WHERE SystemUserID = @SystemUserID_Archive_bigint

		SELECT SCOPE_IDENTITY()
		COMMIT TRAN
	END
    
END TRY
BEGIN CATCH
	ROLLBACK TRAN
	SELECT Error_Message()
	SELECT 0
END CATCH
END
GO
