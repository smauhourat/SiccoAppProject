CREATE TABLE [dbo].[DocumentationTemplate] (
    [DocumentationTemplateID]   INT           IDENTITY (1, 1) NOT NULL,
    [DocumentationTemplateCode] VARCHAR (20)  NOT NULL,
    [Description]               VARCHAR (150) NOT NULL,
    [EntityTypeID]              INT           NOT NULL,
    CONSTRAINT [PK_DocumentationTemplate] PRIMARY KEY CLUSTERED ([DocumentationTemplateID] ASC),
    CONSTRAINT [FK_DocumentationTemplate_EntityType] FOREIGN KEY ([EntityTypeID]) REFERENCES [dbo].[EntityType] ([EntityTypeID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'C=Contractor; E=Employee; V=Vehicle', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DocumentationTemplate', @level2type = N'COLUMN', @level2name = N'EntityTypeID';

