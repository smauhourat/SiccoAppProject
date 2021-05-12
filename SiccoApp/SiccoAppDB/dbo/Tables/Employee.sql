CREATE TABLE [dbo].[Employee] (
    [EmployeeID]                 INT           IDENTITY (1, 1) NOT NULL,
    [ContractorID]               INT           NOT NULL,
    [IdentificationNumberTypeID] INT           NULL,
    [IdentificationNumber]       VARCHAR (20)  NULL,
    [SocialSecurityNumber]       VARCHAR (20)  NULL,
    [EmployeeRelationshipTypeID] INT           NULL,
    [FirstName]                  VARCHAR (50)  NOT NULL,
    [LastName]                   VARCHAR (50)  NOT NULL,
    [Email]                      VARCHAR (250) NULL,
    [MaritalStatus]              CHAR (1)      NULL,
    [Gender]                     CHAR (1)      NULL,
    [BirthDate]                  DATE          NULL,
    [CountryID]                  INT           NULL,
    [StateID]                    INT           NULL,
    [City]                       VARCHAR (150) NULL,
    [Address]                    VARCHAR (150) NULL,
    [PhoneNumber]                VARCHAR (20)  NULL,
    [Active]                     TINYINT       NULL,
    [CreationDate]               DATETIME      CONSTRAINT [DF_Employee_CreationDate] DEFAULT (getdate()) NULL,
    [CreationUser]               VARCHAR (50)  NULL,
    [ModifiedDate]               DATETIME      NULL,
    [ModifiedUser]               VARCHAR (50)  NULL,
    [LeavingDate]                DATETIME      NULL,
    [LeavingUser]                VARCHAR (50)  NULL,
    [DriverLicense]              TINYINT       NULL,
    [DriverLicenseExpiration]    DATE          NULL,
    [Disabled]                   BIT           NULL,
    [DisabledDate]               DATETIME      NULL,
    CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED ([EmployeeID] ASC),
    CONSTRAINT [FK_Employee_Contractor] FOREIGN KEY ([ContractorID]) REFERENCES [dbo].[Contractor] ([ContractorID]),
    CONSTRAINT [FK_Employee_EmployeeRelationshipType] FOREIGN KEY ([EmployeeRelationshipTypeID]) REFERENCES [dbo].[EmployeeRelationshipType] ([EmployeeRelationshipTypeID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'M = Male, F = Female', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Employee', @level2type = N'COLUMN', @level2name = N'Gender';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'M = Married, S = Single', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Employee', @level2type = N'COLUMN', @level2name = N'MaritalStatus';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nro de Documento', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Employee', @level2type = N'COLUMN', @level2name = N'IdentificationNumber';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tipo Documento', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Employee', @level2type = N'COLUMN', @level2name = N'IdentificationNumberTypeID';

