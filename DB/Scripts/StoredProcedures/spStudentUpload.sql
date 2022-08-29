
/****** Object:  StoredProcedure [dbo].[spStudentUploadValidation]    Script Date: 4/1/2022 8:29:57 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].spStudentUpload
	@tblStudent udtbltypeStudent READONLY 
	, @UserID INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Count INT
	DECLARE @TotalCount INT = 0
	BEGIN TRAN
	BEGIN TRY
		SELECT @Count = COUNT(*) FROM @tblStudent

		-----INSERT NEW USERS
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
				, CreatedDate)
		SELECT UserName
			, [Password]
			, FirstName
			, MiddleName
			, LastName
			, 0
			, 0
			, 0
			, 1
			, @UserID
			, GETDATE()
		FROM @tblStudent A
		WHERE UserName NOT IN (SELECT UserName FROM SystemUser)

		SELECT @TotalCount += @@ROWCOUNT

		----- UPDATE Existing Student
		UPDATE A
		SET A.FirstName = B.FirstName
			, A.MiddleName = B.MiddleName
			, A.LastName = B.LastName
			, A.EmailAddress = B.EmailAddress
			, ModifiedBy = @UserID
			, ModifiedDate = GETDATE()
		FROM Students A
		INNER JOIN @tblStudent B ON B.StudentNumber = A.StudentNumber
		INNER JOIN SystemUser C ON B.UserName = C.UserName

		SELECT @TotalCount += @@ROWCOUNT

		---- INSERT NEW STUDENT
		INSERT INTO Students (FirstName
					,MiddleName
					,LastName
					,StudentNumber
					,CourseID
					,SectionID
					,YearID
					,EmailAddress
					,SystemUserID
					,CreatedBy
					,CreatedDate)
		SELECT A.FirstName
			, A.MiddleName
			, A.LastName
			, A.StudentNumber
			, CourseID
			, SectionID
			, YearID
			, EmailAddress
			, E.ID
			, @UserID
			, GETDATE()
		FROM @tblStudent A
		INNER JOIN Course B ON A.Course = B.CourseCode
		INNER JOIN Section C ON C.SectionCode = A.Section
		INNER JOIN [Year] D ON D.YearCode = A.[Year]
		INNER JOIN SystemUser E ON A.UserName = E.UserName
		WHERE A.StudentNumber NOT IN (SELECT StudentNumber FROM Students)

		SELECT @TotalCount += @@ROWCOUNT

		SELECT @TotalCount

		COMMIT TRAN
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN
		SELECT - 1
	END CATCH
END
GO

