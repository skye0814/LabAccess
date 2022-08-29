USE [ERS]
GO

DROP TABLE IF EXISTS Schedule

/****** Object:  Table [dbo].[Schedule]    Script Date: 7/20/2022 7:58:42 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Schedule](
	[ScheduleID] [bigint] IDENTITY(1,1) NOT NULL,
	[Day] [int] NOT NULL,
	[TimeIn] [varchar](50) NOT NULL,
	[TimeOut] [varchar](50) NOT NULL,
	[FacilityID] [bigint] NOT NULL,
	[SubjectCode] [varchar](50) NOT NULL,
	[CourseName] [varchar](100) NOT NULL,
	[ReservedStatus] [bit] NOT NULL
) ON [PRIMARY]
GO


