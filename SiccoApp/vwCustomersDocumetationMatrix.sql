CREATE VIEW [dbo].[vwCustomersDocumetationMatrix]
AS

		SELECT
				Customer.CustomerID,
				Customer.CompanyName,
				BusinessType.BusinessTypeID,
				BusinessType.BusinessTypeCode,
				BusinessType.Description AS BusinessType,
				DocumentationBusinessType.DocumentationBusinessTypeID,
				DocumentationBusinessType.Importance,
				Documentation.DocumentationID,
				Documentation.DocumentationCode,
				DocumentationPeriodicity.DocumentationPeriodicityID,
				DocumentationPeriodicity.Description AS DocumentationPeriodicity,
				DocumentationPeriodicity.DaysToDueDate,
				Documentation.Description AS Documentation,
				EntityType.Description AS EntityType
		FROM
				BusinessType
				INNER JOIN DocumentationBusinessType ON BusinessType.BusinessTypeID = DocumentationBusinessType.BusinessTypeID
				INNER JOIN Documentation ON DocumentationBusinessType.DocumentationID = Documentation.DocumentationID
				INNER JOIN DocumentationPeriodicity ON DocumentationBusinessType.DocumentationPeriodicityID = DocumentationPeriodicity.DocumentationPeriodicityID
				INNER JOIN EntityType ON Documentation.EntityTypeID = EntityType.EntityTypeID
				INNER JOIN Customer ON BusinessType.CustomerID = Customer.CustomerID

GO


