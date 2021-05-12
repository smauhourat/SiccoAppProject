CREATE TABLE [dbo].[BusinessTypeTemplate] (
    [BusinessTypeTemplateID]   INT           IDENTITY (1, 1) NOT NULL,
    [BusinessTypeTemplateCode] VARCHAR (20)  NULL,
    [Description]              VARCHAR (150) NULL,
    CONSTRAINT [PK_BusinessTypeTemplate] PRIMARY KEY CLUSTERED ([BusinessTypeTemplateID] ASC)
);

