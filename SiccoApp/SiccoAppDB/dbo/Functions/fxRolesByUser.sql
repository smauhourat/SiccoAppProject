
CREATE FUNCTION fxRolesByUser 
(
	@UserId nvarchar(128)
)
RETURNS nvarchar(1024)
AS
BEGIN

	DECLARE @RolesCursor as CURSOR;
	DECLARE @RoleName NVARCHAR(256)
	DECLARE @RoleNameResult NVARCHAR(1024)
 
	SET @RolesCursor = CURSOR FORWARD_ONLY FOR

	SELECT 
		AspNetRoles.Name 
	FROM	
		AspNetUserRoles
		INNER JOIN AspNetRoles ON AspNetUserRoles.RoleId = AspNetRoles.Id 
	WHERE
		AspNetUserRoles.UserId = @UserId
 
	OPEN @RolesCursor;
	FETCH NEXT FROM @RolesCursor INTO @RoleName
	 WHILE @@FETCH_STATUS = 0
	BEGIN
		 SET @RoleNameResult = ISNULL(@RoleNameResult, '') + @RoleName + '; '

		 FETCH NEXT FROM @RolesCursor INTO @RoleName
	END
	CLOSE @RolesCursor;
	DEALLOCATE @RolesCursor;

	RETURN @RoleNameResult

END