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
ALTER PROCEDURE spStudentAccountExistingCheck 
	@EmailAddress varchar(100) = '',
	@ResetPasswordCode varchar(100) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT A.EmailAddress, 
			A.SystemUserID,
			B.Password,
			B.IsPasswordChanged,
			B.UserName,
			B.ResetPasswordCode

	FROM Students A
	INNER JOIN SystemUser B ON A.SystemUserID = B.ID
	WHERE A.EmailAddress = @EmailAddress OR B.ResetPasswordCode = @ResetPasswordCode

END
GO
