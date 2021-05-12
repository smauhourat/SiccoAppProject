CREATE TABLE [dbo].[Documentation] (
    [DocumentationID]         INT           IDENTITY (1, 1) NOT NULL,
    [DocumentationTemplateID] INT           NULL,
    [CustomerID]              INT           NOT NULL,
    [DocumentationCode]       VARCHAR (20)  NOT NULL,
    [Description]             VARCHAR (150) NOT NULL,
    [EntityTypeID]            INT           NOT NULL,
    CONSTRAINT [PK_Documentation] PRIMARY KEY CLUSTERED ([DocumentationID] ASC),
    CONSTRAINT [FK_Documentation_Customer] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customer] ([CustomerID]),
    CONSTRAINT [FK_Documentation_EntityType] FOREIGN KEY ([EntityTypeID]) REFERENCES [dbo].[EntityType] ([EntityTypeID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'C=Contractor; E=Employee; V=Vehicle', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Documentation', @level2type = N'COLUMN', @level2name = N'EntityTypeID';

