CREATE TABLE [dbo].[Requirement] (
    [RequirementID]               INT  IDENTITY (1, 1) NOT NULL,
    [DocumentationBusinessTypeID] INT  NOT NULL,
    [ContractID]                  INT  NOT NULL,
    [EmployeeContractID]          INT  NULL,
    [VehicleContractID]           INT  NULL,
    [PeriodID]                    INT  NOT NULL,
    [RequirementStatus]           INT  NOT NULL,
    [DueDate]                     DATE NOT NULL,
    CONSTRAINT [PK_Requirement] PRIMARY KEY CLUSTERED ([RequirementID] ASC),
    CONSTRAINT [FK_Requirement_Contract] FOREIGN KEY ([ContractID]) REFERENCES [dbo].[Contract] ([ContractID]),
    CONSTRAINT [FK_Requirement_DocumentationBusinessType] FOREIGN KEY ([DocumentationBusinessTypeID]) REFERENCES [dbo].[DocumentationBusinessType] ([DocumentationBusinessTypeID]),
    CONSTRAINT [FK_Requirement_EmployeeContract] FOREIGN KEY ([EmployeeContractID]) REFERENCES [dbo].[EmployeeContract] ([EmployeeContractID]),
    CONSTRAINT [FK_Requirement_VehicleContract] FOREIGN KEY ([VehicleContractID]) REFERENCES [dbo].[VehicleContract] ([VehicleContractID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Requirement_01]
    ON [dbo].[Requirement]([DocumentationBusinessTypeID] ASC, [ContractID] ASC, [EmployeeContractID] ASC, [VehicleContractID] ASC, [PeriodID] ASC);

