CREATE TABLE [dbo].[Contractor] (
    [ContractorID]         INT           IDENTITY (1, 1) NOT NULL,
    [CustomerID]           INT           NOT NULL,
    [BusinessTypeID]       INT           NULL,
    [CompanyName]          VARCHAR (150) NULL,
    [TaxIdNumber]          VARCHAR (20)  NULL,
    [CountryID]            INT           NULL,
    [StateID]              INT           NULL,
    [City]                 VARCHAR (150) NULL,
    [Address]              VARCHAR (150) NULL,
    [PhoneNumber]          VARCHAR (20)  NULL,
    [EmergencyPhoneNumber] VARCHAR (20)  NULL,
    [Email]                VARCHAR (100) NULL,
    [Active]               TINYINT       NULL,
    [CreationDate]         DATETIME      CONSTRAINT [DF_Contractor_CreationDate] DEFAULT (getdate()) NULL,
    [CreationUser]         VARCHAR (50)  NULL,
    [ModifiedDate]         DATETIME      NULL,
    [ModifiedUser]         VARCHAR (50)  NULL,
    CONSTRAINT [PK_Contractor] PRIMARY KEY CLUSTERED ([ContractorID] ASC),
    CONSTRAINT [FK_Contractor_BusinessType] FOREIGN KEY ([BusinessTypeID]) REFERENCES [dbo].[BusinessType] ([BusinessTypeID]),
    CONSTRAINT [FK_Contractor_Customer] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customer] ([CustomerID]),
    CONSTRAINT [FK_Contractor_State] FOREIGN KEY ([StateID]) REFERENCES [dbo].[State] ([StateID])
);

