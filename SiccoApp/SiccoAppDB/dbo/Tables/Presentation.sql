CREATE TABLE [dbo].[Presentation] (
    [PresentationID]     INT            IDENTITY (1, 1) NOT NULL,
    [RequirementID]      INT            NOT NULL,
    [PresentationStatus] INT            NOT NULL,
    [PresentationDate]   DATETIME       NOT NULL,
    [DocumentFiles]      VARCHAR (250)  NOT NULL,
    [TakenForID]         NVARCHAR (128) NULL,
    [TakenDate]          DATETIME       NULL,
    [ApprovedForID]      NVARCHAR (128) NULL,
    [ApprovedDate]       DATETIME       NULL,
    [RejectedForID]      NVARCHAR (128) NULL,
    [RejectedDate]       DATETIME       NULL,
    [Observations]       VARCHAR (1024) NULL,
    CONSTRAINT [PK_Presentation] PRIMARY KEY CLUSTERED ([PresentationID] ASC),
    CONSTRAINT [FK_Presentation_Requirement] FOREIGN KEY ([RequirementID]) REFERENCES [dbo].[Requirement] ([RequirementID])
);

