CREATE VIEW [dbo].[PresentationActionType]
AS

	SELECT
			1 AS PresentationActionType,
			'Taken' AS PresentationActionTypeDescription

	UNION

	SELECT
			2 AS PresentationActionType,
			'Approve' AS PresentationActionTypeDescription

	UNION

	SELECT
			3 AS PresentationActionType,
			'Reject' AS PresentationActionTypeDescription

	UNION

	SELECT
			4 AS PresentationActionType,
			'Drop' AS PresentationActionTypeDescription