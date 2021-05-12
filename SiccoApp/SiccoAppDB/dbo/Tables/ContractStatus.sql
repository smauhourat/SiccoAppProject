CREATE TABLE [dbo].[ContractStatus] (
    [ContractStatusID] INT          NOT NULL,
    [Description]      VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_ContractStatus] PRIMARY KEY CLUSTERED ([ContractStatusID] ASC)
);

