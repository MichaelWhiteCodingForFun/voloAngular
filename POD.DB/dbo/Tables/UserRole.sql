CREATE TABLE [dbo].[UserRole] (
    [ID]             UNIQUEIDENTIFIER NOT NULL,
    [RoleID]         UNIQUEIDENTIFIER NOT NULL,
    [UserID]         UNIQUEIDENTIFIER NOT NULL,
    [IsPrimary]      BIT              NULL,
    [IsActive]       BIT              NULL,
    CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_UserRole_Role] FOREIGN KEY ([RoleID]) REFERENCES [dbo].[Role] ([ID]),
    CONSTRAINT [FK_UserRole_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

