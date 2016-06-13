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