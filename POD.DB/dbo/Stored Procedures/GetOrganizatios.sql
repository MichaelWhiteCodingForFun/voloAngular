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