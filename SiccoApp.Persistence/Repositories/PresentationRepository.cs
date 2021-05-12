using SiccoApp.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public class PresentationRepository : IPresentationRepository
    {
        private SiccoAppContext db = new SiccoAppContext();
        private ILogger log = null;
        private IDocumentFileService documentFileService = null;

        public PresentationRepository(ILogger logger, IDocumentFileService docFileServices)
        {
            log = logger;
            documentFileService = docFileServices;
        }

        /// <summary>
        /// Cuando una Presetacion la toma un Auditor para su revision
        /// </summary>
        /// <param name="presentation"></param>
        /// <returns></returns>
#warning "Falta tomar el Id del Usuario logueado"
        public async Task TakeToAudit(Presentation presentation)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            SiccoAppConfiguration.SuspendExecutionStrategy = true;

            DbContextTransaction tran = db.Database.BeginTransaction();

            try
            {
                if (presentation.PresentationStatus != PresentationStatus.Pending)
                    throw new Exception("No se puede tomar Presentaciones que no estan PENDIENTES");

                //actualizacion de Presentation
                presentation.TakenDate = DateTime.UtcNow;
                //presentation.TakenFor = 999;
                presentation.PresentationStatus = PresentationStatus.Processing;

                //actualizacion de Requerimientos
                var requirement = await db.Requirements.FindAsync(presentation.RequirementID);
                requirement.RequirementStatus = RequirementStatus.Processing;

                //creacion de el historico de accionesde Presentation
                PresentationAction presentationAction = new PresentationAction
                {
                    PresentationID = presentation.PresentationID,
                    PresentationDate = DateTime.UtcNow,
                    ActionForID = presentation.TakenForID,
                    PresentationActionType = PresentationActionType.Taken
                };
                db.PresentationActions.Add(presentationAction);

                db.Entry(presentation).State = EntityState.Modified;

                await db.SaveChangesAsync();

                tran.Commit();

                timespan.Stop();
                log.TraceApi("SQL Database", "PresentationRepository.TakeToAudit", timespan.Elapsed, "presentation={0}", presentation);
            }
            catch (Exception e)
            {
                tran.Rollback();
                log.Error(e, "Error in PresentationRepository.TakeToAudit(presentation={0})", presentation);
                throw;
            }

            SiccoAppConfiguration.SuspendExecutionStrategy = false;
        }

