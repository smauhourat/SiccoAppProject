CREATE VIEW vwUsersWithRoles
AS

	SELECT 
			*, 
			dbo.fxRolesByUser(Id) AS Roles
	FROM 
			AspNetUsers