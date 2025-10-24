IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_NAME = N'Table_106_POS') 
BEGIN
CREATE TABLE [dbo].[Table_106_POS](
	 [ColumnId] [smallint] NOT NULL,
	 [Column01] [nvarchar](50) NULL,
	 [Column02] [bit] NULL CONSTRAINT [DF_Table_106_POS_Column02]  DEFAULT ((0)),
 CONSTRAINT [PK_Table_106_POS] PRIMARY KEY CLUSTERED 
(
	 [ColumnId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

INSERT [dbo].[Table_106_POS] ([ColumnId], [Column01], [Column02]) VALUES (1, N'بانک صادرات', 0)
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'NameBank' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Table_106_POS', @level2type=N'COLUMN',@level2name=N'Column01'
END
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_NAME = N'Table_107_SettingPose') 
BEGIN
CREATE TABLE [dbo].[Table_107_SettingPose](
	 [ColumnId] [int] NOT NULL,
	 [Column01] [nvarchar](500) NULL,
	 [Column02] [nvarchar](500) NULL,
	 [Column03] [smallint] NULL,
 CONSTRAINT [PK_Table_107_SettingPose] PRIMARY KEY CLUSTERED 
(
	 [ColumnId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

INSERT [dbo].[Table_107_SettingPose] ([ColumnId], [Column01], [Column02], [Column03]) VALUES (1, N'شماره پایانه', N'', 1)
INSERT [dbo].[Table_107_SettingPose] ([ColumnId], [Column01], [Column02], [Column03]) VALUES (2, N'IP دستگاه', N'', 1)
INSERT [dbo].[Table_107_SettingPose] ([ColumnId], [Column01], [Column02], [Column03]) VALUES (3, N'Port دستگاه', N'', 1)
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Defulte' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Table_106_POS', @level2type=N'COLUMN',@level2name=N'Column02'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'عنوان' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Table_107_SettingPose', @level2type=N'COLUMN',@level2name=N'Column01'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'مقدار' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Table_107_SettingPose', @level2type=N'COLUMN',@level2name=N'Column02'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'بانک انتخاب شده' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Table_107_SettingPose', @level2type=N'COLUMN',@level2name=N'Column03'

END
Go