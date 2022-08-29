-- ================================

-- Create the data type
CREATE TYPE udtbltypeStudent AS TABLE 
(
	RowNumber INT
	, FirstName varchar(50)
	, MiddleName Varchar(50)
	, LastName Varchar(50)
	, StudentNumber Varchar(50)
	, Course Varchar(50)
	, Section Varchar(50)
	, [Year] Varchar(50)
	, EmailAddress Varchar (50)
	, UserName Varchar (50)
	, [Password] VARCHAR(500)
)
GO
