CREATE VIEW [dbo].[vwRequirements]
AS

		select
				[Contract].CustomerID,
				Contractor.ContractorID,
				Requirement.ContractID,
				Requirement.RequirementID,
				Documentation.DocumentationCode,
				Documentation.Description,

				'CONTRACTOR' AS ResourceType,
				Contractor.CompanyName AS ResourceIdentification,

				Requirement.PeriodID,
				RequirementStatus.RequirementStatusDescription,
				RequirementStatus.RequirementStatusSummary,
				Requirement.DueDate
		from
				Requirement
				inner join DocumentationBusinessType on Requirement.DocumentationBusinessTypeID = DocumentationBusinessType.DocumentationBusinessTypeID
				inner join Documentation on DocumentationBusinessType.DocumentationID = Documentation.DocumentationID
				inner join RequirementStatus on Requirement.RequirementStatus = RequirementStatus.RequirementStatus
				inner join [Contract] on Requirement.ContractID = [Contract].ContractID
				inner join Contractor on [Contract].ContractorID = Contractor.ContractorID
		where
				Requirement.EmployeeContractID is null and Requirement.VehicleContractID is null

		union

		select
				[Contract].CustomerID,
				Contractor.ContractorID,
				Requirement.ContractID,
				Requirement.RequirementID,
				Documentation.DocumentationCode,
				Documentation.Description,

				'EMPLOYEE' AS ResourceType,
				(Employee.LastName + ', ' + Employee.FirstName) AS ResourceIdentification,

				Requirement.PeriodID,
				RequirementStatus.RequirementStatusDescription,
				RequirementStatus.RequirementStatusSummary,
				Requirement.DueDate
		from
				Requirement
				inner join DocumentationBusinessType on Requirement.DocumentationBusinessTypeID = DocumentationBusinessType.DocumentationBusinessTypeID
				inner join Documentation on DocumentationBusinessType.DocumentationID = Documentation.DocumentationID
				inner join RequirementStatus on Requirement.RequirementStatus = RequirementStatus.RequirementStatus
				inner join EmployeeContract on Requirement.EmployeeContractID = EmployeeContract.EmployeeContractID  
				inner join Employee on EmployeeContract.EmployeeID = Employee.EmployeeID
				inner join [Contract] on Requirement.ContractID = [Contract].ContractID
				inner join Contractor on [Contract].ContractorID = Contractor.ContractorID
		where
				not Requirement.EmployeeContractID is null

		union

		select
				[Contract].CustomerID,
				Contractor.ContractorID,
				Requirement.ContractID,
				Requirement.RequirementID,
				Documentation.DocumentationCode,
				Documentation.Description,

				'VEHICLE' AS ResourceType,
				Vehicle.IdentificationNumber AS ResourceIdentification,

				Requirement.PeriodID,
				RequirementStatus.RequirementStatusDescription,
				RequirementStatus.RequirementStatusSummary,
				Requirement.DueDate
		from
				Requirement
				inner join DocumentationBusinessType on Requirement.DocumentationBusinessTypeID = DocumentationBusinessType.DocumentationBusinessTypeID
				inner join Documentation on DocumentationBusinessType.DocumentationID = Documentation.DocumentationID
				inner join RequirementStatus on Requirement.RequirementStatus = RequirementStatus.RequirementStatus
				inner join VehicleContract on Requirement.VehicleContractID = VehicleContract.VehicleContractID  
				inner join Vehicle on VehicleContract.VehicleID = Vehicle.VehicleID
				inner join [Contract] on Requirement.ContractID = [Contract].ContractID
				inner join Contractor on [Contract].ContractorID = Contractor.ContractorID
		where
				not Requirement.VehicleContractID is null



GO


