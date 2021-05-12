CREATE TABLE [dbo].[AspNetUsers] (
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
    [CreationUser]         VARCHAR (50)   NULL,
    CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.AspNetUsers_dbo.Contractor_ContractorID] FOREIGN KEY ([ContractorID]) REFERENCES [dbo].[Contractor] ([ContractorID]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.AspNetUsers_dbo.Customer_CustomerID] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customer] ([CustomerID]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ContractorID]
    ON [dbo].[AspNetUsers]([ContractorID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CustomerID]
    ON [dbo].[AspNetUsers]([CustomerID] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex]
    ON [dbo].[AspNetUsers]([UserName] ASC);

