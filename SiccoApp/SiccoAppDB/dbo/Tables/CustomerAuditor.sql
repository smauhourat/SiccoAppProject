CREATE TABLE [dbo].[CustomerAuditor] (
    [CustomerAuditorID] INT            IDENTITY (1, 1) NOT NULL,
    [CustomerID]        INT            NOT NULL,
    [UserId]            NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_CustomerAuditor] PRIMARY KEY CLUSTERED ([CustomerAuditorID] ASC),
    CONSTRAINT [FK_CustomerAuditor_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_CustomerAuditor_Customer] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customer] ([CustomerID])
);

