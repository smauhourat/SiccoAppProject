CREATE TABLE [dbo].[AppVersion] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [AppVersion]   VARCHAR (50)  NOT NULL,
    [DateVersion]  DATETIME      NOT NULL,
    [Observations] VARCHAR (500) NULL,
    CONSTRAINT [PK_AppVersion] PRIMARY KEY CLUSTERED ([Id] ASC)
);

