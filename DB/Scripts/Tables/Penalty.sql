USE [ERS]
GO

/****** Object:  Table [dbo].[Penalty]    Script Date: 5/21/2022 6:22:18 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Penalty](
	[PenaltyID] [bigint] IDENTITY(1,1) NOT NULL,
	[RequestID] [bigint] NULL,
	[FacilityRequestID] [bigint] NULL,
	[RequestorID] [bigint] NULL,
	[isActive] [bit] NULL,
	[RequestType] [varchar](50) NULL,
	[CreatedBy] [int] NULL,
	[ModifiedBy] [int] NULL,
	[PenaltyDetails] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
