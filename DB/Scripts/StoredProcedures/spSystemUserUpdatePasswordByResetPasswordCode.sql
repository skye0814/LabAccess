USE [ERS]
GO
/****** Object:  StoredProcedure [dbo].[spSystemUserUpdate]    Script Date: 15/04/2022 1:44:51 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: 11/21/2021
-- Description:	Update System User
-- =============================================
ALTER PROCEDURE spSystemUserUpdatePasswordByResetPasswordCode
	@ResetPasswordCode varchar(100) = '',
	@Password varchar(100) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	   UPDATE SystemUser
	   SET Password = @Password,
	   ResetPasswordCode = NULL
	   WHERE ResetPasswordCode = @ResetPasswordCode

	   SELECT 1
END

