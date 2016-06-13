CREATE TABLE [dbo].[Organization] (
    [ID]                   UNIQUEIDENTIFIER NOT NULL,
    [OrganizationName]                 NVARCHAR (250)    NOT NULL,
    [CreatedByID]          UNIQUEIDENTIFIER NOT NULL,
    [CreatedDateUtc]       DATETIME         NOT NULL,
    [IsActive]             BIT              NOT NULL,
    [AccountNumber]   NVARCHAR (250)    NULL,
    [AccountName]     NVARCHAR (250)    NULL,
    [OrganizationTypeID]               INT              NULL,
    [ParentOrganizationID] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Organization] PRIMARY KEY CLUSTERED ([ID] ASC)
);

