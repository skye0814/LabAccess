USE [ERS]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RequestDetails]') AND type in (N'U'))
DROP TABLE [dbo].[RequestDetails]
GO
/****** Object:  Table [dbo].[EquipmentCategory]    Script Date: 2/23/2022 11:00:31 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RequestDetails](
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
 CONSTRAINT [PK_RequestDetails] PRIMARY KEY CLUSTERED 
(
	[RequestDetailsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[RequestDetails]  WITH CHECK ADD  CONSTRAINT [FK_RequestDetails_EquipmentCategory] FOREIGN KEY([EquipmentCategoryID])
REFERENCES [dbo].[EquipmentCategory] ([EquipmentCategoryID])
GO

ALTER TABLE [dbo].[RequestDetails] CHECK CONSTRAINT [FK_RequestDetails_EquipmentCategory]
GO
