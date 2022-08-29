
DROP PROCEDURE IF EXISTS spStudentUpdate
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
CREATE PROCEDURE spStudentUpdate 
	@ID INT
	,@FirstName VARCHAR(50) = ''
	,@MiddleName  VARCHAR(50) = ''
	,@LastName  VARCHAR(50) = ''
	,@StudentNumber  VARCHAR(50) = ''
	,@CourseID INT
	,@SectionID INT
	,@YearID INT
	,@EmailAddress  VARCHAR(50) = ''
	,@ModifiedBy INT
	,@SystemUserID INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY
		UPDATE Students
		SET FirstName = @FirstName
			, LastName = @LastName
			, MiddleName = @MiddleName
			, StudentNumber = @StudentNumber
			, CourseID = @CourseID
			, SectionID = @SectionID
			, YearID = @YearID
			, EmailAddress = @EmailAddress
			, ModifiedBy = @ModifiedBy
			, ModifiedDate = GETDATE()
		WHERE ID = @ID

		UPDATE SystemUser
		SET FirstName = @FirstName,
			MiddleName = @MiddleName,
			LastName = @LastName,
			ModifiedBy = @ModifiedBy,
			ModifiedDate = GETDATE()
		WHERE ID = @SystemUserID

		SELECT 1
	END TRY
	BEGIN CATCH
		SELECT 0
	END CATCH
END
GO
