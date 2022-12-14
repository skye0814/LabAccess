DROP PROCEDURE IF EXISTS [dbo].[spPenaltyInsert]
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
CREATE PROCEDURE [dbo].[spPenaltyInsert]
-- Add the parameters for the stored procedure here
	@RequestType varchar(50) = ''
	,@RequestGUID varchar(50) = ''
	,@PenaltyDetails varchar(max) = ''
	,@CreatedBy bigint = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @requestID int, @facilityRequestID int, @requestorID int = 0
	IF (@RequestType = 'Equipment')
	BEGIN
		SELECT @requestID = RequestID
			,@requestorID = RequestorID
			FROM Requests
			WHERE RequestGUID = @RequestGUID
	END
	ELSE IF (@RequestType = 'Facility')
	BEGIN
		SELECT @facilityRequestID = FacilityRequestID
			,@requestorID = FacilityRequestorID
			FROM RequestFacility
			WHERE RequestFacilityGUID = @RequestGUID
	END
	INSERT INTO Penalty	(RequestID
		,FacilityRequestID
		,RequestorID
		,isActive
		,RequestType
		,CreatedBy
		,PenaltyDetails)
	VALUES (@requestID
		,@facilityRequestID
		,@requestorID
		,1
		,@RequestType
		,@CreatedBy
		,@PenaltyDetails)
	SELECT SCOPE_IDENTITY() 
END

	
