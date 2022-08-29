
/****** Object:  Table [dbo].[Year]    Script Date: 12/4/2021 10:23:01 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Year]') AND type in (N'U'))
DROP TABLE [dbo].[Year]
GO

/****** Object:  Table [dbo].[Year]    Script Date: 12/4/2021 10:23:01 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Year](
	[YearID] [int] IDENTITY(1,1) NOT NULL,
	[YearCode] [varchar](50) NULL,
	[YearDescription] [varchar](100) NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Year] PRIMARY KEY CLUSTERED 
(
	[YearID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


