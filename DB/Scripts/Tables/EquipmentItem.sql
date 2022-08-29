DROP TABLE IF EXISTS [EquipmentItem]
USE [ERS]
GO

/****** Object:  Table [dbo].[EquipmentItem]    Script Date: 4/8/2022 6:24:33 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EquipmentItem](
	[EquipmentItemID] [int] IDENTITY(1,1) NOT NULL,
	[isActive] [bit] NOT NULL,
	[EquipmentCategoryID] [int] NOT NULL,
	[EquipmentItemCode] [varchar](50) NOT NULL,
	[ItemBrand] [varchar](50) NOT NULL,
	[ItemModel] [varchar](50) NOT NULL,
	[ItemSerialNumber] [varchar](50) NOT NULL,
	[DateBought] [date] NOT NULL,
	[WarrantyStatus] [bit] NOT NULL,
	[isUsable] [bit] NOT NULL,
	[Status] [varchar](50) NOT NULL,
	[NoOfTimesBorrowed] [int] NOT NULL,
	[CreatedBy] [varchar](50) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [varchar](50) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[Comments] [varchar](max) NULL,
 CONSTRAINT [PK_EquipmentItem] PRIMARY KEY CLUSTERED 
(
	[EquipmentItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[EquipmentItem]  WITH CHECK ADD FOREIGN KEY([EquipmentCategoryID])
REFERENCES [dbo].[EquipmentCategory] ([EquipmentCategoryID])
GO


