
/****** Object:  StoredProcedure [dbo].[spSystemUserGet]    Script Date: 1/31/2022 3:46:14 PM ******/
DROP PROCEDURE IF EXISTS [dbo].[spSystemUserGet]
GO

/****** Object:  StoredProcedure [dbo].[spSystemUserGet]    Script Date: 1/31/2022 3:46:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: 11/21/2021
-- Description:	Get System User Detaails
-- =============================================
CREATE PROCEDURE [dbo].[spSystemUserGet] 
	@ID INT = 0
	, @UserName Varchar(50) = ''
	, @Password Varchar(100) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT A.ID 
		, A.UserName
		, A.FirstName
		, A.MiddleName
		, A.LastName
		, A.FailedAttempt
		, A.IsPasswordChanged
		, A.IsLoggedIn
		, A.isActive
		, CASE WHEN B.ID IS NULL THEN 0 ELSE 1 END as isStudent
		, CASE WHEN C.ID IS NULL THEN 0 ELSE 1 END as isLabPersonnel
	FROM SystemUser A
	LEFT JOIN Students B ON B.SystemUserID = A.ID 
	LEFT JOIN LabPersonnel C ON C.SystemUserID = A.ID
	WHERE (@ID = 0 OR A.ID = @ID)
		AND (@UserName = '' OR UserName = @UserName)
		AND (@Password = '' OR [Password] = @Password)
END
GO


