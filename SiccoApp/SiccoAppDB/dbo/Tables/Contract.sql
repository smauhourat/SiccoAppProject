CREATE TABLE [dbo].[Contract] (
    [ContractID]       INT           IDENTITY (1, 1) NOT NULL,
    [ContractorID]     INT           NOT NULL,
    [CustomerID]       INT           NOT NULL,
    [ContractCode]     AS            ((('CON - '+replace(CONVERT([varchar],[startdate],(102)),'.',''))+'-')+CONVERT([varchar],[ContractID],(0))),
    [Description]      VARCHAR (250) NULL,
    [ContractStatusID] INT           NULL,
    [StartDate]        DATE          NULL,
    [EndDate2]         INT           NULL,
    [EndDate]          DATE          NULL,
    [Score]            INT           NULL,
    CONSTRAINT [PK_Contract] PRIMARY KEY CLUSTERED ([ContractID] ASC),
    CONSTRAINT [FK_Contract_Contractor] FOREIGN KEY ([ContractorID]) REFERENCES [dbo].[Contractor] ([ContractorID]),
    CONSTRAINT [FK_Contract_ContractStatus] FOREIGN KEY ([ContractStatusID]) REFERENCES [dbo].[ContractStatus] ([ContractStatusID]),
    CONSTRAINT [FK_Contract_Customer] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customer] ([CustomerID])
);

