CREATE TABLE [dbo].[EmployeeContract] (
    [EmployeeContractID] INT IDENTITY (1, 1) NOT NULL,
    [EmployeeID]         INT NOT NULL,
    [ContractID]         INT NOT NULL,
    CONSTRAINT [PK_EmployeeContract] PRIMARY KEY CLUSTERED ([EmployeeContractID] ASC),
    CONSTRAINT [FK_EmployeeContract_Contract] FOREIGN KEY ([ContractID]) REFERENCES [dbo].[Contract] ([ContractID]),
    CONSTRAINT [FK_EmployeeContract_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employee] ([EmployeeID])
);

