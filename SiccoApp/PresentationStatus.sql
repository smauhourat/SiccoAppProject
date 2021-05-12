CREATE VIEW [dbo].[PresentationStatus]
AS

		SELECT
				1 AS PresentationStatus,
				'Pending' AS PresentationStatusDescription

		UNION
		SELECT
				2 AS PresentationStatus,
				'ToProcess' AS PresentationStatusDescription

		UNION
		SELECT
				3 AS PresentationStatus,
				'Processing' AS PresentationStatusDescription

		UNION
		SELECT
				4 AS PresentationStatus,
				'Approved' AS PresentationStatusDescription

		UNION
		SELECT
				5 AS PresentationStatus,
				'Rejected' AS PresentationStatusDescription



GO


