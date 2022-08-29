DROP TABLE IF EXISTS [Facility]
USE [ERS]
GO

/****** Object:  Table [dbo].[Facility]    Script Date: 1/31/2022 5:20:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Facility](
	[FacilityID] [int] IDENTITY(1,1) NOT NULL,
	[RoomNumber] [varchar](50) NOT NULL,
	[RoomType] [varchar](50) NOT NULL,
	[RoomDescription] [varchar](50) NULL,
	[isActive] [bit] NOT NULL,
	[isAvailable] [bit] NOT NULL,
	[TimeIn] [time](7) NULL,
	[TimeOut] [time](7) NULL,
	[NextSchedule] [datetime] NULL,
	[NoOfTimesBooked] [int] NOT NULL,
	[Comments] [varchar](max) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL
CONSTRAINT [PK_Facility] PRIMARY KEY CLUSTERED 
(
	[FacilityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO 


