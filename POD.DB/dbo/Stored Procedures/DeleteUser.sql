-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DeleteUser] 
	-- Add the parameters for the stored procedure here
	(@UserID uniqueidentifier) 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from	
	UPDATE [User] SET IsActive = 0 WHERE ID = @UserID	
	UPDATE [UserRole] SET IsActive = 0 WHERE UserID = @UserID	

END