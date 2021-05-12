CREATE VIEW [dbo].[vwRequirementsWithPresentations]
AS

		select
				Requirement.ContractID,
				Requirement.RequirementID,
				Documentation.DocumentationCode,
				Documentation.Description,

				'CONTRACTOR' AS ResourceType,
				Contractor.CompanyName AS ResourceIdentification,

				Requirement.PeriodID,
				RequirementStatus.RequirementStatusDescription,
				RequirementStatus.RequirementStatusSummary,
				Requirement.DueDate,
				'<--->' as Sep,
				Presentation.PresentationID,
				PresentationStatus.PresentationStatusDescription,
				PresentationStatus.PresentationStatusSummary,
				Presentation.PresentationDate,
				Presentation.DocumentFiles,
				Presentation.TakenForID,
				TakeUser.UserName as TakeForUser,
				Presentation.TakenDate,
				Presentation.ApprovedForID,
				ApprovedUser.UserName as ApprovedForUser,
				Presentation.ApprovedDate,
				Presentation.RejectedForID,
				RejectedUser.UserName as RejectedForUser,
				Presentation.RejectedDate,
				Presentation.Observations
		from
				Requirement
				inner join DocumentationBusinessType on Requirement.DocumentationBusinessTypeID = DocumentationBusinessType.DocumentationBusinessTypeID
				inner join Documentation on DocumentationBusinessType.DocumentationID = Documentation.DocumentationID
				inner join RequirementStatus on Requirement.RequirementStatus = RequirementStatus.RequirementStatus
				inner join [Contract] on Requirement.ContractID = [Contract].ContractID
				inner join Contractor on [Contract].ContractorID = Contractor.ContractorID
				left join Presentation on Requirement.RequirementID = Presentation.RequirementID
				left join PresentationStatus on Presentation.PresentationStatus = PresentationStatus.PresentationStatus
				left join AspNetUsers TakeUser on Presentation.TakenForID = TakeUser.Id
				left join AspNetUsers ApprovedUser on Presentation.ApprovedForID = ApprovedUser.Id
				left join AspNetUsers RejectedUser on Presentation.RejectedForID = RejectedUser.Id
		where
				Requirement.EmployeeContractID is null and Requirement.VehicleContractID is null

		union

		select
				Requirement.ContractID,
				Requirement.RequirementID,
				Documentation.DocumentationCode,
				Documentation.Description,

				'EMPLOYEE' AS ResourceType,
				(Employee.LastName + ', ' + Employee.FirstName) AS ResourceIdentification,

				Requirement.PeriodID,
				RequirementStatus.RequirementStatusDescription,
				RequirementStatus.RequirementStatusSummary,
				Requirement.DueDate,
				'<--->' as Sep,
				Presentation.PresentationID,
				PresentationStatus.PresentationStatusDescription,
				PresentationStatus.PresentationStatusSummary,
				Presentation.PresentationDate,
				Presentation.DocumentFiles,
				Presentation.TakenForID,
				TakeUser.UserName as TakeForUser,
				Presentation.TakenDate,
				Presentation.ApprovedForID,
				ApprovedUser.UserName as ApprovedForUser,
				Presentation.ApprovedDate,
				Presentation.RejectedForID,
				RejectedUser.UserName as RejectedForUser,
				Presentation.RejectedDate,
				Presentation.Observations
		from
				Requirement
				inner join DocumentationBusinessType on Requirement.DocumentationBusinessTypeID = DocumentationBusinessType.DocumentationBusinessTypeID
				inner join Documentation on DocumentationBusinessType.DocumentationID = Documentation.DocumentationID
				inner join RequirementStatus on Requirement.RequirementStatus = RequirementStatus.RequirementStatus
				inner join EmployeeContract on Requirement.EmployeeContractID = EmployeeContract.EmployeeContractID  
				inner join Employee on EmployeeContract.EmployeeID = Employee.EmployeeID
				left join Presentation on Requirement.RequirementID = Presentation.RequirementID
				left join PresentationStatus on Presentation.PresentationStatus = PresentationStatus.PresentationStatus
				left join AspNetUsers TakeUser on Presentation.TakenForID = TakeUser.Id
				left join AspNetUsers ApprovedUser on Presentation.ApprovedForID = ApprovedUser.Id
				left join AspNetUsers RejectedUser on Presentation.RejectedForID = RejectedUser.Id
		where
				not Requirement.EmployeeContractID is null

		union

		select
				Requirement.ContractID,
				Requirement.RequirementID,
				Documentation.DocumentationCode,
				Documentation.Description,

				'VEHICLE' AS ResourceType,
				Vehicle.IdentificationNumber AS ResourceIdentification,

				Requirement.PeriodID,
				RequirementStatus.RequirementStatusDescription,
				RequirementStatus.RequirementStatusSummary,
				Requirement.DueDate,
				'<--->' as Sep,
				Presentation.PresentationID,
				PresentationStatus.PresentationStatusDescription,
				PresentationStatus.PresentationStatusSummary,
				Presentation.PresentationDate,
				Presentation.DocumentFiles,
				Presentation.TakenForID,
				TakeUser.UserName as TakeForUser,
				Presentation.TakenDate,
				Presentation.ApprovedForID,
				ApprovedUser.UserName as ApprovedForUser,
				Presentation.ApprovedDate,
				Presentation.RejectedForID,
				RejectedUser.UserName as RejectedForUser,
				Presentation.RejectedDate,
				Presentation.Observations
		from
				Requirement
				inner join DocumentationBusinessType on Requirement.DocumentationBusinessTypeID = DocumentationBusinessType.DocumentationBusinessTypeID
				inner join Documentation on DocumentationBusinessType.DocumentationID = Documentation.DocumentationID
				inner join RequirementStatus on Requirement.RequirementStatus = RequirementStatus.RequirementStatus
				inner join VehicleContract on Requirement.VehicleContractID = VehicleContract.VehicleContractID  
				inner join Vehicle on VehicleContract.VehicleID = Vehicle.VehicleID
				left join Presentation on Requirement.RequirementID = Presentation.RequirementID
				left join PresentationStatus on Presentation.PresentationStatus = PresentationStatus.PresentationStatus
				left join AspNetUsers TakeUser on Presentation.TakenForID = TakeUser.Id
				left join AspNetUsers ApprovedUser on Presentation.ApprovedForID = ApprovedUser.Id
				left join AspNetUsers RejectedUser on Presentation.RejectedForID = RejectedUser.Id
		where
				not Requirement.VehicleContractID is null