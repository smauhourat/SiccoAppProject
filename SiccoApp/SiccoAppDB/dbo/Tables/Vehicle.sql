CREATE TABLE [dbo].[Vehicle] (
    [VehicleID]            INT           IDENTITY (1, 1) NOT NULL,
    [ContractorID]         INT           NOT NULL,
    [IdentificationNumber] VARCHAR (50)  NOT NULL,
    [Description]          VARCHAR (250) NOT NULL,
    [Active]               TINYINT       NULL,
    [CreationDate]         DATETIME      NULL,
    [CreationUser]         VARCHAR (50)  NULL,
    [ModifiedDate]         DATETIME      NULL,
    [ModifiedUser]         VARCHAR (50)  NULL,
    [Disabled]             BIT           NULL,
    [DisabledDate]         DATETIME      NULL,
    CONSTRAINT [PK_Vehicle] PRIMARY KEY CLUSTERED ([VehicleID] ASC),
    CONSTRAINT [FK_Vehicle_Contractor] FOREIGN KEY ([ContractorID]) REFERENCES [dbo].[Contractor] ([ContractorID])
);

