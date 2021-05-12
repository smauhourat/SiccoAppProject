CREATE TABLE [dbo].[EmployeeRelationshipType] (
    [EmployeeRelationshipTypeID] INT          NOT NULL,
    [Description]                VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_EmployeeRelationshipType] PRIMARY KEY CLUSTERED ([EmployeeRelationshipTypeID] ASC)
);

