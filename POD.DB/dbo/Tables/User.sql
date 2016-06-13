CREATE TABLE [dbo].[User] (
    [ID]                UNIQUEIDENTIFIER NOT NULL,
    [UserName]          NVARCHAR (250)   NOT NULL,
    [FullName]          NVARCHAR (250)   NOT NULL,
    [IsActive]          BIT              NOT NULL,
    [CreatedDateUtc]    DATETIME         NOT NULL,
    [SecurityStamp]     NVARCHAR (500)   NULL,
    [LockoutEnabled]    BIT              NOT NULL,
    [AccessFailedCount] INT              NOT NULL,
    [LockoutEndDateUtc] DATETIME         NULL,
    [PasswordHash]      NVARCHAR (250)   NULL,
    [CreatedByID]       UNIQUEIDENTIFIER NOT NULL,
    [LastLoginDate]     DATETIME         NULL,
    [StatusID]          SMALLINT         NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_User_User] FOREIGN KEY ([ID]) REFERENCES [dbo].[User] ([ID])
);

