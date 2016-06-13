CREATE PROCEDURE [dbo].[GetOrganizationsByParentID] 
	-- Add the parameters for the stored procedure here
	(@ParentOrganization UNIQUEIDENTIFIER)
AS
BEGIN

	SET NOCOUNT ON;	

		-- organizations --
	SELECT  org.ID, org.OrganizationName FROM [Organization] org WHERE org.ParentOrganizationID = @ParentOrganization AND org.IsActive = 1
	
END