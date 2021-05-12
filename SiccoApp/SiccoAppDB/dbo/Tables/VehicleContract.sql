CREATE TABLE [dbo].[VehicleContract] (
    [VehicleContractID] INT IDENTITY (1, 1) NOT NULL,
    [VehicleID]         INT NOT NULL,
    [ContractID]        INT NOT NULL,
    CONSTRAINT [PK_VehicleContract] PRIMARY KEY CLUSTERED ([VehicleContractID] ASC),
    CONSTRAINT [FK_VehicleContract_Contract] FOREIGN KEY ([ContractID]) REFERENCES [dbo].[Contract] ([ContractID]),
    CONSTRAINT [FK_VehicleContract_Vehicle] FOREIGN KEY ([VehicleID]) REFERENCES [dbo].[Vehicle] ([VehicleID])
);

