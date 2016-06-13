CREATE TABLE [dbo].[Role] (
    [ID]          UNIQUEIDENTIFIER NOT NULL,
    [RoleName]        NVARCHAR (50)    NOT NULL,
    [Description] NVARCHAR (250)   NULL,
    [IsAdmin]     BIT              NULL,
    CONSTRAINT [PK_Role_1] PRIMARY KEY CLUSTERED ([ID] ASC)
);

