CREATE PROCEDURE [dbo].[spDocumentationMatrix_Delete]

	@CustomerID INT
AS




	BEGIN TRY

		----------------------------------------------------------------------------------------------
		BEGIN TRAN
		----------------------------------------------------------------------------------------------

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

		DELETE FROM BusinessType WHERE CustomerID = @CustomerID

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

