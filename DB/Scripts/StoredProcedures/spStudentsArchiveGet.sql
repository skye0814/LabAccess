DROP PROCEDURE IF EXISTS spStudentGet
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
CREATE PROCEDURE spStudentsArchiveGet
	@ID INT = 0
	,@StudentNumber VARCHAR(50) = ''
	,@EmailAddress VARCHAR(50) = ''
	,@Username VARCHAR(50) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT A.ID
		, A.FirstName
		, A.MiddleName
		, A.LastName
		, StudentNumber
		, CourseID
		, SectionID
		, YearID
		, EmailAddress
		, B.UserName
		, B.[Password]
		, SystemUserID
	FROM StudentsArchive A
	INNER JOIN SystemUserArchive B ON A.SystemUserID = B.ID
	WHERE (@ID = 0 OR A.ID = @ID)
		AND (@StudentNumber = '' OR A.StudentNumber = @StudentNumber)
		AND (@EmailAddress = '' OR A.EmailAddress = @EmailAddress)
		AND (@Username = '' OR B.UserName = @Username)
END
GO
