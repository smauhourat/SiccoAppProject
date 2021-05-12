CREATE VIEW [dbo].[vwDocumentations]
AS

		select
				NEWID() as Id,
				BusinessType.CustomerID,
				Contractor.ContractorID,
				BusinessType.BusinessTypeCode,
				BusinessType.Description AS BusinessTypeDescription,
				Documentation.DocumentationCode,
				Documentation.Description as DocumentationDescription,
				EntityType.Description as EntityTypeDescription,
				DocumentationPeriodicity.Description as DocumentationPeriodicityDescription
		from
				BusinessType
				inner join DocumentationBusinessType on BusinessType.BusinessTypeID = DocumentationBusinessType.BusinessTypeID
				inner join Documentation on DocumentationBusinessType.DocumentationID = Documentation.DocumentationID
				inner join DocumentationPeriodicity on DocumentationBusinessType.DocumentationPeriodicityID = DocumentationPeriodicity.DocumentationPeriodicityID
				inner join EntityType on Documentation.EntityTypeID = EntityType.EntityTypeID
				inner join Contractor on BusinessType.BusinessTypeID = Contractor.BusinessTypeID