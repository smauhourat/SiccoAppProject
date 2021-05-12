CREATE TABLE [dbo].[Country] (
    [CountryID]   INT          IDENTITY (1, 1) NOT NULL,
    [CountryName] VARCHAR (50) NOT NULL,
    [CountryCode] VARCHAR (10) NULL,
    CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED ([CountryID] ASC)
);

