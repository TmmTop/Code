USE [WanboTuliao]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConsignmentInfo](
    [id] [int] IDENTITY(1,1) NOT NULL,
    [cpid] [nvarchar](50) NULL,
    [conid] [nvarchar](50) NULL,
    [name] [nvarchar](50) NULL,
    [batch] [nvarchar](50) NULL,
    [num] [int] NULL,
    [price] [money] NULL,
    [sumprice] [money] NULL,
    [norms] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
    [id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Consignment](
    [id] [int] IDENTITY(1,1) NOT NULL,
    [conid] [nvarchar](50) NULL,
    [sumprice] [money] NULL,
    [company] [nvarchar](50) NULL,
    [carnumber] [nvarchar](50) NULL,
    [remark] [nvarchar](max) NULL,
    [scdate] [nvarchar](50) NULL,
    [createtime] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
    [id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Consignment] ADD  CONSTRAINT [DF_Consignment_createtime]  DEFAULT (getdate()) FOR [createtime]
GO
