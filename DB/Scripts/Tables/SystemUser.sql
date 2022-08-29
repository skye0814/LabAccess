USE [ERS]
GO

/****** Object:  Table [dbo].[SystemUser]    Script Date: 7/4/2022 1:31:57 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SystemUser](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](50) NULL,
	[Password] [varchar](500) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[MiddleName] [varchar](50) NULL,
	[LastName] [varchar](50) NOT NULL,
	[FailedAttempt] [smallint] NOT NULL,
	[IsPasswordChanged] [bit] NOT NULL,
	[IsLoggedIn] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ResetPasswordCode] [varchar](100) NULL,
 CONSTRAINT [PK_SystemUser] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SystemUser] ADD  CONSTRAINT [DF_SystemUser_FailedAttempt_1]  DEFAULT ((0)) FOR [FailedAttempt]
GO

ALTER TABLE [dbo].[SystemUser] ADD  CONSTRAINT [DF_SystemUser_IsPasswordChanged_1]  DEFAULT ((0)) FOR [IsPasswordChanged]
GO

ALTER TABLE [dbo].[SystemUser] ADD  CONSTRAINT [DF_SystemUser_IsLoggedIn]  DEFAULT ((0)) FOR [IsLoggedIn]
GO

ALTER TABLE [dbo].[SystemUser] ADD  CONSTRAINT [DF_SystemUser_IsActive_1]  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [dbo].[SystemUser] ADD  CONSTRAINT [DF_SystemUser_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO


