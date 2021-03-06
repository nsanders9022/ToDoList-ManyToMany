USE [todo]
GO
/****** Object:  Table [dbo].[categories]    Script Date: 2/27/2017 3:12:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[categories](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[categories_tasks]    Script Date: 2/27/2017 3:12:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[categories_tasks](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[category_id] [int] NULL,
	[task_id] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tasks]    Script Date: 2/27/2017 3:12:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tasks](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[description] [varchar](255) NULL,
	[duedate] [varchar](255) NULL,
	[completed] [tinyint] NULL
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[categories] ON 

INSERT [dbo].[categories] ([id], [name]) VALUES (1, N'House chores')
SET IDENTITY_INSERT [dbo].[categories] OFF
SET IDENTITY_INSERT [dbo].[categories_tasks] ON 

INSERT [dbo].[categories_tasks] ([id], [category_id], [task_id]) VALUES (1, 1, 1)
INSERT [dbo].[categories_tasks] ([id], [category_id], [task_id]) VALUES (2, 1, 2)
SET IDENTITY_INSERT [dbo].[categories_tasks] OFF
SET IDENTITY_INSERT [dbo].[tasks] ON 

INSERT [dbo].[tasks] ([id], [description], [duedate], [completed]) VALUES (10, N'Cleaning', N'2017-02-28', 1)
INSERT [dbo].[tasks] ([id], [description], [duedate], [completed]) VALUES (11, N'Dancing', N'2017-03-03', 0)
INSERT [dbo].[tasks] ([id], [description], [duedate], [completed]) VALUES (12, N'asdfasdf', N'2017-01-25', 0)
SET IDENTITY_INSERT [dbo].[tasks] OFF
