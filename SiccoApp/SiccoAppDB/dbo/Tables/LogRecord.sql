CREATE TABLE [dbo].[LogRecord] (
    [LogRecordID]  INT            IDENTITY (1, 1) NOT NULL,
    [LogLevelId]   INT            NOT NULL,
    [ShortMessage] NVARCHAR (MAX) NOT NULL,
    [FullMessage]  NVARCHAR (MAX) NULL,
    [IpAddress]    NVARCHAR (200) NULL,
    [PageUrl]      NVARCHAR (MAX) NULL,
    [ReferrerUrl]  NVARCHAR (MAX) NULL,
    [CreatedOnUtc] DATETIME       NOT NULL,
    CONSTRAINT [PK_LogRecord] PRIMARY KEY CLUSTERED ([LogRecordID] ASC)
);

