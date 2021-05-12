CREATE TABLE [dbo].[CustomerAdmin] (
    [CustomerAdminID] INT            IDENTITY (1, 1) NOT NULL,
    [CustomerID]      INT            NOT NULL,
    [UserId]          NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_CustomerAdmin] PRIMARY KEY CLUSTERED ([CustomerAdminID] ASC),
    CONSTRAINT [FK_CustomerAdmin_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_CustomerAdmin_Customer] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customer] ([CustomerID])
);

