CREATE PROCEDURE [dbo].[spDocumentationMatrix_Delete]

	@CustomerID INT
AS

	DECLARE @Return VARCHAR(150)
	SET @Return = ''

	--------------------------------------------------------------------------------------------------------------------------------------------------------
	-- Chequeamos que no esten ya generados la Matriz de Documentacion para el Cliente con Requerimientos con presentaciones
	--------------------------------------------------------------------------------------------------------------------------------------------------------
	IF --@ForceDelete = 0 AND 
	--(EXISTS(SELECT DocumentationID FROM Documentation WHERE CustomerID = @CustomerID) OR EXISTS(SELECT BusinessTypeID FROM BusinessType WHERE CustomerID = @CustomerID))
	(EXISTS(SELECT * FROM Presentation INNER JOIN Requirement ON Presentation.RequirementID = Requirement.RequirementID INNER JOIN DocumentationBusinessType ON Requirement.DocumentationBusinessTypeID = DocumentationBusinessType.DocumentationBusinessTypeID INNER JOIN BusinessType ON DocumentationBusinessType.BusinessTypeID = BusinessType.BusinessTypeID AND BusinessType.CustomerID = @CustomerID))
		BEGIN
			SET @Return = 'SP_MSG_EXIST_DOCUMENTATION'

			RAISERROR(@Return, 16, 1)
			RETURN
		END
	--------------------------------------------------------------------------------------------------------------------------------------------------------


	BEGIN TRY

		----------------------------------------------------------------------------------------------
		BEGIN TRAN
		----------------------------------------------------------------------------------------------

		DELETE FROM EmployeeContract WHERE ContractID IN (SELECT ContractID FROM Contract WHERE ContractorID IN (SELECT ContractorID FROM Contractor WHERE CustomerId = @CustomerID) )

		DELETE FROM VehicleContract WHERE ContractID IN (SELECT ContractID FROM Contract WHERE ContractorID IN (SELECT ContractorID FROM Contractor WHERE CustomerId = @CustomerID) )

		DELETE FROM Requirement WHERE ContractID IN (SELECT ContractID FROM Contract WHERE ContractorID IN (SELECT ContractorID FROM Contractor WHERE CustomerId = @CustomerID) )

		DELETE FROM Contract WHERE ContractorID IN (SELECT ContractorID FROM Contractor WHERE CustomerId = @CustomerID)

		DELETE FROM Employee WHERE ContractorID IN (SELECT ContractorID FROM Contractor WHERE CustomerID = @CustomerID)

		DELETE FROM Vehicle WHERE ContractorID IN (SELECT ContractorID FROM Contractor WHERE CustomerID = @CustomerID)

		DELETE FROM Contractor WHERE CustomerID = @CustomerID

		DELETE 
		FROM 
				DocumentationBusinessType
		WHERE
				DocumentationBusinessTypeID IN
				(
					SELECT 
							DocumentationBusinessType.DocumentationBusinessTypeID
					FROM 
							DocumentationBusinessType 
							INNER JOIN BusinessType ON DocumentationBusinessType.BusinessTypeID = BusinessType.BusinessTypeID
							INNER JOIN Documentation ON DocumentationBusinessType.DocumentationID = Documentation.DocumentationID
					WHERE
							BusinessType.CustomerID = @CustomerID
							and Documentation.CustomerID = @CustomerID				
				)

		DELETE FROM BusinessType WHERE CustomerID = @CustomerID --AND NOT BusinessTypeID IN (SELECT BusinessTypeID FROM Contractor WHERE CustomerID = @CustomerID)

		DELETE FROM Documentation WHERE CustomerID = @CustomerID


		----------------------------------------------------------------------------------------------

		----------------------------------------------------------------------------------------------
		COMMIT TRAN
		----------------------------------------------------------------------------------------------

	END TRY
	BEGIN CATCH

		----------------------------------------------------------------------------------------------
		ROLLBACK TRAN
		----------------------------------------------------------------------------------------------

		DECLARE @MsgCath VARCHAR(250)
		SET @MsgCath = ERROR_MESSAGE() + ' - ' + CONVERT(VARCHAR, ERROR_NUMBER())
		RAISERROR(@MsgCath, 16, 1)

	END CATCH