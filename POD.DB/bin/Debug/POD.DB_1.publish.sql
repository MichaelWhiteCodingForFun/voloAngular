﻿/*
Deployment script for POD

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "POD"
:setvar DefaultFilePrefix "POD"
:setvar DefaultDataPath "C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\"
:setvar DefaultLogPath "C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET ANSI_NULLS ON,
                ANSI_PADDING ON,
                ANSI_WARNINGS ON,
                ARITHABORT ON,
                CONCAT_NULL_YIELDS_NULL ON,
                QUOTED_IDENTIFIER ON,
                ANSI_NULL_DEFAULT ON,
                CURSOR_DEFAULT LOCAL 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET PAGE_VERIFY NONE 
            WITH ROLLBACK IMMEDIATE;
    END


GO
USE [$(DatabaseName)];


GO
/*
The column [dbo].[Role].[IsOwner] is being dropped, data loss could occur.
*/

IF EXISTS (select top 1 1 from [dbo].[Role])
    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT

GO
/*
The column [dbo].[User].[StatusID] on table [dbo].[User] must be added, but the column has no default value and does not allow NULL values. If the table contains data, the ALTER script will not work. To avoid this issue you must either: add a default value to the column, mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.
*/

IF EXISTS (select top 1 1 from [dbo].[User])
    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT

GO
PRINT N'Rename refactoring operation with key d0d3abac-d379-4e34-9d66-20137bd4d974 is skipped, element [dbo].[Organization].[AccountName] (SqlSimpleColumn) will not be renamed to AccountName';


GO
PRINT N'Rename refactoring operation with key 6408b870-4eb6-47d1-aed7-e58625f704e4 is skipped, element [dbo].[Organization].[AccountNumber] (SqlSimpleColumn) will not be renamed to AccountNumber';


GO
PRINT N'The following operation was generated from a refactoring log file 30a87109-d79b-46ae-9ff3-6ebb98964e15';

PRINT N'Rename [dbo].[Organization].[PayerAccountName] to AccountName';


GO
EXECUTE sp_rename @objname = N'[dbo].[Organization].[PayerAccountName]', @newname = N'AccountName', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file dd340e30-2bc7-42f9-889f-5d0e36010cbb';

PRINT N'Rename [dbo].[Organization].[PayerAccountNumber] to AccountNumber';


GO
EXECUTE sp_rename @objname = N'[dbo].[Organization].[PayerAccountNumber]', @newname = N'AccountNumber', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file 21b1b18a-7d54-4cd7-ab9e-454d55b6288c';

PRINT N'Rename [dbo].[Report].[CreatedByID] to UserID';


GO
EXECUTE sp_rename @objname = N'[dbo].[Report].[CreatedByID]', @newname = N'UserID', @objtype = N'COLUMN';


GO
PRINT N'Rename refactoring operation with key 3837f684-d2df-4564-bf0f-c36cae0ddbaf is skipped, element [dbo].[UserOrganization].[Id] (SqlSimpleColumn) will not be renamed to ID';


GO
PRINT N'The following operation was generated from a refactoring log file 22312a59-9bd2-45d2-af35-f5d7dae6ae70';

PRINT N'Rename [dbo].[Organization].[TypeID] to OrganizationTypeID';


GO
EXECUTE sp_rename @objname = N'[dbo].[Organization].[TypeID]', @newname = N'OrganizationTypeID', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file 7c7eedd7-c349-4897-93e0-0e8e7277a9b5';

PRINT N'Rename [dbo].[Organization].[Name] to OrganizationName';


GO
EXECUTE sp_rename @objname = N'[dbo].[Organization].[Name]', @newname = N'OrganizationName', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file aca45870-5ee6-431c-aaa5-869b384d8aa2';

PRINT N'Rename [dbo].[PK_UserOrganizationRole] to PK_UserRole';


GO
EXECUTE sp_rename @objname = N'[dbo].[PK_UserOrganizationRole]', @newname = N'PK_UserRole', @objtype = N'OBJECT';


GO
PRINT N'The following operation was generated from a refactoring log file 66143f4c-e2f7-4a91-b571-1a5a7a560a4c';

