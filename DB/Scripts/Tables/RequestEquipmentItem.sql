USE [ERS]
GO

/****** Object:  Table [dbo].[RequestEquipmentItem]    Script Date: 5/10/2022 8:15:16 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RequestEquipmentItem](
	[RequestEquipmentItemID] [int] IDENTITY(1,1) NOT NULL,
	[RequestGUID] [varchar](100) NOT NULL,
	[EquipmentCategoryID] [int] NOT NULL,
	[EquipmentItemID] [int] NULL,
	[isClaimed] [bit] NOT NULL,
	[ClaimedTime] [time](7) NULL,
	[ReturnedTime] [time](7) NULL,
	[Status] [varchar](50) NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[RequestEquipmentItem]  WITH CHECK ADD  CONSTRAINT [FK_RequestEquipmentItem_EquipmentCategory] FOREIGN KEY([EquipmentCategoryID])
REFERENCES [dbo].[EquipmentCategory] ([EquipmentCategoryID])
GO

ALTER TABLE [dbo].[RequestEquipmentItem] CHECK CONSTRAINT [FK_RequestEquipmentItem_EquipmentCategory]
GO

ALTER TABLE [dbo].[RequestEquipmentItem]  WITH CHECK ADD  CONSTRAINT [FK_RequestEquipmentItem_EquipmentItem] FOREIGN KEY([EquipmentItemID])
REFERENCES [dbo].[EquipmentItem] ([EquipmentItemID])
GO

ALTER TABLE [dbo].[RequestEquipmentItem] CHECK CONSTRAINT [FK_RequestEquipmentItem_EquipmentItem]
GO

