CREATE TABLE [dbo].[zzzCustomers] (
    [CustomerID]   INT            IDENTITY (1, 1) NOT NULL,
    [CompanyName]  NVARCHAR (150) NOT NULL,
    [TaxIdNumber]  NVARCHAR (20)  NOT NULL,
    [City]         NVARCHAR (MAX) NULL,
    [Address]      NVARCHAR (MAX) NULL,
    [PhoneNumber]  NVARCHAR (20)  NULL,
    [Active]       TINYINT        NULL,
    [CreationDate] DATETIME       NULL,
    [CreationUser] NVARCHAR (MAX) NULL,
    [ModifiedDate] DATETIME       NULL,
    [ModifiedUser] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.Customers] PRIMARY KEY CLUSTERED ([CustomerID] ASC)
);

