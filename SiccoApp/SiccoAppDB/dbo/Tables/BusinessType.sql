CREATE TABLE [dbo].[BusinessType] (
    [BusinessTypeID]         INT           IDENTITY (1, 1) NOT NULL,
    [BusinessTypeTemplateID] INT           NULL,
    [CustomerID]             INT           NOT NULL,
    [BusinessTypeCode]       VARCHAR (20)  NULL,
    [Description]            VARCHAR (150) NULL,
    CONSTRAINT [PK_BusinessType] PRIMARY KEY CLUSTERED ([BusinessTypeID] ASC),
    CONSTRAINT [FK_BusinessType_Customer] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customer] ([CustomerID])
);

