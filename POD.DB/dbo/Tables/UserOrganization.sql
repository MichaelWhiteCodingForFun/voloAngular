﻿CREATE TABLE [dbo].[UserOrganization]
(
	[ID] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [UserID] UNIQUEIDENTIFIER NOT NULL, 
    [OrganizationID] UNIQUEIDENTIFIER NOT NULL, 
    [IsActive] BIT NOT NULL
)