#warning "Falta tomar el Id del Usuario logueado"
        public async Task Approve(Presentation presentation)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            SiccoAppConfiguration.SuspendExecutionStrategy = true;

            DbContextTransaction tran = db.Database.BeginTransaction();

            try
            {
                if (presentation.PresentationStatus != PresentationStatus.Processing)
                    throw new Exception("No se puede aprobar Presentaciones que no estan EN PROCESO");

                //actualizacion de Presentation
                presentation.ApprovedDate = DateTime.UtcNow;
                //presentation.ApprovedFor = 999;
                presentation.PresentationStatus = PresentationStatus.Approved;

                //actualizacion de Requerimientos
                var requirement = await db.Requirements.FindAsync(presentation.RequirementID);
                requirement.RequirementStatus = RequirementStatus.Approved;

                //creacion de el historico de accionesde Presentation
                PresentationAction presentationAction = new PresentationAction
                {
                    PresentationID = presentation.PresentationID,
                    PresentationDate = DateTime.UtcNow,
                    ActionForID = presentation.ApprovedForID,
                    PresentationActionType = PresentationActionType.Approve
                };
                db.PresentationActions.Add(presentationAction);

                db.Entry(presentation).State = EntityState.Modified;

                await db.SaveChangesAsync();

                tran.Commit();

                timespan.Stop();
                log.TraceApi("SQL Database", "PresentationRepository.Approve", timespan.Elapsed, "presentation={0}", presentation);
            }
            catch (Exception e)
            {
                tran.Rollback();
                log.Error(e, "Error in PresentationRepository.Approve(presentation={0})", presentation);
                throw;
            }

            SiccoAppConfiguration.SuspendExecutionStrategy = false;
        }

        public async Task Reject(Presentation presentation)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            SiccoAppConfiguration.SuspendExecutionStrategy = true;

            DbContextTransaction tran = db.Database.BeginTransaction();

            try
            {
                if (presentation.PresentationStatus != PresentationStatus.Processing)
                    throw new Exception("No se puede rechazar Presentaciones que no estan EN PROCESO");

                //actualizacion de Presentation
                presentation.RejectedDate = DateTime.UtcNow;
                //presentation.ApprovedFor = 999;
                presentation.PresentationStatus = PresentationStatus.Rejected;

                //actualizacion de Requerimientos
                var requirement = await db.Requirements.FindAsync(presentation.RequirementID);
                requirement.RequirementStatus = RequirementStatus.Rejected;

                //creacion de el historico de accionesde Presentation
                PresentationAction presentationAction = new PresentationAction
                {
                    PresentationID = presentation.PresentationID,
                    PresentationDate = DateTime.UtcNow,
                    ActionForID = presentation.RejectedForID,
                    PresentationActionType = PresentationActionType.Reject
                };
                db.PresentationActions.Add(presentationAction);

                db.Entry(presentation).State = EntityState.Modified;

                await db.SaveChangesAsync();

                tran.Commit();

                timespan.Stop();
                log.TraceApi("SQL Database", "PresentationRepository.Reject", timespan.Elapsed, "presentation={0}", presentation);
            }
            catch (Exception e)
            {
                tran.Rollback();
                log.Error(e, "Error in PresentationRepository.Reject(presentation={0})", presentation);
                throw;
            }

            SiccoAppConfiguration.SuspendExecutionStrategy = false;
        }

        /// <summary>
        /// Cuando se agrega una Presentacion a un Requerimiento, la Presentacion queda en estado de PENDIENTE y el Requerimiento en estado TOPROCCESS.
        /// </summary>
        /// <param name="presentationToAdd"></param>
        /// <returns></returns>
        public async Task CreateAsync(Presentation presentationToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            SiccoAppConfiguration.SuspendExecutionStrategy = true;

            DbContextTransaction tran = db.Database.BeginTransaction();

            try
            {
                //El Requerimiento debe estar PENDING para poder aceptar Presentaciones
                var requirement = await db.Requirements.FindAsync(presentationToAdd.RequirementID);
                if (requirement.RequirementStatus != RequirementStatus.Pending)
                    throw new Exception("No se puede adjuntar Presentaciones a Requerimientos que no estan PENDIENTES");

                requirement.RequirementStatus = RequirementStatus.ToProcess;

                presentationToAdd.PresentationStatus = PresentationStatus.Pending;
                presentationToAdd.PresentationDate = DateTime.UtcNow;
                db.Presentations.Add(presentationToAdd);

                db.Entry(requirement).State = EntityState.Modified;

                await db.SaveChangesAsync();

                tran.Commit();

                timespan.Stop();
                log.TraceApi("SQL Database", "PresentationRepository.CreateAsync", timespan.Elapsed, "presentationToAdd={0}", presentationToAdd);
            }
            catch (Exception e)
            {
                tran.Rollback();
                log.Error(e, "Error in PresentationRepository.CreateAsync(presentationToAdd={0})", presentationToAdd);
                throw;
            }

            SiccoAppConfiguration.SuspendExecutionStrategy = false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Free managed resources
                if (db != null)
                {
                    db.Dispose();
                    db = null;
                }
                //if (documentFileService != null)
                //{
                //    documentFileService.dispose()
                //}
            }
        }

        public async Task<Presentation> FindByIdAsync(int presentationID)
        {
            Presentation presentation = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                presentation = await db.Presentations.FindAsync(presentationID);

                timespan.Stop();
                log.TraceApi("SQL Database", "PresentationRepository.FindByIdAsync", timespan.Elapsed, "presentationID={0}", presentationID);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in PresentationRepository.FindByIdAsync(presentationID={0})", presentationID);
                throw;
            }

            return presentation;
        }

        public async Task DeleteAsync(int presentationID)
        {
            Presentation presentation = null;
            Stopwatch timespan = Stopwatch.StartNew();

            SiccoAppConfiguration.SuspendExecutionStrategy = true;

            DbContextTransaction tran = db.Database.BeginTransaction();

            try
            {
                presentation = await db.Presentations.FindAsync(presentationID);

                if (presentation.PresentationStatus != PresentationStatus.Pending)
                    throw new Exception("No se puede eliminar Presentaciones cuyo estado no es PENDIENTE");

                var requirement = await db.Requirements.FindAsync(presentation.RequirementID);
                //if (requirement.RequirementStatus != RequirementStatus.Pending)
                //    throw new Exception("No se puede adjuntar Presentaciones a Requerimientos que no estan PENDIENTES");

                //Al eliminar la Presentacion el Requerimiento queda en estado PENDING
                requirement.RequirementStatus = RequirementStatus.Pending;

                await documentFileService.DeleteDocumentFileAsync(presentation.DocumentFiles);

                db.Presentations.Remove(presentation);

                db.Entry(requirement).State = EntityState.Modified;

                await db.SaveChangesAsync();

                tran.Commit();

                timespan.Stop();
                log.TraceApi("SQL Database", "PresentationRepository.DeleteAsync", timespan.Elapsed, "presentationID={0}", presentationID);
            }
            catch (Exception e)
            {
                tran.Rollback();

                log.Error(e, "Error in PresentationRepository.DeleteAsync(presentationID={0})", presentationID);
                throw;
            }

            SiccoAppConfiguration.SuspendExecutionStrategy = false;
        }

        public async Task<IList<Presentation>> FindByPeriodAsync(int periodID)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var result = await db.Presentations
                    //.Where(t => t.ContractorID == (contractorID == 0 ? t.ContractorID : contractorID))
                    .Where(t => t.Requirement.PeriodID == periodID).ToListAsync();
                //.OrderByDescending(t => t.StartDate).ToListAsync();

                timespan.Stop();
                log.TraceApi("SQL Database", "PresentationRepository.FindByPeriodAsync", timespan.Elapsed, "periodID={0}", periodID);

                return result;
            }
            catch (Exception e)
            {
                log.Error(e, "Error in PresentationRepository.FindByPeriodAsync(periodID={0})", periodID);
                throw;
            }
        }
    }
}
