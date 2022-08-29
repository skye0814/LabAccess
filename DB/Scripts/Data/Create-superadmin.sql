-- RUN FIRST
INSERT INTO SystemUser(UserName
,Password
,FirstName
,LastName
,FailedAttempt
,IsPasswordChanged
,IsLoggedIn
,IsActive
,CreatedBy
,CreatedDate)
VALUES ('superadmin'
,'p5tOe5o7ppzllOAU+vx5BA==' -- 'admin'
,'superadmin'
,'superadmin'
,0
,0
,0
,1
,0
,GETDATE())
-- RUN FIRST



DECLARE @systemUserID INT = X -- CHANGE VALUE 
INSERT INTO LabPersonnel (FirstName
,LastName
,SystemUserID
,CreatedBy
,CreatedDate)
VALUES('superadmin'
,'superadmin'
,@systemUserID
,0
,GETDATE())

INSERT INTO Students (FirstName
,MiddleName
,LastName
,StudentNumber
,CourseID
,SectionID
,YearID
,SystemUserID
,CreatedBy
,CreatedDate)
VALUES ('superadmin'
,'superadmin'
,'superadmin'
,'superadmin'
,1
,1
,1
,@systemUserID
,0
,GETDATE())