using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiccoApp.Persistence.Repositories
{
    public class LogRecordRepository : ILogRecordRepository
    {
        private SiccoAppContext db = new SiccoAppContext();

        public void Create(LogRecord logRecordToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            //try
            //{
                db.LogRecords.Add(logRecordToAdd);
                db.SaveChanges();

                timespan.Stop();
                //log.TraceApi("SQL Database", "ProviderRepository.CreateAsync", timespan.Elapsed, "providerToAdd={0}", providerToAdd);

                //ojoo
                //throw new Exception("Este es un error de prueba");
            //}
            //catch (Exception e)
            //{
            //    //log.Error(e, "Error in ProviderRepository.CreateAsync(providerToAdd={0})", providerToAdd);
            //    throw;
            //}
        }

        public async Task CreateAsync(LogRecord logRecordToAdd)
        {
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                db.LogRecords.Add(logRecordToAdd);
                await db.SaveChangesAsync();

                timespan.Stop();
                //log.TraceApi("SQL Database", "ProviderRepository.CreateAsync", timespan.Elapsed, "providerToAdd={0}", providerToAdd);

                //ojoo
                //throw new Exception("Este es un error de prueba");
            }
            catch //(Exception e)
            {
                //log.Error(e, "Error in ProviderRepository.CreateAsync(providerToAdd={0})", providerToAdd);
                throw;
            }
        }

        public async Task<LogRecord> FindLogRecordByIDAsync(int logRecordID)
        {
            LogRecord logRecord = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                logRecord = await db.LogRecords.FindAsync(logRecordID);

                timespan.Stop();
                //log.TraceApi("SQL Database", "LogRecordRepository.FindProvidersByIDAsync", timespan.Elapsed, "logRecordID={0}", logRecordID);
            }
            catch //(Exception e)
            {
                //log.Error(e, "Error in LogRecordRepository.FindProvidersByIDAsync(logRecordID={0})", logRecordID);
                throw;
            }

            return logRecord;
        }

        public async Task DeleteAsync(int logRecordID)
        {
            LogRecord logRecord = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                logRecord = await db.LogRecords.FindAsync(logRecordID);
                db.LogRecords.Remove(logRecord);
                await db.SaveChangesAsync();

                timespan.Stop();
                //log.TraceApi("SQL Database", "ProviderRepository.DeleteAsync", timespan.Elapsed, "providerID={0}", providerID);
            }
            catch //(Exception e)
            {
                //log.Error(e, "Error in ProviderRepository.DeleteAsync(providerID={0})", providerID);
                throw;
            }
        }

        public async Task<List<LogRecord>> FindLogRecordsAsync()
        {
            var result = await db.LogRecords.ToListAsync();
            return result;
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
            }
        }
    }

}
