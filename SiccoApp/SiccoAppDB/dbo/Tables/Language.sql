CREATE TABLE [dbo].[Language] (
    [Id]                INT            NOT NULL,
    [Name]              NVARCHAR (100) NOT NULL,
    [LanguageCulture]   NVARCHAR (20)  NOT NULL,
    [UniqueSeoCode]     NVARCHAR (2)   NULL,
    [FlagImageFileName] NVARCHAR (50)  NULL,
    [Rtl]               BIT            NOT NULL,
    [LimitedToStores]   BIT            NOT NULL,
    [DefaultCurrencyId] INT            NOT NULL,
    [Published]         BIT            NOT NULL,
    [DisplayOrder]      INT            NOT NULL
);

