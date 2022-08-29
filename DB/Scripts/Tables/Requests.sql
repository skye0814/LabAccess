USE [ERS]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Requests]') AND type in (N'U'))
DROP TABLE [dbo].[Requests]
GO
/****** Object:  Table [dbo].[EquipmentCategory]    Script Date: 2/23/2022 11:00:31 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Requests](
	[RequestID] [bigint] IDENTITY(1,1) NOT NULL,
	[Requestor] [varchar](100) NOT NULL,
	[RequestorID] [int] NOT NULL,
	[RequestDateTime] [varchar](50) NOT NULL,
	[RequestGUID] [varchar](100) NOT NULL,
	[StartTime] [varchar](50) NOT NULL,
	[EndTime] [varchar](50) NOT NULL,
	[isApproved] [int] NULL,
	[Remarks] [varchar](250) NULL,
	[Status] [varchar](50) NULL,
	[ClaimedTime] [varchar](50) NULL,
	[ReturnedTime] [varchar](50) NULL
)
GO