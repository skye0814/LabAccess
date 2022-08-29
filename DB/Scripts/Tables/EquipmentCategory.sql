DROP TABLE IF EXISTS [EquipmentCategory]
USE [ERS]
GO

/****** Object:  Table [dbo].[EquipmentCategory]    Script Date: 4/8/2022 6:23:32 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EquipmentCategory](
	[EquipmentCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[isActive] [bit] NOT NULL,
	[Category] [varchar](50) NOT NULL,
	[CategoryCode] [varchar](5) NOT NULL,
	[QuantityTotal] [int] NOT NULL,
	[QuantityUsable] [int] NOT NULL,
	[QuantityDefective] [int] NOT NULL,
	[QuantityMissing] [int] NOT NULL,
	[NoOfTimesBorrowed] [int] NOT NULL,
	[Comments] [varchar](50) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_EquipmentCategory] PRIMARY KEY CLUSTERED 
(
	[EquipmentCategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


