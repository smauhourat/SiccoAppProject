CREATE TABLE [dbo].[ScheduleTask] (
    [Id]                  INT            NOT NULL,
    [Name]                NVARCHAR (MAX) NOT NULL,
    [Seconds]             INT            NOT NULL,
    [Type]                NVARCHAR (MAX) NOT NULL,
    [Enabled]             BIT            NOT NULL,
    [StopOnError]         BIT            NOT NULL,
    [LeasedByMachineName] NVARCHAR (MAX) NULL,
    [LeasedUntilUtc]      DATETIME       NULL,
    [LastStartUtc]        DATETIME       NULL,
    [LastEndUtc]          DATETIME       NULL,
    [LastSuccessUtc]      DATETIME       NULL
);

