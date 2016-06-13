
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