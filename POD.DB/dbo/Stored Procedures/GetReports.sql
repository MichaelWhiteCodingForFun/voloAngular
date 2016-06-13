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
