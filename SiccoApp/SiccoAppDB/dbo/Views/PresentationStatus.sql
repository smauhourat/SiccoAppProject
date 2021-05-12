

CREATE VIEW [dbo].[PresentationStatus]
AS

		SELECT
				1 AS PresentationStatus,
				'Pending' AS PresentationStatusDescription,
				'(Cuando se sube el documento y queda disponible) en este estado aun puede ser eliminado por el Contratista' AS PresentationStatusSummary

		UNION
		SELECT
				2 AS PresentationStatus,
				'ToProcess' AS PresentationStatusDescription,
				'(Marcado listo para ser tomado por un Auditor) Este estado en ppio puede ser manejado de forma automatica' AS PresentationStatusSummary

		UNION
		SELECT
				3 AS PresentationStatus,
				'Processing' AS PresentationStatusDescription,
				'(Cuando es tomado por un Auditor para su analisis) dispara el PROCESSING del Requerimiento' AS PresentationStatusSummary

		UNION
		SELECT
				4 AS PresentationStatus,
				'Approved' AS PresentationStatusDescription,
				'(Cuando el Auditor aprueba el Requerimiento en base al analisis del documento presentado) ESTADO FINAL, dispara el APPROVED del Requerimiento' AS PresentationStatusSummary

		UNION
		SELECT
				5 AS PresentationStatus,
				'Rejected' AS PresentationStatusDescription,
				'(Cuando el Auditor NO aprueba el Requerimiento) ESTADO FINAL, dipara el PENDING del Requerimiento.' AS PresentationStatusSummary