PRINT N'Rename [dbo].[FK_UserOrganizationRole_Organization] to FK_UserRole_Organization';


GO
EXECUTE sp_rename @objname = N'[dbo].[FK_UserOrganizationRole_Organization]', @newname = N'FK_UserRole_Organization', @objtype = N'OBJECT';


GO
PRINT N'The following operation was generated from a refactoring log file e36ff927-0e01-4613-951c-b312beb91b27';

PRINT N'Rename [dbo].[FK_UserOrganizationRole_Role] to FK_UserRole_Role';


GO
EXECUTE sp_rename @objname = N'[dbo].[FK_UserOrganizationRole_Role]', @newname = N'FK_UserRole_Role', @objtype = N'OBJECT';


GO
PRINT N'The following operation was generated from a refactoring log file db81f4e5-6fb3-49a2-8942-f87442d267b8';

PRINT N'Rename [dbo].[FK_UserOrganizationRole_User] to FK_UserRole_User';


GO
EXECUTE sp_rename @objname = N'[dbo].[FK_UserOrganizationRole_User]', @newname = N'FK_UserRole_User', @objtype = N'OBJECT';


GO
PRINT N'The following operation was generated from a refactoring log file 94b1c5d0-aefb-49cd-be13-8c57e5f3baa2';

PRINT N'Rename [dbo].[Role].[Name] to RoleName';


GO
EXECUTE sp_rename @objname = N'[dbo].[Role].[Name]', @newname = N'RoleName', @objtype = N'COLUMN';


GO
PRINT N'Rename refactoring operation with key ec73344c-d63f-47d3-9d44-2f18b4100040 is skipped, element [dbo].[FK_Report_ToTable] (SqlForeignKeyConstraint) will not be renamed to [FK_Report_User]';


GO
PRINT N'Altering [dbo].[Organization]...';


GO
ALTER TABLE [dbo].[Organization] ALTER COLUMN [AccountName] NVARCHAR (250) NULL;

ALTER TABLE [dbo].[Organization] ALTER COLUMN [AccountNumber] NVARCHAR (250) NULL;

ALTER TABLE [dbo].[Organization] ALTER COLUMN [OrganizationName] NVARCHAR (250) NOT NULL;


GO
PRINT N'Altering [dbo].[Report]...';


GO
ALTER TABLE [dbo].[Report] ALTER COLUMN [TicketID] UNIQUEIDENTIFIER NULL;


GO
ALTER TABLE [dbo].[Report]
    ADD [OrganizationID] UNIQUEIDENTIFIER NULL;


GO
PRINT N'Altering [dbo].[Role]...';


GO
ALTER TABLE [dbo].[Role] DROP COLUMN [IsOwner];


GO
PRINT N'Altering [dbo].[Ticket]...';


GO
ALTER TABLE [dbo].[Ticket] ALTER COLUMN [HaulierCode] NVARCHAR (250) NULL;

ALTER TABLE [dbo].[Ticket] ALTER COLUMN [HaulierReg] NVARCHAR (250) NULL;

ALTER TABLE [dbo].[Ticket] ALTER COLUMN [InvoiceNumber] NVARCHAR (250) NULL;

ALTER TABLE [dbo].[Ticket] ALTER COLUMN [PayerAccount] NVARCHAR (250) NULL;

ALTER TABLE [dbo].[Ticket] ALTER COLUMN [QuoteNumber] NVARCHAR (250) NULL;

ALTER TABLE [dbo].[Ticket] ALTER COLUMN [SerialNumber] NVARCHAR (250) NOT NULL;


GO
PRINT N'Altering [dbo].[User]...';


GO
ALTER TABLE [dbo].[User]
    ADD [StatusID] SMALLINT NOT NULL;


GO
PRINT N'Creating [dbo].[AccountStatus]...';


GO
CREATE TABLE [dbo].[AccountStatus] (
    [ID]       UNIQUEIDENTIFIER NOT NULL,
    [StatusID] SMALLINT         NOT NULL,
    [Status]   NVARCHAR (50)    NOT NULL
);


GO
PRINT N'Creating [dbo].[ReportType]...';


