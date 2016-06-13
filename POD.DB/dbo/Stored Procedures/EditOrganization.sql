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
