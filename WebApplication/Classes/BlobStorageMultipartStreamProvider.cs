using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace WebApplication.Classes
{
    /// <summary>
    /// Uploads a single image to blob storage 
    /// </summary>
    public class BlobStorageMultipartStreamProvider : MultipartStreamProvider
    {
        // Get memory stream from the file being uploaded
        public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
        {
            Stream stream                                    = null;
            ContentDispositionHeaderValue contentDisposition = headers.ContentDisposition;

            if (!string.IsNullOrWhiteSpace(contentDisposition?.FileName))
            {
                // Create container
                var containerName  = "dumpster";     
                var sContainerName = AppendDateToName(containerName);

                // Blob storage set-up
                var connectionString               = ConfigurationManager.AppSettings["ConnString"];
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
                CloudBlobClient blobClient         = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer blobContainer   = blobClient.GetContainerReference(sContainerName);
                                   blobContainer.CreateIfNotExists();

                // Prepend date to image name 
                var fileName        = PrependDateToNameJpg("Images");
                CloudBlockBlob blob = blobContainer.GetBlockBlobReference(fileName);

                stream = blob.OpenWrite();
            }
            return stream;
        }


        /// <summary>
        /// Prepends current date to file name with format: yy-MM-dd-HH-mm-ss
        /// </summary>
        /// <param name="sRootName">Name of file we are prepending</param>
        /// <returns>A string with current date to file name with format: yy-MM-dd-HH-mm-ss</returns>
        private static string PrependDateToNameJpg(string sRootName)
        {
            string currentDate = DateTime.Now.ToString("yy-MM-dd-HH-mm-ss-");
            string newName     = currentDate + sRootName + ".jpg";

            return newName;
        }


        /// <summary>
        ///  Appends current date to file name with format: yy-MM-dd
        ///  Later on we'll use this to search for containers within a week and return those w/ images
        /// </summary>
        /// <returns>A string with current date to file name with format: yy-MM-dd</returns>
        private static string AppendDateToName(string sRootName)
        {
            string currentDate = DateTime.Now.ToString("-yy-MM-dd");
            string newName     = sRootName + currentDate;

            return newName;
        }
    }
}