GO
CREATE TABLE [dbo].[ReportType] (
    [ID]             UNIQUEIDENTIFIER NOT NULL,
    [ReportTypeName] NVARCHAR (50)    NOT NULL,
    [ReportTypeID]   INT              NOT NULL,
    CONSTRAINT [PK_ReportType] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
PRINT N'Creating [dbo].[UserOrganization]...';


GO
CREATE TABLE [dbo].[UserOrganization] (
    [ID]             UNIQUEIDENTIFIER NOT NULL,
    [UserID]         UNIQUEIDENTIFIER NOT NULL,
    [OrganizationID] UNIQUEIDENTIFIER NOT NULL,
    [IsActive]       BIT              NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
PRINT N'Creating [dbo].[UserRole]...';


GO
CREATE TABLE [dbo].[UserRole] (
    [ID]        UNIQUEIDENTIFIER NOT NULL,
    [RoleID]    UNIQUEIDENTIFIER NOT NULL,
    [UserID]    UNIQUEIDENTIFIER NOT NULL,
    [IsPrimary] BIT              NULL,
    [IsActive]  BIT              NULL,
    CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
PRINT N'Creating [dbo].[FK_UserRole_Role]...';


GO
ALTER TABLE [dbo].[UserRole] WITH NOCHECK
    ADD CONSTRAINT [FK_UserRole_Role] FOREIGN KEY ([RoleID]) REFERENCES [dbo].[Role] ([ID]);


GO
PRINT N'Creating [dbo].[FK_UserRole_User]...';


GO
ALTER TABLE [dbo].[UserRole] WITH NOCHECK
    ADD CONSTRAINT [FK_UserRole_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]);


GO
PRINT N'Creating [dbo].[FK_Report_Organization]...';


GO
ALTER TABLE [dbo].[Report] WITH NOCHECK
    ADD CONSTRAINT [FK_Report_Organization] FOREIGN KEY ([OrganizationID]) REFERENCES [dbo].[Organization] ([ID]);


GO
PRINT N'Creating [dbo].[FK_Report_User]...';


GO
ALTER TABLE [dbo].[Report] WITH NOCHECK
    ADD CONSTRAINT [FK_Report_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]);


GO
PRINT N'Creating [dbo].[FK_User_User]...';


GO
ALTER TABLE [dbo].[User] WITH NOCHECK
    ADD CONSTRAINT [FK_User_User] FOREIGN KEY ([ID]) REFERENCES [dbo].[User] ([ID]);


GO
PRINT N'Altering [dbo].[GetOrganizationByID]...';


GO
ALTER PROCEDURE [dbo].[GetOrganizationByID]
	-- Add the parameters for the stored procedure here
	(@OrganizationID uniqueidentifier)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	 -- Insert statements for procedure here
  

SELECT 

o.ID, o.OrganizationName, o.AccountNumber, o.OrganizationTypeID,  
u.FullName, u.LastLoginDate, u.UserName, u.StatusID
from [Organization] as o
inner join [UserOrganization] uo ON uo.OrganizationID = o.ID and uo.IsActive = 1
inner join [User] u ON u.ID = uo.UserID and u.IsActive = 1
where o.ID = @OrganizationID and u.IsActive=1

END
GO
PRINT N'Altering [dbo].[GetOrganizationByName]...';


GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[GetOrganizationByName]
	-- Add the parameters for the stored procedure here
	(@OrganizationName  nvarchar(250)) 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT 

o.ID, o.OrganizationName, o.AccountNumber, o.OrganizationTypeID,  
u.FullName, u.LastLoginDate, u.UserName, u.StatusID
from [Organization] as o
inner join [UserOrganization] uo ON uo.OrganizationID = o.ID and uo.IsActive = 1
inner join [User] u ON u.ID = uo.UserID and u.IsActive = 1
where o.OrganizationName = @OrganizationName and u.IsActive=1
END
GO
PRINT N'Altering [dbo].[DeleteUser]...';


GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[DeleteUser] 
	-- Add the parameters for the stored procedure here
	(@UserID uniqueidentifier) 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from	
	UPDATE [User] SET IsActive = 0 WHERE ID = @UserID	
	UPDATE [UserRole] SET IsActive = 0 WHERE UserID = @UserID	

END
GO
PRINT N'Creating [dbo].[EditOrganization]...';


GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[EditOrganization] 
	-- Add the parameters for the stored procedure here
	(@CustomerID uniqueidentifier,@CustomerName nvarchar(50),@AdminID uniqueidentifier,@RoleID nvarchar(50)) 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	DECLARE @ID uniqueidentifier
	SET @ID = NEWID()

	UPDATE [Organization] SET OrganizationName = @CustomerName WHERE Id = @CustomerID
	--UPDATE [UserRole] SET IsPrimary = 0 WHERE OrganizationID = @CustomerID AND IsPrimary = 1	
	--INSERT INTO [UserRole](ID,OrganizationID,RoleID,UserID,IsPrimary) VALUES (@ID,@CustomerID,@RoleID,@AdminID,1 /* IsPrimary */)

END
GO
PRINT N'Creating [dbo].[GetOrganizations]...';


GO
CREATE PROCEDURE [dbo].[GetOrganizations] 
	-- Add the parameters for the stored procedure here
	(@ParentOrganizationID UNIQUEIDENTIFIER,
	 @AccountNumber nvarchar(250) = NULL,
	 @AccountName nvarchar(250) = NULL,
	 @AdminUser nvarchar(250) = NULL,
	 @UserName nvarchar(250) = NULL,
	 @StatusName nvarchar = NULL,
	 @LastLoginDate datetime = NULL,
	 @Offset INT,
	 @PageSize INT,
	 @SortDirection varchar(50),
	 @SortExpression varchar(50),
	 @RowCount INT OUTPUT)
AS
BEGIN

	    SET NOCOUNT ON;

		-- organizations --
SELECT  o.ID, o.OrganizationName, o.OrganizationTypeID, o.AccountNumber, o.IsActive, u.UserName, u.FullName, u.LastLoginDate, u.StatusID INTO #organization
		FROM  [Organization] o 
		inner join [UserOrganization] uo ON uo.OrganizationID = o.ID and uo.IsActive = 1
		inner join [User] u ON u.ID = uo.UserID
		inner join [UserRole] ur ON ur.UserID = u.ID and ur.IsActive = 1
		inner join [AccountStatus] s ON s.StatusID = u.StatusID

		WHERE o.ParentOrganizationID = @ParentOrganizationID AND o.IsActive=1 AND ur.IsPrimary = 1
		AND (@LastLoginDate IS NULL OR u.LastLoginDate = @LastLoginDate)
		AND (@AccountNumber IS NULL OR o.AccountNumber LIKE CONCAT('%', @AccountNumber, '%'))
		AND (@AccountName IS NULL OR o.OrganizationName LIKE CONCAT('%', @AccountName, '%'))
		AND (@AdminUser IS NULL OR u.FullName LIKE CONCAT('%', @AdminUser, '%'))
		AND (@UserName IS NULL OR u.UserName LIKE CONCAT('%', @UserName, '%'))
		AND (@StatusName IS NULL OR s.[Status] LIKE CONCAT('%', @StatusName, '%'))
		

SELECT  @RowCount = COUNT(*) 
		From #organization

 
SELECT  * from #organization
 ORDER BY 
		 CASE WHEN @SortDirection = 'asc' AND @SortExpression = 'OrganizationName' THEN OrganizationName END ASC, -- AccountName
		 CASE WHEN @SortDirection = 'asc' AND @SortExpression = 'AccountNumber' THEN AccountNumber END ASC, -- AccountNumber
		 CASE WHEN @SortDirection = 'asc' AND @SortExpression = 'Email' THEN UserName END ASC, -- Email Address
		 CASE WHEN @SortDirection = 'asc' AND @SortExpression = 'FullName' THEN FullName END ASC, -- Admin User
		 CASE WHEN @SortDirection = 'asc' AND @SortExpression = 'LastLogin' THEN LastLoginDate END ASC, -- Last Login 
		 CASE WHEN @SortDirection = 'asc' AND @SortExpression = 'StatusDisplayName' THEN StatusID END ASC, -- IsActive

		 CASE WHEN @SortDirection = 'desc' AND @SortExpression = 'OrganizationName' THEN OrganizationName END DESC, -- AccountName
		 CASE WHEN @SortDirection = 'desc' AND @SortExpression = 'AccountNumber' THEN AccountNumber END DESC, -- AccountNumber
		 CASE WHEN @SortDirection = 'desc' AND @SortExpression = 'Email' THEN UserName END DESC,-- Email Address
		 CASE WHEN @SortDirection = 'desc' AND @SortExpression = 'FullName' THEN FullName END DESC,-- Admin User
		 CASE WHEN @SortDirection = 'desc' AND @SortExpression = 'LastLogin' THEN LastLoginDate END DESC,-- Last Login 
		 CASE WHEN @SortDirection = 'desc' AND @SortExpression = 'StatusDisplayName' THEN StatusID END DESC-- IsActive
		 OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY


Drop table #organization
END
GO
PRINT N'Creating [dbo].[GetOrganizationsByParentID]...';


GO
CREATE PROCEDURE [dbo].[GetOrganizationsByParentID] 
	-- Add the parameters for the stored procedure here
	(@ParentOrganization UNIQUEIDENTIFIER)
AS
BEGIN

	SET NOCOUNT ON;	

		-- organizations --
	SELECT  org.ID, org.OrganizationName FROM [Organization] org WHERE org.ParentOrganizationID = @ParentOrganization AND org.IsActive = 1
	
END
GO
PRINT N'Creating [dbo].[GetReports]...';


GO
-- =============================================
-- Author:		Vardan
-- Create date: <Create Date,,>
-- Description:	Get list of reports
-- =============================================
CREATE PROCEDURE [dbo].[GetReports]	
	(
	@UserID uniqueidentifier,
	@ReportTypeID int = NULL,
	@Details varchar(50) = NULL,
	@Offset INT, 
	@PageSize INT,
	@SortDirection varchar(50) = 'ASC',
    @SortExpression varchar(50) = 'CreatedDateUtc', 
	@RowCount INT OUTPUT)	
AS
BEGIN

SET NOCOUNT ON;

 SELECT 
 r.ID, r.ReportTypeID, r.CreatedDateUtc, r.Details, r.IsSuccess,
 t.[TicketNumber],
 u.FullName
 INTO #reports
 FROM dbo.Report r 
 INNER JOIN dbo.Ticket t ON r.TicketID = t.ID
 INNER JOIN dbo.[User] u ON u.ID = @UserID
 INNER JOIN dbo.[UserRole] ur ON ur.UserID = u.ID
  INNER JOIN dbo.[UserOrganization] uo ON uo.UserID = u.ID
 INNER JOIN dbo.Organization o ON o.ID = uo.OrganizationID
 WHERE r.UserID = @UserID
 AND @ReportTypeID IS NULL OR r.ReportTypeID = @ReportTypeID
 AND @Details IS NULL OR r.Details LIKE '%' +  @Details + ' %'
 	SELECT   @RowCount = COUNT(*) 
		 From #reports


 
	SELECT * from #reports	
	ORDER BY 		 
		 CASE WHEN @SortDirection = 'asc' AND @SortExpression = 'CreatedDateUtc' THEN CreatedDateUtc END ASC,
		 -----------------------------------------------------------------------------------------
		 CASE WHEN @SortDirection = 'desc' AND @SortExpression = 'CreatedDateUtc' THEN CreatedDateUtc END DESC
		 OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY

	Drop table #reports

END
GO
PRINT N'Creating [dbo].[GetTickets]...';


GO
CREATE PROCEDURE [dbo].[GetTickets] 
	-- Add the parameters for the stored procedure here
	(@OrganizationID uniqueidentifier,
	@AccontNumber nvarchar(250) = NULL,
	@AccountName nvarchar(250) = NULL,
	@TicketNumber int = NULL,
	@DeliveryDate datetime = NULL,
	@InvoiceNumber nvarchar(250) = NULL,
	@Offset INT, 
	@PageSize INT,
	@SortDirection varchar(50) = 'ASC',
    @SortExpression varchar(50) = 'AccountName', 
	@RowCount INT OUTPUT)
	
AS
BEGIN

	    SET NOCOUNT ON;

		-- organizations --
SELECT  org.AccountNumber, org.AccountName, t.TicketNumber, t.DeliveryDate, t.InvoiceNumber INTO #tickets
		FROM  [Ticket] t 
		INNER JOIN  [Organization] org ON org.ID = t.OrganizationID AND org.IsActive = 1
		WHERE t.OrganizationID = @OrganizationID
		AND (@DeliveryDate IS NULL OR t.DeliveryDate = @DeliveryDate)
		AND (@TicketNumber IS NULL OR t.TicketNumber = @TicketNumber)
		AND (@AccontNumber IS NULL OR org.AccountNumber LIKE CONCAT('%', @AccontNumber, '%'))
		AND (@AccountName IS NULL OR org.AccountName LIKE CONCAT('%',  @AccountName, '%'))
		AND (@InvoiceNumber IS NULL OR t.InvoiceNumber LIKE CONCAT('%',  @InvoiceNumber, '%'))	

SELECT  @RowCount = COUNT(*) 
		From #tickets

		-- organizations --
	

 
SELECT  * from #tickets
 ORDER BY 
		 CASE WHEN @SortDirection = 'asc' AND @SortExpression = 'accountName' THEN AccountName END ASC, -- PayerAccount
		 CASE WHEN @SortDirection = 'asc' AND @SortExpression = 'accountNumber' THEN AccountNumber END ASC, -- AccountNumber
		 CASE WHEN @SortDirection = 'asc' AND @SortExpression = 'ticketNumber' THEN TicketNumber END ASC, -- Email Address
		 CASE WHEN @SortDirection = 'asc' AND @SortExpression = 'deliveryDate' THEN DeliveryDate END ASC, -- Admin User
		 CASE WHEN @SortDirection = 'asc' AND @SortExpression = 'invoiceNumber' THEN InvoiceNumber END ASC, -- Last Login 

		 CASE WHEN @SortDirection = 'desc' AND @SortExpression = 'accountName' THEN AccountName END DESC, -- PayerAccount
		 CASE WHEN @SortDirection = 'desc' AND @SortExpression = 'accountNumber' THEN AccountNumber END DESC, -- AccountNumber
		 CASE WHEN @SortDirection = 'desc' AND @SortExpression = 'ticketNumber' THEN TicketNumber END DESC,-- Email Address
		 CASE WHEN @SortDirection = 'desc' AND @SortExpression = 'deliveryDate' THEN DeliveryDate END DESC,-- Admin User
		 CASE WHEN @SortDirection = 'desc' AND @SortExpression = 'invoiceNumber' THEN InvoiceNumber END DESC-- Last Login 
		 OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY


Drop table #tickets
END
GO
PRINT N'Creating [dbo].[GetUserByID]...';


GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetUserByID]
	-- Add the parameters for the stored procedure here
	(@UserID uniqueidentifier)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT 

