CREATE DATABASE [todo]
GO
USE [todo]
GO
/****** Object:  Table [dbo].[tasks]    Script Date: 2/20/2017 9:07:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tasks](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[description] [varchar](255) NULL
) ON [PRIMARY]

GO
