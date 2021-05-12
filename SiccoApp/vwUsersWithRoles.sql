CREATE VIEW [dbo].[vwUsersWithRoles]
AS

	SELECT 
			*, 
			dbo.fxRolesByUser(Id) AS Roles
	FROM 
			AspNetUsers 


GO