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
ALTER PROCEDURE [dbo].[spSystemUserUpdate]
	@ID INT = 0
	, @UserName Varchar(50) = ''
	, @Password Varchar(100) = ''
	, @IsLoggedIn BIT = 0
	, @ModifiedBy INT = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	   UPDATE SystemUser
	   SET
			[UserName] = CASE WHEN (@UserName = '') THEN [UserName] ELSE @UserName END
			, [Password] =  CASE WHEN (@Password = '') THEN [Password] ELSE @Password END		
			, IsLoggedIn = @IsLoggedIn
			, ModifiedBy = @ModifiedBy
	   WHERE ID = @ID

	   SELECT 1
END

