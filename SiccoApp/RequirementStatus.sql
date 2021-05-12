CREATE VIEW [dbo].[RequirementStatus]
AS

		SELECT
				1 AS RequirementStatus,
				'Created' AS RequirementStatusDescription

		UNION
		SELECT
				2 AS RequirementStatus,
				'Pending' AS RequirementStatusDescription

		UNION
		SELECT
				3 AS RequirementStatus,
				'ToProcess' AS RequirementStatusDescription

		UNION
		SELECT
				4 AS RequirementStatus,
				'Processing' AS RequirementStatusDescription

		UNION
		SELECT
				5 AS RequirementStatus,
				'Approved' AS RequirementStatusDescription

		UNION
		SELECT
				6 AS RequirementStatus,
				'Rejected' AS RequirementStatusDescription



GO


