CREATE TABLE [dbo].[zzz_AspNetUsers] (
    [Id]                      NVARCHAR (128) NOT NULL,
    [UserName]                NVARCHAR (MAX) NULL,
    [PasswordHash]            NVARCHAR (MAX) NULL,
    [SecurityStamp]           NVARCHAR (MAX) NULL,
    [FirstName]               NVARCHAR (MAX) NULL,
    [LastName]                NVARCHAR (MAX) NULL,
    [Email]                   NVARCHAR (MAX) NULL,
    [EmailConfirmed]          BIT            NOT NULL,
    [PhoneNumber]             NVARCHAR (MAX) NULL,
    [PhoneNumberConfirmed]    BIT            NOT NULL,
    [TwoFactorEnabled]        BIT            NOT NULL,
    [LockoutEndDateUtc]       DATETIME       NULL,
    [LockoutEnabled]          BIT            NOT NULL,
    [AccessFailedCount]       INT            NOT NULL,
    [Discriminator]           NVARCHAR (128) NULL,
    [customer_CustomerID]     INT            NULL,
    [contractor_ContractorID] INT            NULL
);

