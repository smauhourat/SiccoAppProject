CREATE TABLE [dbo].[Log] (
    [Id]           INT            NOT NULL,
    [LogLevelId]   INT            NOT NULL,
    [ShortMessage] NVARCHAR (MAX) NOT NULL,
    [FullMessage]  NVARCHAR (MAX) NULL,
    [IpAddress]    NVARCHAR (200) NULL,
    [CustomerId]   INT            NULL,
    [PageUrl]      NVARCHAR (MAX) NULL,
    [ReferrerUrl]  NVARCHAR (MAX) NULL,
    [CreatedOnUtc] DATETIME       NOT NULL
);

