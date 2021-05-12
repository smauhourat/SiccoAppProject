CREATE TABLE [dbo].[CustomerAdminUser] (
    [CustomerAdminUserID] INT            IDENTITY (1, 1) NOT NULL,
    [UserID]              NVARCHAR (128) NOT NULL,
    [CustomerID]          INT            NOT NULL,
    CONSTRAINT [PK_CustomerAdminUser_1] PRIMARY KEY CLUSTERED ([CustomerAdminUserID] ASC),
    CONSTRAINT [FK_CustomerAdminUser_AspNetUsers] FOREIGN KEY ([UserID]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_CustomerAdminUser_Customer] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customer] ([CustomerID])
);

