CREATE TABLE [dbo].[DocumentationBusinessType] (
    [DocumentationBusinessTypeID]         INT IDENTITY (1, 1) NOT NULL,
    [DocumentationBusinessTypeTemplateID] INT NULL,
    [DocumentationID]                     INT NOT NULL,
    [BusinessTypeID]                      INT NOT NULL,
    [Importance]                          INT NULL,
    [DocumentationImportanceID]           INT NULL,
    [DocumentationPeriodicityID]          INT NOT NULL,
    [RestrictAccess]                      BIT NULL,
    CONSTRAINT [PK_DocumentationBusinessType_1] PRIMARY KEY CLUSTERED ([DocumentationBusinessTypeID] ASC),
    CONSTRAINT [FK_DocumentationBusinessType_BusinessType] FOREIGN KEY ([BusinessTypeID]) REFERENCES [dbo].[BusinessType] ([BusinessTypeID]),
    CONSTRAINT [FK_DocumentationBusinessType_Documentation] FOREIGN KEY ([DocumentationID]) REFERENCES [dbo].[Documentation] ([DocumentationID]),
    CONSTRAINT [FK_DocumentationBusinessType_DocumentationImportance] FOREIGN KEY ([DocumentationImportanceID]) REFERENCES [dbo].[DocumentationImportance] ([DocumentationImportanceID]),
    CONSTRAINT [FK_DocumentationBusinessType_DocumentationPeriodicity] FOREIGN KEY ([DocumentationPeriodicityID]) REFERENCES [dbo].[DocumentationPeriodicity] ([DocumentationPeriodicityID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_DocumentationBusinessType_01]
    ON [dbo].[DocumentationBusinessType]([DocumentationID] ASC, [BusinessTypeID] ASC);

