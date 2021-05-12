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
    public class DocumentFileBlobStorageService : IDocumentFileService
    {
        ILogger log = null;

        public DocumentFileBlobStorageService(ILogger logger)
        {
            log = logger;
        }
        async public void CreateAndConfigureAsync()
        {
            try
            {
                CloudStorageAccount storageAccount = StorageUtils.StorageAccount;

                // Create a blob client and retrieve reference to images container
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("images");

                // Create the "images" container if it doesn't already exist.
                if (await container.CreateIfNotExistsAsync())
                {
                    // Enable public access on the newly created "images" container
                    await container.SetPermissionsAsync(
                        new BlobContainerPermissions
                        {
                            PublicAccess =
                                BlobContainerPublicAccessType.Blob
                        });

                    log.Information("Successfully created Blob Storage Images Container and made it public");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Failure to Create or Configure images container in Blob Storage Service");
                throw;
            }
        }

        async public Task<string> UploadDocumentFileAsync(HttpPostedFileBase documentFileToUpload)
        {
            if (documentFileToUpload == null || documentFileToUpload.ContentLength == 0)
            {
                return null;
            }

            string fullPath = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                CloudStorageAccount storageAccount = StorageUtils.StorageAccount;

                // Create the blob client and reference the container
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("images");

                // Create a unique name for the images we are about to upload
                string imageName = String.Format("task-photo-{0}{1}",
                    Guid.NewGuid().ToString(),
                    Path.GetExtension(documentFileToUpload.FileName));

                // Upload image to Blob Storage
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(imageName);
                blockBlob.Properties.ContentType = documentFileToUpload.ContentType;
                await blockBlob.UploadFromStreamAsync(documentFileToUpload.InputStream);

                fullPath = blockBlob.Uri.ToString();

                timespan.Stop();
                log.TraceApi("Blob Service", "DocumentFileService.UploadDocumentFile", timespan.Elapsed, "imagepath={0}", fullPath);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error upload photo blob to storage");
                throw;
            }

            return fullPath;
        }

        async public Task<string> UploadDocumentFileAsync(HttpPostedFileBase documentFileToUpload, string fileName)
        {
            if (documentFileToUpload == null || documentFileToUpload.ContentLength == 0)
            {
                return null;
            }

            string fullPath = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                CloudStorageAccount storageAccount = StorageUtils.StorageAccount;

                // Create the blob client and reference the container
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("images");

                // Create a unique name for the images we are about to upload
                string imageName = String.Format("FILE-{0}{1}{2}",
                    fileName,
                    Guid.NewGuid().ToString(),
                    Path.GetExtension(documentFileToUpload.FileName));

                // Upload image to Blob Storage
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(imageName);
                blockBlob.Properties.ContentType = documentFileToUpload.ContentType;
                await blockBlob.UploadFromStreamAsync(documentFileToUpload.InputStream);

                fullPath = blockBlob.Uri.ToString();

                timespan.Stop();
                log.TraceApi("Blob Service", "DocumentFileService.UploadDocumentFile", timespan.Elapsed, "imagepath={0}", fullPath);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error upload photo blob to storage");
                throw;
            }

            return fullPath;
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
                CloudStorageAccount storageAccount = StorageUtils.StorageAccount;

                // Create the blob client and reference the container
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("images");

                CloudBlockBlob blockBlob = container.GetBlockBlobReference(GetSingleFileName(documentFileName));

                await blockBlob.DeleteAsync();

                timespan.Stop();
                log.TraceApi("Blob Service", "DocumentFileService.DeleteDocumentFileAsync", timespan.Elapsed, "documentFileName={0}", documentFileName);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error delete photo blob to storage");
                throw;
            }
        }
    }
}
