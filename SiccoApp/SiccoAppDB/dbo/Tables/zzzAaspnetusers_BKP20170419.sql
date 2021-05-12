CREATE TABLE [dbo].[zzzAaspnetusers_BKP20170419] (
    [Id]                   NVARCHAR (128) NOT NULL,
    [FirstName]            NVARCHAR (MAX) NOT NULL,
    [LastName]             NVARCHAR (MAX) NOT NULL,
    [Email]                NVARCHAR (256) NOT NULL,
    [EmailConfirmed]       BIT            NOT NULL,
    [PasswordHash]         NVARCHAR (MAX) NULL,
    [SecurityStamp]        NVARCHAR (MAX) NULL,
    [PhoneNumber]          NVARCHAR (MAX) NULL,
    [PhoneNumberConfirmed] BIT            NOT NULL,
    [TwoFactorEnabled]     BIT            NOT NULL,
    [LockoutEndDateUtc]    DATETIME       NULL,
    [LockoutEnabled]       BIT            NOT NULL,
    [AccessFailedCount]    INT            NOT NULL,
    [UserName]             NVARCHAR (256) NOT NULL,
    [Discriminator]        NVARCHAR (128) NOT NULL,
    [CustomerID]           INT            NULL,
    [ContractorID]         INT            NULL,
    [CreationDate]         DATETIME       NULL,
    [CreationUser]         VARCHAR (50)   NULL
);

