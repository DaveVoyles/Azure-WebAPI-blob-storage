using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace WebApplication.Classes
{
    public class BlobStorageMultipartStreamProvider : MultipartStreamProvider
    {
        public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
        {
            Stream stream                                    = null;
            ContentDispositionHeaderValue contentDisposition = headers.ContentDisposition;

            if (!string.IsNullOrWhiteSpace(contentDisposition?.FileName))
            {
                string containerName = "dumpster";
                var connectionString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;

                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
                CloudBlobClient blobClient         = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer blobContainer   = blobClient.GetContainerReference(containerName);

                var fileName = "knuckles.jpg";
                //CloudBlockBlob blob = blobContainer.GetBlockBlobReference(contentDisposition.FileName);
                CloudBlockBlob blob = blobContainer.GetBlockBlobReference(fileName);

                stream = blob.OpenWrite();
            }
            return stream;
        }
    }
}