u.*,  
ur.RoleID, 
r.IsAdmin, r.RoleName, 
o.ID as OrganizationID, o.OrganizationName, o.OrganizationTypeID as OrganizationTypeID 
from [User] as u
inner join [UserRole] ur ON ur.UserID = u.ID and ur.IsActive = 1
inner join [Role] r ON r.ID = ur.RoleID
inner join [UserOrganization] uo ON uo.UserID = u.ID and uo.IsActive = 1
inner join [Organization] o ON o.ID = uo.OrganizationID
where u.ID = @UserID and u.IsActive=1


END
GO
PRINT N'Creating [dbo].[GetUserByUsername]...';


GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetUserByUsername]
	-- Add the parameters for the stored procedure here
	(@Username nvarchar(250))
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements .
	SET NOCOUNT ON;

SELECT 

u.*,  
ur.RoleID, 
r.IsAdmin, r.RoleName, 
o.ID as OrganizationID, o.OrganizationName, o.OrganizationTypeID as OrganizationTypeID 
from [User] as u
inner join [UserRole] ur ON ur.UserID = u.ID and ur.IsActive = 1
inner join [Role] r ON r.ID = ur.RoleID
inner join [UserOrganization] uo ON uo.UserID = u.ID and uo.IsActive = 1
inner join [Organization] o ON o.ID = uo.OrganizationID
where u.UserName = @Username and u.IsActive=1


