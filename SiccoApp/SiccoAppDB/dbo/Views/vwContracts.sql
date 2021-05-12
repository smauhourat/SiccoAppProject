CREATE VIEW dbo.vwContracts

AS

		SELECT 
				Contractor.ContractorID,
				Contractor.CustomerID,
				Contractor.BusinessTypeID,
				Contractor.CompanyName,
				Contract.ContractID,
				Contract.ContractCode,
				Contract.ContractStatusID,
				ContractStatus.Description as ContractStatus,
				Contract.StartDate,
				Contract.EndDate
		FROM 
				Contractor
				INNER JOIN Contract ON Contractor.ContractorID = Contract.ContractorID
				LEFT JOIN ContractStatus ON Contract.ContractStatusID = ContractStatus.ContractStatusID