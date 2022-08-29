USE [ERS]

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE spStudentAccountUpdateResetPasswordCode 
	@ResetPasswordCode varchar(100) = '',
	@EmailAddress varchar(100) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    BEGIN TRY

		UPDATE SystemUser
		SET ResetPasswordCode = @ResetPasswordCode
		FROM SystemUser A
		INNER JOIN Students B ON A.ID = B.SystemUserID
		WHERE B.EmailAddress = @EmailAddress

	SELECT 1
	END TRY

	BEGIN CATCH
	SELECT 0
	END CATCH

END
GO