END
GO
PRINT N'Creating [dbo].[GetUsers]...';


GO

-- =============================================
-- Author:		Anushaham Lavash
-- =============================================

CREATE PROCEDURE [dbo].[GetUsers]
	(@OrganizationID uniqueidentifier,
	 @OrganizationName nvarchar(250) = NULL,
	 @FullName nvarchar(250) = NULL,
	 @UserName nvarchar(250) = NULL,
	 @RoleName nvarchar = NULL,
	 @StatusName nvarchar = NULL,
	 @LastLoginDate datetime = NULL,
	 @Offset INT, 
	 @PageSize INT,
	 @SortDirection varchar(50),
	 @SortExpression varchar(50), 
	 @RowCount INT OUTPUT)
AS
BEGIN
	SET NOCOUNT ON;		 		 

	SELECT  u.ID, u.UserName, u.FullName, u.LastLoginDate, u.StatusID, u.IsActive,
	ur.IsPrimary,
	r.RoleName, r.ID as RoleID, r.IsAdmin, 
	o.OrganizationName, o.ID as OrganizationID, o.OrganizationTypeID as OrganizationTypeID INTO #users

	FROM  [User] as u
		inner join [UserRole] ur ON ur.UserID = u.ID and ur.IsActive = 1
		inner join [Role] r ON r.ID = ur.RoleID
		inner join [UserOrganization] uo ON uo.UserID = u.ID and uo.IsActive = 1
		inner join [Organization] o ON o.ID = uo.OrganizationID
		inner join [AccountStatus] s ON s.StatusID = u.StatusID
	WHERE uo.OrganizationID = @OrganizationID and u.IsActive=1
		AND (@LastLoginDate IS NULL OR u.LastLoginDate = @LastLoginDate)
		AND (@OrganizationName IS NULL OR o.OrganizationName LIKE CONCAT('%', @OrganizationName, '%'))
		AND (@FullName IS NULL OR u.FullName LIKE CONCAT('%', @FullName, '%'))
		AND (@UserName IS NULL OR u.UserName LIKE CONCAT('%', @UserName, '%'))
		AND (@RoleName IS NULL OR r.RoleName LIKE CONCAT('%', @RoleName, '%'))
		AND (@StatusName IS NULL OR s.[Status] LIKE CONCAT('%', @StatusName, '%'))

	SELECT   @RowCount = COUNT(*) 
	From #users

	SELECT * from #users 	
	ORDER BY 		 
		 CASE WHEN @SortDirection = 'asc' AND @SortExpression = 'User' THEN UserName END ASC,
		 CASE WHEN @SortDirection = 'asc' AND @SortExpression = 'FullName' THEN FullName END ASC,
		 CASE WHEN @SortDirection = 'asc' AND @SortExpression = 'OrganizationName' THEN OrganizationName END ASC,
		 CASE WHEN @SortDirection = 'asc' AND @SortExpression = 'RoleName' THEN RoleName END ASC,
		 CASE WHEN @SortDirection = 'asc' AND @SortExpression = 'LastLogin' THEN LastLoginDate END ASC,
		 CASE WHEN @SortDirection = 'asc' AND @SortExpression = 'StatusDisplayName' THEN StatusID END ASC,
		 -----------------------------------------------------------------------------------------
		 CASE WHEN @SortDirection = 'desc' AND @SortExpression = 'User' THEN UserName END DESC,
		 CASE WHEN @SortDirection = 'desc' AND @SortExpression = 'FullName' THEN FullName END DESC,
		 CASE WHEN @SortDirection = 'desc' AND @SortExpression = 'OrganizationName' THEN OrganizationName END DESC,
		 CASE WHEN @SortDirection = 'desc' AND @SortExpression = 'LastLogin' THEN LastLoginDate END DESC,
		 CASE WHEN @SortDirection = 'desc' AND @SortExpression = 'RoleName' THEN RoleName END DESC,
		 CASE WHEN @SortDirection = 'desc' AND @SortExpression = 'StatusDisplayName' THEN StatusID END DESC
		 OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY


	Drop table #users
	
