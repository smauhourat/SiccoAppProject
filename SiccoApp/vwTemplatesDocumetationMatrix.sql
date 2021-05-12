CREATE VIEW [dbo].[vwTemplatesDocumetationMatrix]
AS

	SELECT	
			BusinessTypeTemplate.BusinessTypeTemplateCode,
			BusinessTypeTemplate.Description AS BusinessTypeTemplate,
			DocumentationBusinessTypeTemplate.Importance,
			DocumentationPeriodicity.DocumentationPeriodicityID,
			DocumentationPeriodicity.Description AS DocumentationPeriodicity,
			DocumentationPeriodicity.DaysToDueDate,
			Documentation.DocumentationCode,
			Documentation.Description AS Documentation,
			EntityType.Description AS EntityType
	FROM
			BusinessTypeTemplate
			INNER JOIN DocumentationBusinessTypeTemplate ON BusinessTypeTemplate.BusinessTypeTemplateID = DocumentationBusinessTypeTemplate.BusinessTypeTemplateID
			INNER JOIN DocumentationPeriodicity ON DocumentationBusinessTypeTemplate.DocumentationPeriodicityID = DocumentationPeriodicity.DocumentationPeriodicityID
			INNER JOIN Documentation ON DocumentationBusinessTypeTemplate.DocumentationTemplateID = Documentation.DocumentationID 
			INNER JOIN EntityType ON Documentation.EntityTypeID = EntityType.EntityTypeID 

GO