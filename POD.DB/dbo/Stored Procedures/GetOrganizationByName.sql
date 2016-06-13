-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetOrganizationByName]
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
