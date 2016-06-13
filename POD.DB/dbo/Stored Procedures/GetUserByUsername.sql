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