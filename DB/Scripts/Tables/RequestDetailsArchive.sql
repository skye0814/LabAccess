USE [ERS]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RequestDetailsArchive]') AND type in (N'U'))
DROP TABLE [dbo].[RequestDetailsArchive]
GO
/****** Object:  Table [dbo].[EquipmentCategory]    Script Date: 2/23/2022 11:00:31 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RequestDetailsArchive](
	[RequestDetailsID] [int] IDENTITY(1,1) NOT NULL,
	[RequestGUID] [varchar](100) NOT NULL,
	[EquipmentCategoryID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[StartTime] [varchar](50) NOT NULL,
	[EndTime] [varchar](50) NOT NULL,
	[Status] [varchar](50) NULL,
	[ClaimedTime] [varchar](50) NULL,
	[ReturnedTime] [varchar](50) NULL,
	[RequestorID] [int] NOT NULL
 CONSTRAINT [PK_RequestDetailsArchive] PRIMARY KEY CLUSTERED 
(
	[RequestDetailsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[RequestDetailsArchive]  WITH CHECK ADD  CONSTRAINT [FK_RequestDetailsArchive_EquipmentCategory] FOREIGN KEY([EquipmentCategoryID])
REFERENCES [dbo].[EquipmentCategory] ([EquipmentCategoryID])
GO

ALTER TABLE [dbo].[RequestDetailsArchive] CHECK CONSTRAINT [FK_RequestDetailsArchive_EquipmentCategory]
GO
