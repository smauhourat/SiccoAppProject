CREATE TABLE [dbo].[EmailAccount] (
    [EmailAccountID]        INT            NOT NULL,
    [Email]                 NVARCHAR (255) NOT NULL,
    [DisplayName]           NVARCHAR (255) NULL,
    [Host]                  NVARCHAR (255) NOT NULL,
    [Port]                  INT            NOT NULL,
    [Username]              NVARCHAR (255) NOT NULL,
    [Password]              NVARCHAR (255) NOT NULL,
    [EnableSsl]             BIT            NOT NULL,
    [UseDefaultCredentials] BIT            NOT NULL,
    [IsDefault]             BIT            CONSTRAINT [DF_EmailAccount_IsDefault] DEFAULT ((0)) NOT NULL
);

