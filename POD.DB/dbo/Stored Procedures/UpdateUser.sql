-- =============================================
-- Author:		Vardan Kosakyan
-- Create date: 07.06.16
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateUser] 
	
	@UserID uniqueidentifier,
	@FullName nvarchar(50),
	@RoleID uniqueidentifier,
	@IsPrimary bit,
	@StatusID int  
AS
BEGIN	
	BEGIN TRAN

	UPDATE dbo.[User] 
	SET FullName = @FullName, 
	StatusID = 
		CASE WHEN @StatusID = 2 THEN 
			(CASE WHEN [PasswordHash] IS NULL OR  [PasswordHash] = '' THEN 1 ELSE 2 END)
			ELSE @StatusID
		END
	WHERE Id = @userID

	IF @@ROWCOUNT > 0
	BEGIN
		UPDATE dbo.[UserRole]  SET RoleID = @RoleID, IsPrimary = @IsPrimary 
		WHERE UserId = @UserID 	
		COMMIT TRAN
	END ELSE
		ROLLBACK TRAN
END