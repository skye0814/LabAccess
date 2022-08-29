ALTER TABLE [dbo].[LabPersonnel] DROP CONSTRAINT [FK_LabPersonnel_SystemUser]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LabPersonnel]') AND type in (N'U'))
DROP TABLE [dbo].[LabPersonnel]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LabPersonnel](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](100) NOT NULL,
	[MiddleName] [varchar](50) NULL,
	[LastName] [varchar](50) NOT NULL,
	[EmailAddress] [varchar](100) NULL,
	[SystemUserID] [bigint] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_LabPersonnel] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[LabPersonnel]  WITH CHECK ADD  CONSTRAINT [FK_LabPersonnel_SystemUser] FOREIGN KEY([SystemUserID])
REFERENCES [dbo].[SystemUser] ([ID])
GO

ALTER TABLE [dbo].[LabPersonnel] CHECK CONSTRAINT [FK_LabPersonnel_SystemUser]
GO



