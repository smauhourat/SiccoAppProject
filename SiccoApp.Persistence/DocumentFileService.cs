using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using SiccoApp.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace SiccoApp.Persistence
{
    public class DocumentFileService : IDocumentFileService
    {
        ILogger log = null;

        public DocumentFileService(ILogger logger)
        {
            log = logger;
        }
        async public void CreateAndConfigureAsync()
        {
            try
            {
                // Create the "images" container if it doesn't already exist.
                //if (await container.CreateIfNotExistsAsync())
                //{
                //    // Enable public access on the newly created "images" container
                //    await container.SetPermissionsAsync(
                //        new BlobContainerPermissions 
                //        {
                //            PublicAccess =
                //                BlobContainerPublicAccessType.Blob
                //        });

                //    log.Information("Successfully created Doc Images Container and made it public");
                //}
            }
            catch (Exception ex)
            {
                log.Error(ex, "Failure to Create or Configure images container in Doc Service");
                throw;
            }
        }

        async public Task<string> UploadDocumentFileAsync(HttpPostedFileBase documentFileToUpload)
        {
            if (documentFileToUpload == null || documentFileToUpload.ContentLength == 0)
            {
                return null;
            }

            string imageName = String.Format("task-photo-{0}{1}",
                Guid.NewGuid().ToString(),
                Path.GetExtension(documentFileToUpload.FileName));

            return await UploadDocumentFileAsync(documentFileToUpload, imageName);
        }

        async public Task<string> UploadDocumentFileAsync(HttpPostedFileBase documentFileToUpload, string fileName)
        {
            if (documentFileToUpload == null || documentFileToUpload.ContentLength == 0)
            {
                return null;
            }

            string fullPath = null;
            string imageName = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                // Create a unique name for the images we are about to upload
                imageName = String.Format("FILE-{0}{1}{2}",
                    fileName,
                    Guid.NewGuid().ToString(),
                    Path.GetExtension(documentFileToUpload.FileName));

                fullPath = Path.Combine(CommonHelper.MapPath("~/Documents"), imageName);
                documentFileToUpload.SaveAs(fullPath);

                timespan.Stop();
                log.TraceApi("Doc Service", "DocumentFileService.UploadDocumentFile", timespan.Elapsed, "imagepath={0}", fullPath);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error upload photo blob to storage");
                throw;
            }

            //return imageName;
            return await Task.FromResult(imageName);
        }

        private string GetSingleFileName(string documentFileName)
        {
            if (String.IsNullOrEmpty(documentFileName)) return documentFileName;
            if (!documentFileName.Contains(@"/")) return documentFileName;

            var result = documentFileName.Substring(documentFileName.LastIndexOf(@"/")+1, documentFileName.Length - documentFileName.LastIndexOf(@"/")-1);
            return result;
        }

        public async Task DeleteDocumentFileAsync(string documentFileName)
        {
            if (documentFileName == null || documentFileName == "")
            {
                return;
            }

            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                string fullNameFile = Path.Combine(CommonHelper.MapPath("~/Documents"), documentFileName);
                //string fullNameFile = path + documentFileName;

                if (File.Exists(fullNameFile))
                {
                    File.SetAttributes(fullNameFile, FileAttributes.Normal);
                    File.Delete(fullNameFile);
                }


                await Task.Delay(0);

                timespan.Stop();
                log.TraceApi("Doc Service", "DocumentFileService.DeleteDocumentFileAsync", timespan.Elapsed, "documentFileName={0}", documentFileName);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error delete photo blob to storage");
                throw;
            }
        }
    }
}
