USE [ERS]
GO
/****** Object:  StoredProcedure [dbo].[spRequestFacilityInsert]    Script Date: 06/03/2022 2:42:05 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[spRequestFacilityInsert]
	@FacilityRequestID int
	,@RequestFacilityGUID varchar(100) = ''
	,@FacilityRequestor varchar(100) = ''
	,@FacilityRequestorID int
	,@FacilityID int
	,@StartTime varchar(50) = ''
	,@EndTime varchar(50) = ''
	,@Status varchar(100) = 'Unclaimed'
	,@Remarks varchar(250) = ''
	,@GetDate datetime
	,@Schedule varchar(50) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT into RequestFacility(FacilityRequestor
	,FacilityRequestorID
	,FacilityID
	,RequestFacilityGUID
	,RequestDate
	,StartTime
	,EndTime
	,Status
	,Remarks
	,Schedule)
	VALUES (@FacilityRequestor
	,@FacilityRequestorID
	,@FacilityID
	,@RequestFacilityGUID
	,FORMAT (@GetDate, 'M/d/yyyy hh:mm:ss tt')
	,@StartTime
	,@EndTime
	,@Status
	,@Remarks
	,@Schedule)

END

-- SET IDENTITY_INSERT RequestEquipment OFF
