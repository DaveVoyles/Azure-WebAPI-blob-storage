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
    /// <summary>
    /// Uploads a single image to blob storage 
    /// </summary>
    public class BlobStorageMultipartStreamProvider : MultipartStreamProvider
    {
        public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
        {
            Stream stream                                    = null;
            ContentDispositionHeaderValue contentDisposition = headers.ContentDisposition;

            if (!string.IsNullOrWhiteSpace(contentDisposition?.FileName))
            {
                // TODO: Change this to the container for each day
                string containerName = "dumpster";
                var connectionString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;

                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
                CloudBlobClient blobClient         = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer blobContainer   = blobClient.GetContainerReference(containerName);

  
                var fileName = PrependDateToNameJpg("Images");
                CloudBlockBlob blob = blobContainer.GetBlockBlobReference(fileName);

                stream = blob.OpenWrite();
            }
            return stream;
        }


        private static string PrependDateToNameJpg(string sRootName)
        {
            string currentDate = DateTime.Now.ToString("yy-MM-dd-HH-mm-ss");
            string newName     = currentDate + sRootName + ".jpg";

            return newName;
        }
    }
}