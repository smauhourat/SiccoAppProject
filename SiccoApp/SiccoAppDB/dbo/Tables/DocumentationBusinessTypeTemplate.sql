CREATE TABLE [dbo].[DocumentationBusinessTypeTemplate] (
    [DocumentationBusinessTypeTemplateID] INT IDENTITY (1, 1) NOT NULL,
    [DocumentationTemplateID]             INT NOT NULL,
    [BusinessTypeTemplateID]              INT NOT NULL,
    [Importance]                          INT NULL,
    [DocumentationImportanceID]           INT NULL,
    [DocumentationPeriodicityID]          INT NOT NULL,
    [RestrictAccess]                      BIT NULL,
    CONSTRAINT [PK_DocumentationBusinessTypeTemplate] PRIMARY KEY CLUSTERED ([DocumentationBusinessTypeTemplateID] ASC),
    CONSTRAINT [FK_DocumentationBusinessTypeTemplate_BusinessTypeTemplate] FOREIGN KEY ([BusinessTypeTemplateID]) REFERENCES [dbo].[BusinessTypeTemplate] ([BusinessTypeTemplateID]),
    CONSTRAINT [FK_DocumentationBusinessTypeTemplate_DocumentationImportance] FOREIGN KEY ([DocumentationImportanceID]) REFERENCES [dbo].[DocumentationImportance] ([DocumentationImportanceID]),
    CONSTRAINT [FK_DocumentationBusinessTypeTemplate_DocumentationPeriodicity] FOREIGN KEY ([DocumentationPeriodicityID]) REFERENCES [dbo].[DocumentationPeriodicity] ([DocumentationPeriodicityID]),
    CONSTRAINT [FK_DocumentationBusinessTypeTemplate_DocumentationTemplate] FOREIGN KEY ([DocumentationTemplateID]) REFERENCES [dbo].[DocumentationTemplate] ([DocumentationTemplateID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_DocumentationBusinessTypeTemplate_01]
    ON [dbo].[DocumentationBusinessTypeTemplate]([DocumentationTemplateID] ASC, [BusinessTypeTemplateID] ASC);

