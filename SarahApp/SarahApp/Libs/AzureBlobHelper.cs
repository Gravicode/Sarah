using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage types
using Windows.Storage;
using System.IO;

namespace SarahApp
{
    public class AzureBlobHelper
    {
        public CloudBlobContainer container { get; set; }
        public AzureBlobHelper()
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                APPCONTANTS.BlobConnString);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container.
            container = blobClient.GetContainerReference("unit");

            // Create the container if it doesn't already exist.
            Task<bool> a =  container.CreateIfNotExistsAsync();
          
            a.Wait();
        }

        public async Task<bool> UploadFile(StorageFile imageFile)
        {
            try
            {
                var blobName = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")+".jpg";
                // Retrieve reference to a blob named "myblob".
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
                // Create or overwrite the "myblob" blob with contents from a local file.
                using (var fileStream = await imageFile.OpenStreamForReadAsync())
                {
                    await blockBlob.UploadFromStreamAsync(fileStream);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
