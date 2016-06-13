CREATE PROCEDURE [dbo].[GetOrganizationByID]
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