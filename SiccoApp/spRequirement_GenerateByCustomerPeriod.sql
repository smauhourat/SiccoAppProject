--[dbo].[spRequirement_GenerateByCustomerPeriod]
DECLARE @CustomerID INT, @PeriodID INT

SET @CustomerID = 1
SET @PeriodID = 202002

	SELECT
			DISTINCT
			Contract.ContractID,
			DocumentationBusinessType.DocumentationBusinessTypeID,
			NULL AS EmployeeContractID,
			NULL AS VehicleContractID,
			@PeriodID AS PeriodID,
			2 AS RequirementStatus, -- PENDING

			GETDATE() AS DueDate 
	FROM
			Contract
			INNER JOIN Contractor ON Contract.ContractorID = Contractor.ContractorID
			INNER JOIN BusinessType ON Contractor.BusinessTypeID = BusinessType.BusinessTypeID AND BusinessType.CustomerID = @CustomerID
			INNER JOIN DocumentationBusinessType ON BusinessType.BusinessTypeID = DocumentationBusinessType.BusinessTypeID 
			INNER JOIN Documentation ON DocumentationBusinessType.DocumentationID = Documentation.DocumentationID AND Documentation.CustomerID = @CustomerID AND Documentation.EntityTypeID = 1 -- CONTRACTOR
			INNER JOIN DocumentationPeriodicity ON Documentation.DocumentationPeriodicityID = DocumentationPeriodicity.DocumentationPeriodicityID
