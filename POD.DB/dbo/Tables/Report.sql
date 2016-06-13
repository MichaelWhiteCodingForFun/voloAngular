CREATE TABLE [dbo].[Report] (
    [ID]             UNIQUEIDENTIFIER NOT NULL,
    [ReportTypeID]   INT              NOT NULL,
    [CreatedDateUtc] DATETIME         NOT NULL,
    [Details]        NVARCHAR (250)   NOT NULL,
    [UserID]    UNIQUEIDENTIFIER NOT NULL,
    [IsSuccess]      BIT              NULL,
    [TicketID]       UNIQUEIDENTIFIER NULL,
    [OrganizationID] UNIQUEIDENTIFIER NULL, 
    CONSTRAINT [PK_Report_1] PRIMARY KEY CLUSTERED ([ID] ASC), 
    CONSTRAINT [FK_Report_User] FOREIGN KEY ([UserID]) REFERENCES [User]([ID]), 
    CONSTRAINT [FK_Report_Organization] FOREIGN KEY ([OrganizationID]) REFERENCES [Organization]([ID])
);

