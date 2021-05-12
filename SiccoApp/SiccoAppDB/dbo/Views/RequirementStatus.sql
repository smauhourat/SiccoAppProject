

CREATE VIEW [dbo].[RequirementStatus]
AS

		SELECT
				1 AS RequirementStatus,
				'Created' AS RequirementStatusDescription,
				'Cuando se crea un requerimiento pero aun no esta disponible para el Sistema' as RequirementStatusSummary

		UNION
		SELECT
				2 AS RequirementStatus,
				'Pending' AS RequirementStatusDescription,
				'Cuando pasa a estar disponible para el Sistema, y no tiene presentaciones para procesar' as RequirementStatusSummary

		UNION
		SELECT
				3 AS RequirementStatus,
				'ToProcess' AS RequirementStatusDescription,
				'Cuando se le adjunto una Presentacion pero aun no ha sido "tomada" por un Auditor' as RequirementStatusSummary

		UNION
		SELECT
				4 AS RequirementStatus,
				'Processing' AS RequirementStatusDescription,
				'Cuando la Presentacion adjunta es tomada por un Auditor para su analisis' as RequirementStatusSummary

		UNION
		SELECT
				5 AS RequirementStatus,
				'Approved' AS RequirementStatusDescription,
				'Cuando tiene una Presentacion Aprobada - ESTADO FINAL' as RequirementStatusSummary

		UNION
		SELECT
				6 AS RequirementStatus,
				'Rejected' AS RequirementStatusDescription,
				'Cuando no tiene ninguna Presentacion Aprobada y ademas se vencio la fecha de presentaciones - ESTADO FINAL' as RequirementStatusSummary