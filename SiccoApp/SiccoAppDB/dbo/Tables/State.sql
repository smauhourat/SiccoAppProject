CREATE TABLE [dbo].[State] (
    [StateID]   INT          IDENTITY (1, 1) NOT NULL,
    [CountryID] INT          NOT NULL,
    [StateName] VARCHAR (50) NOT NULL,
    [StateCode] VARCHAR (10) NULL,
    CONSTRAINT [PK_State] PRIMARY KEY CLUSTERED ([StateID] ASC),
    CONSTRAINT [FK_State_Country] FOREIGN KEY ([CountryID]) REFERENCES [dbo].[Country] ([CountryID])
);

