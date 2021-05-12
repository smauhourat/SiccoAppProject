CREATE PROCEDURE [dbo].[spDocumentationMatrix_Generate]
	@CustomerID INT,
	@ForceDelete INT
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

		EXEC spDocumentationMatrix_Delete @CustomerID

		----------------------------------------------------------------------------------------------
		INSERT INTO
			BusinessType
			(
				BusinessTypeTemplateID,
				CustomerID,
				BusinessTypeCode,
				Description 
			)

			SELECT
				BusinessTypeTemplateID,
				@CustomerID,
				BusinessTypeTemplateCode,
				Description
			FROM
				BusinessTypeTemplate
			WHERE
				NOT BusinessTypeTemplateID IN (SELECT BusinessTypeTemplateID FROM BusinessType WHERE BusinessTypeID IN (SELECT BusinessTypeID FROM Contractor WHERE CustomerID = @CustomerID))
		----------------------------------------------------------------------------------------------

		----------------------------------------------------------------------------------------------
		INSERT INTO
			Documentation
			(
				DocumentationTemplateID,
				CustomerID,
				DocumentationCode,
				Description,
				EntityTypeID
			)
			SELECT
				DocumentationTemplateID,
				@CustomerID,
				DocumentationTemplateCode,
				Description,
				EntityTypeID
			FROM
				DocumentationTemplate
		----------------------------------------------------------------------------------------------

		----------------------------------------------------------------------------------------------
		INSERT INTO
			DocumentationBusinessType
			(
				DocumentationBusinessTypeTemplateID,
				DocumentationID,
				BusinessTypeID,
				DocumentationImportanceID,
				DocumentationPeriodicityID,
				RestrictAccess
			)
			SELECT
				DocumentationBusinessTypeTemplate.DocumentationBusinessTypeTemplateID,
				Documentation.DocumentationID,
				BusinessType.BusinessTypeID,
				DocumentationBusinessTypeTemplate.DocumentationImportanceID,
				DocumentationBusinessTypeTemplate.DocumentationPeriodicityID,
				DocumentationBusinessTypeTemplate.RestrictAccess

			FROM
				DocumentationBusinessTypeTemplate
				INNER JOIN Documentation ON DocumentationBusinessTypeTemplate.DocumentationTemplateID = Documentation.DocumentationTemplateID AND Documentation.CustomerID = @CustomerID 
				INNER JOIN BusinessType ON DocumentationBusinessTypeTemplate.BusinessTypeTemplateID = BusinessType.BusinessTypeTemplateID AND BusinessType.CustomerID = @CustomerID 
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