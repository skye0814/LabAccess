USE [ERS]
GO

ALTER TABLE [dbo].[RequestFacilityArchive] DROP CONSTRAINT
[FK_RequestFacilityArchive_Facility]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RequestFacilityArchive]') AND type in (N'U'))
DROP TABLE [dbo].[RequestFacilityArchive]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RequestFacilityArchive] (
	[FacilityRequestID] [bigint] IDENTITY (1,1) NOT NULL,
	[RequestFacilityGUID] [varchar] (100) NOT NULL,
	[FacilityRequestor] [varchar](100) NOT NULL,
	[FacilityRequestorID] [int] NOT NULL,
	[FacilityID] [int] NOT NULL,
	[RequestDate] [varchar](100) NOT NULL,
	[StartTime] [varchar](100) NOT NULL,
	[EndTime] [varchar](100) NOT NULL,
	[Status] [varchar](100) NOT NULL,
	[ClaimedTime] [varchar](100) NULL,
	[ReturnedTime] [varchar](100) NULL,
	[Remarks] [varchar](50) NULL
 CONSTRAINT [PK_RequestFacilityArchive] PRIMARY KEY CLUSTERED 
(
	[FacilityRequestID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[RequestFacilityArchive]  WITH CHECK ADD  CONSTRAINT [FK_RequestFacilityArchive_Facility] FOREIGN KEY([FacilityID])
REFERENCES [dbo].[Facility] ([FacilityID])
GO

ALTER TABLE [dbo].[RequestFacilityArchive] CHECK CONSTRAINT [FK_RequestFacilityArchive_Facility]
GO