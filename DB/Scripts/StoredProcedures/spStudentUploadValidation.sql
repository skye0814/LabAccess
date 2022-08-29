 DROP PROCEDURE IF EXISTS [dbo].[spStudentUploadValidation]
/****** Object:  StoredProcedure [dbo].[spStudentUploadValidation]    Script Date: 4/1/2022 7:52:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spStudentUploadValidation]
	@tblStudent udtbltypeStudent READONLY 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @FirstName VARCHAR(50)
	DECLARE @MiddleName Varchar(50)
	DECLARE @LastName Varchar(50)
	DECLARE @StudentNumber Varchar(50)
	DECLARE @Course Varchar(50)
	DECLARE @Section Varchar(50)
	DECLARE @Year Varchar(50)
	DECLARE @EmailAddress Varchar (50)
	DECLARE @UserName Varchar (50)
	DECLARE @RowNumber INT = 0
	DECLARE @UserId INT = 0
	DECLARE @tblErrorMessages AS TABLE([message] varchar(MAX))

	SELECT * INTO #tempStudent 
	FROM @tblStudent
	
	WHILE (SELECT COUNT(*) FROM #tempStudent) > 0
	BEGIN
		
		SELECT TOP 1 @FirstName = FirstName
			, @MiddleName = MiddleName
			, @LastName = LastName
			, @StudentNumber = StudentNumber
			, @Course = Course
			, @Section = Section
			, @Year = [Year]
			, @EmailAddress = EmailAddress
			, @UserName = UserName
			, @RowNumber = RowNumber
		FROM #tempStudent 
		ORDER BY RowNumber

		----- VALIDATE Username

		SELECT @UserId = ID FROM SystemUser WHERE UserName = @UserName
		IF @UserId != 0
		BEGIN
			IF EXISTS (SELECT * FROM Students WHERE SystemUserID = @UserID AND StudentNumber != @StudentNumber)
			BEGIN
				INSERT INTO @tblErrorMessages VALUES(CAST(@RowNumber as varchar(5)) + '*' + @UserName 
				+ '*UserName already used by another student.' )
			END
			ELSE IF EXISTS (SELECT * FROM LabPersonnel WHERE SystemUserID = @UserID )
			BEGIN
				INSERT INTO @tblErrorMessages VALUES(CAST(@RowNumber as varchar(5)) + '*' + @UserName 
				+ '*UserName already used by a lab personnel.' )
			END
		END

		IF NOT EXISTS (SELECT * FROM Course WHERE CourseCode = @Course)
		BEGIN
				INSERT INTO @tblErrorMessages VALUES(CAST(@RowNumber as varchar(5)) + '*' + @Course 
				+ '*Course does not exists.' )
		END

		IF NOT EXISTS (SELECT * FROM Section WHERE SectionCode = @Section)
		BEGIN
				INSERT INTO @tblErrorMessages VALUES(CAST(@RowNumber as varchar(5)) + '*' + @Section 
				+ '*Section does not exists.' )
		END

		IF NOT EXISTS (SELECT * FROM [Year] WHERE YearCode = @Year)
		BEGIN
				INSERT INTO @tblErrorMessages VALUES(CAST(@RowNumber as varchar(5)) + '*' + @Course 
				+ '* Year does not exists.' )
		END

		DELETE #tempStudent WHERE RowNumber = @RowNumber

	END

	SELECT * FROM @tblErrorMessages
END