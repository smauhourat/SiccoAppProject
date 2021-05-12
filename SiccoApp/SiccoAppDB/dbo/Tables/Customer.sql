CREATE TABLE [dbo].[Customer] (
    [CustomerID]   INT           IDENTITY (1, 1) NOT NULL,
    [CompanyName]  VARCHAR (150) NULL,
    [TaxIdNumber]  VARCHAR (20)  NULL,
    [CountryID]    INT           NULL,
    [StateID]      INT           NULL,
    [City]         VARCHAR (150) NULL,
    [Address]      VARCHAR (150) NULL,
    [PhoneNumber]  VARCHAR (20)  NULL,
    [Active]       TINYINT       NULL,
    [CreationDate] DATETIME      NULL,
    [CreationUser] VARCHAR (50)  NULL,
    [ModifiedDate] DATETIME      NULL,
    [ModifiedUser] VARCHAR (50)  NULL,
    CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED ([CustomerID] ASC),
    CONSTRAINT [FK_Customer_State] FOREIGN KEY ([StateID]) REFERENCES [dbo].[State] ([StateID])
);

