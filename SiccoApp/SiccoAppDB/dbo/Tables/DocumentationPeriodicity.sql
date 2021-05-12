CREATE TABLE [dbo].[DocumentationPeriodicity] (
    [DocumentationPeriodicityID] INT          NOT NULL,
    [Description]                VARCHAR (50) NOT NULL,
    [DaysToDueDate]              INT          NULL,
    CONSTRAINT [PK_DocumentationPeriodicity_1] PRIMARY KEY CLUSTERED ([DocumentationPeriodicityID] ASC)
);

