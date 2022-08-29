USE [ERS]
GO

/****** Object:  Table [dbo].[SystemUserArchive]    Script Date: 6/2/2022 5:52:21 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[StudentsArchive](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](100) NOT NULL,
	[MiddleName] [varchar](50) NULL,
	[LastName] [varchar](50) NOT NULL,
	[StudentNumber] [varchar](50) NOT NULL,
	[CourseID] [int] NOT NULL,
	[SectionID] [int] NOT NULL,
	[YearID] [int] NOT NULL,
	[EmailAddress] [varchar](100) NULL,
	[SystemUserID] [bigint] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL
) ON [PRIMARY]
GO