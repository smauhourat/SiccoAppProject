CREATE PROCEDURE [dbo].[spRequirement_Generate]
	
	@CustomerID	INT,
	@PeriodID	INT,
	@DueDate	DATE,
	@Return		VARCHAR(150) OUTPUT
AS

--EntityType: 
--1	CONTRACTOR
--2	EMPLOYEE
--3	VEHICLE

	SET @Return = ''

	--------------------------------------------------------------------------------------------------------------------------------------------------------
	-- Chequeamos que no esten ya generados los requerimientos para el Cliente/Periodo.
	--------------------------------------------------------------------------------------------------------------------------------------------------------
	IF EXISTS(SELECT RequirementID FROM Requirement INNER JOIN Contract ON Requirement.ContractID = Contract.ContractID WHERE Contract.CustomerID = @CustomerID AND Requirement.PeriodID = @PeriodID)
	BEGIN
			SET @Return = 'SP_MSG_EXIST_REQUIREMENT'
			RAISERROR(@Return, 16, 1)
			RETURN
	END

	INSERT INTO	
			Requirement 
			(
				ContractID,
				DocumentationBusinessTypeID,
				EmployeeContractID,
				VehicleContractID,
				PeriodID,
				RequirementStatus,
				DueDate 
			)
	--------------------------------------------------------------------------------------------------------------------------------------------------------
	-- Son todos los documentos de CONTRACTOR que se deben generar como requerimientos
	--------------------------------------------------------------------------------------------------------------------------------------------------------
	SELECT
			DISTINCT
			Contract.ContractID,
			DocumentationBusinessType.DocumentationBusinessTypeID,
			NULL AS EmployeeContractID,
			NULL AS VehicleContractID,
			@PeriodID AS PeriodID,
			2 AS RequirementStatus, -- PENDING
			@DueDate AS DueDate 
	FROM
			Contract
			INNER JOIN Contractor ON Contract.ContractorID = Contractor.ContractorID
			INNER JOIN BusinessType ON Contractor.BusinessTypeID = BusinessType.BusinessTypeID AND BusinessType.CustomerID = @CustomerID
			INNER JOIN DocumentationBusinessType ON BusinessType.BusinessTypeID = DocumentationBusinessType.BusinessTypeID 
			INNER JOIN Documentation ON DocumentationBusinessType.DocumentationID = Documentation.DocumentationID AND Documentation.CustomerID = @CustomerID AND Documentation.EntityTypeID = 1 -- CONTRACTOR

	UNION ALL

	--------------------------------------------------------------------------------------------------------------------------------------------------------
	-- Son todos los documentos de EMPLOYEE que se deben generar como requerimientos
	--------------------------------------------------------------------------------------------------------------------------------------------------------
	SELECT
			DISTINCT
			Contract.ContractID,
			DocumentationBusinessType.DocumentationBusinessTypeID,
			EmployeeContract.EmployeeContractID,
			NULL AS VehicleContractID,
			@PeriodID AS PeriodID,
			2 AS RequirementStatus, -- PENDING
			@DueDate AS DueDate 
	FROM
			Contract
			INNER JOIN Contractor ON Contract.ContractorID = Contractor.ContractorID
			INNER JOIN BusinessType ON Contractor.BusinessTypeID = BusinessType.BusinessTypeID AND BusinessType.CustomerID = @CustomerID
			INNER JOIN DocumentationBusinessType ON BusinessType.BusinessTypeID = DocumentationBusinessType.BusinessTypeID 
			INNER JOIN Documentation ON DocumentationBusinessType.DocumentationID = Documentation.DocumentationID AND Documentation.CustomerID = @CustomerID AND Documentation.EntityTypeID = 2 -- EMPLOYEE
			INNER JOIN EmployeeContract ON Contract.ContractID = EmployeeContract.ContractID 

	UNION ALL

	--------------------------------------------------------------------------------------------------------------------------------------------------------
	-- Son todos los documentos de VEHICLE que se deben generar como requerimientos
	--------------------------------------------------------------------------------------------------------------------------------------------------------
	SELECT
			DISTINCT
			Contract.ContractID,
			DocumentationBusinessType.DocumentationBusinessTypeID,
			NULL AS EmployeeContractID,
			VehicleContract.VehicleContractID,
			@PeriodID AS PeriodID,
			2 AS RequirementStatus, -- PENDING
			@DueDate AS DueDate 
	FROM
			Contract
			INNER JOIN Contractor ON Contract.ContractorID = Contractor.ContractorID
			INNER JOIN BusinessType ON Contractor.BusinessTypeID = BusinessType.BusinessTypeID AND BusinessType.CustomerID = @CustomerID
			INNER JOIN DocumentationBusinessType ON BusinessType.BusinessTypeID = DocumentationBusinessType.BusinessTypeID 
			INNER JOIN Documentation ON DocumentationBusinessType.DocumentationID = Documentation.DocumentationID AND Documentation.CustomerID = @CustomerID AND Documentation.EntityTypeID = 3 -- VEHICLE
			INNER JOIN VehicleContract ON Contract.ContractID = VehicleContract.ContractID 

	ORDER BY
			Contract.ContractID,
			DocumentationBusinessType.DocumentationBusinessTypeID

