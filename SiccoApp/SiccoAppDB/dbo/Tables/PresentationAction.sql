CREATE TABLE [dbo].[PresentationAction] (
    [PresentationActionID]   INT            IDENTITY (1, 1) NOT NULL,
    [PresentationID]         INT            NOT NULL,
    [PresentationDate]       DATETIME       NOT NULL,
    [ActionForID]            NVARCHAR (128) NULL,
    [PresentationActionType] INT            NOT NULL,
    CONSTRAINT [PK_PresentationAction] PRIMARY KEY CLUSTERED ([PresentationActionID] ASC),
    CONSTRAINT [FK_PresentationAction_Presentation1] FOREIGN KEY ([PresentationID]) REFERENCES [dbo].[Presentation] ([PresentationID])
);