END
GO
PRINT N'Creating [dbo].[UpdateUser]...';


GO
-- =============================================
-- Author:		Vardan Kosakyan
-- Create date: 07.06.16
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateUser] 
	
	@UserID uniqueidentifier,
	@FullName nvarchar(50),
	@RoleID uniqueidentifier,
	@IsPrimary bit,
	@StatusID int  
AS
BEGIN	
	BEGIN TRAN

	UPDATE dbo.[User] 
	SET FullName = @FullName, 
	StatusID = 
		CASE WHEN @StatusID = 2 THEN 
			(CASE WHEN [PasswordHash] IS NULL OR  [PasswordHash] = '' THEN 1 ELSE 2 END)
			ELSE @StatusID
		END
	WHERE Id = @userID

	IF @@ROWCOUNT > 0
	BEGIN
		UPDATE dbo.[UserRole]  SET RoleID = @RoleID, IsPrimary = @IsPrimary 
		WHERE UserId = @UserID 	
		COMMIT TRAN
	END ELSE
		ROLLBACK TRAN
END
GO
-- Refactoring step to update target server with deployed transaction logs

IF OBJECT_ID(N'dbo.__RefactorLog') IS NULL
BEGIN
    CREATE TABLE [dbo].[__RefactorLog] (OperationKey UNIQUEIDENTIFIER NOT NULL PRIMARY KEY)
    EXEC sp_addextendedproperty N'microsoft_database_tools_support', N'refactoring log', N'schema', N'dbo', N'table', N'__RefactorLog'
