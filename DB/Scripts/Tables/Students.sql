
ALTER TABLE [dbo].[Students] DROP CONSTRAINT [FK_Students_Year]
GO

ALTER TABLE [dbo].[Students] DROP CONSTRAINT [FK_Students_SystemUser]
GO

ALTER TABLE [dbo].[Students] DROP CONSTRAINT [FK_Students_Section]
GO

ALTER TABLE [dbo].[Students] DROP CONSTRAINT [FK_Students_Course]
GO

/****** Object:  Table [dbo].[Students]    Script Date: 12/4/2021 10:35:37 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Students]') AND type in (N'U'))
DROP TABLE [dbo].[Students]
GO

/****** Object:  Table [dbo].[Students]    Script Date: 12/4/2021 10:35:37 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Students](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](100) NOT NULL,
	[MiddleName] [varchar](50) NULL,
	[LastName] [varchar](50) NOT NULL,
	[StudentNumber] [varchar](50) NOT NULL,
	[CourseID] [int] NOT NULL,
	[SectionID] [int] NOT NULL,
	[YearID] [int] NOT NULL,
	[EmailAddress] [varchar](100) NULL,
	[SystemUserID] [bigint] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_Students] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Students]  WITH CHECK ADD  CONSTRAINT [FK_Students_Course] FOREIGN KEY([CourseID])
REFERENCES [dbo].[Course] ([CourseID])
GO

ALTER TABLE [dbo].[Students] CHECK CONSTRAINT [FK_Students_Course]
GO

ALTER TABLE [dbo].[Students]  WITH CHECK ADD  CONSTRAINT [FK_Students_Section] FOREIGN KEY([SectionID])
REFERENCES [dbo].[Section] ([SectionID])
GO

ALTER TABLE [dbo].[Students] CHECK CONSTRAINT [FK_Students_Section]
GO

ALTER TABLE [dbo].[Students]  WITH CHECK ADD  CONSTRAINT [FK_Students_SystemUser] FOREIGN KEY([SystemUserID])
REFERENCES [dbo].[SystemUser] ([ID])
GO

ALTER TABLE [dbo].[Students] CHECK CONSTRAINT [FK_Students_SystemUser]
GO

ALTER TABLE [dbo].[Students]  WITH CHECK ADD  CONSTRAINT [FK_Students_Year] FOREIGN KEY([YearID])
REFERENCES [dbo].[Year] ([YearID])
GO

ALTER TABLE [dbo].[Students] CHECK CONSTRAINT [FK_Students_Year]
GO


