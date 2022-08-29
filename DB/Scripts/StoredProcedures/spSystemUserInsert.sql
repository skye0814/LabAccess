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
CREATE PROCEDURE spSystemUserInsert
	@UserName VARCHAR(50) = ''
	,@Password VARCHAR(500) = ''
	,@FirstName VARCHAR(50) = ''
	,@MiddleName VARCHAR(50) = ''
	,@LastName VARCHAR(50) = ''
	,@FailedAttempt smallint = 0
	,@IsPasswordChanged bit = 0
	,@IsLoggedIn bit = 0
	,@IsActive bit = 0
	,@CreatedBy int = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   INSERT INTO SystemUser (UserName
	,[Password]
	,FirstName
	,MiddleName
	,LastName
	,FailedAttempt
	,IsPasswordChanged
	,IsLoggedIn
	,IsActive
	,CreatedBy
	,CreatedDate)
	VALUES (@UserName 
	,@Password 
	,@FirstName
	,@MiddleName 
	,@LastName
	,@FailedAttempt 
	,@IsPasswordChanged 
	,@IsLoggedIn
	,@IsActive
	,@CreatedBy
	, GETDATE())

	SELECT SCOPE_IDENTITY()
END
GO