END
GO
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '6408b870-4eb6-47d1-aed7-e58625f704e4')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('6408b870-4eb6-47d1-aed7-e58625f704e4')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'd0d3abac-d379-4e34-9d66-20137bd4d974')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('d0d3abac-d379-4e34-9d66-20137bd4d974')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '30a87109-d79b-46ae-9ff3-6ebb98964e15')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('30a87109-d79b-46ae-9ff3-6ebb98964e15')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'dd340e30-2bc7-42f9-889f-5d0e36010cbb')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('dd340e30-2bc7-42f9-889f-5d0e36010cbb')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '21b1b18a-7d54-4cd7-ab9e-454d55b6288c')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('21b1b18a-7d54-4cd7-ab9e-454d55b6288c')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '3837f684-d2df-4564-bf0f-c36cae0ddbaf')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('3837f684-d2df-4564-bf0f-c36cae0ddbaf')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '22312a59-9bd2-45d2-af35-f5d7dae6ae70')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('22312a59-9bd2-45d2-af35-f5d7dae6ae70')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '7c7eedd7-c349-4897-93e0-0e8e7277a9b5')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('7c7eedd7-c349-4897-93e0-0e8e7277a9b5')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'aca45870-5ee6-431c-aaa5-869b384d8aa2')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('aca45870-5ee6-431c-aaa5-869b384d8aa2')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '66143f4c-e2f7-4a91-b571-1a5a7a560a4c')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('66143f4c-e2f7-4a91-b571-1a5a7a560a4c')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'e36ff927-0e01-4613-951c-b312beb91b27')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('e36ff927-0e01-4613-951c-b312beb91b27')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'db81f4e5-6fb3-49a2-8942-f87442d267b8')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('db81f4e5-6fb3-49a2-8942-f87442d267b8')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '94b1c5d0-aefb-49cd-be13-8c57e5f3baa2')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('94b1c5d0-aefb-49cd-be13-8c57e5f3baa2')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'ec73344c-d63f-47d3-9d44-2f18b4100040')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('ec73344c-d63f-47d3-9d44-2f18b4100040')

GO

GO
PRINT N'Checking existing data against newly created constraints';


GO
USE [$(DatabaseName)];


GO
ALTER TABLE [dbo].[UserRole] WITH CHECK CHECK CONSTRAINT [FK_UserRole_Role];

ALTER TABLE [dbo].[UserRole] WITH CHECK CHECK CONSTRAINT [FK_UserRole_User];

ALTER TABLE [dbo].[Report] WITH CHECK CHECK CONSTRAINT [FK_Report_Organization];

ALTER TABLE [dbo].[Report] WITH CHECK CHECK CONSTRAINT [FK_Report_User];

ALTER TABLE [dbo].[User] WITH CHECK CHECK CONSTRAINT [FK_User_User];


GO
PRINT N'Update complete.';


GO
