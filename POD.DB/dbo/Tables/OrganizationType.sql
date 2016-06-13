CREATE TABLE [dbo].[OrganizationType] (
    [ID]                   UNIQUEIDENTIFIER NOT NULL,
    [OrganizationTypeName] NVARCHAR (50)    NOT NULL,
    [OrganizationTypeID]   INT              NULL,
    CONSTRAINT [PK_OrganizationType] PRIMARY KEY CLUSTERED ([ID] ASC)
);

