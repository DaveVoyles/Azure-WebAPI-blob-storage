# Azure-WebAPI-blob-storage
### Author(s): Dave Voyles | [@DaveVoyles](http://www.twitter.com/DaveVoyles)
### URL: [www.DaveVoyles.com][1]

Access Azure Blob Storage from a .NET Web API app.
----------
### About
Manipulate Azure Blob Storage from a .NET Web API app. Simply host this project in Azure App Service.


## Instructions
You'll need to set the conection string to your **Azure Blob Storage Account** to store any content and use this function.

### If testing locally...
You'll need to change the connection strings in the *web.config* file:

```xaml
 <connectionStrings>
    <add name="ConnString"  connectionString="DefaultEndpointsProtocol=https;AccountName=MyStorageAccount;AccountKey=7msknqo7qOzSxh34123Xa1gr5rglDle0KEBowa3Kz0ZapefBN45uV59YyBeFIn1v1jqe/sqeWYo12412Q==;" />
    <add name="AccountName" connectionString="Mystorageacct" />
    <add name="Key"         connectionString="7msknqo7qOzSxhdh6THUr23r23r1gr5rglDle0KEBowa3Kz0ZapefBN45uV59YyBeFIn1v1jqe/sq4234234Yoo2SfYQ==" />
  </connectionStrings>

```


That information can be found in your Azure Portal or Azure Storage Explorer, Here is where it appears in the portal:

![Imgur](http://i.imgur.com/bVsa0zI.png)

AzureBlobManager.cs uses this to connect to your blob storage account:

``` csharp

        private static string connString = Environment.GetEnvironmentVariable("ConnString");       
         
        public AzureBlobManager()
        {
            _storageAccount = CloudStorageAccount.Parse(connString);
            _blobClient     = _storageAccount.CreateCloudBlobClient();
            _container      = _blobClient.GetContainerReference(ROOT_CONTAINER_NAME);
        }
```

As does the **BlobStorageMultipartStreamProvider** with this line:

```csharp
var connectionString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
```


### If hosting this in azure
Create your connection strings in the configuration tool there. [Documentation.](https://azure.microsoft.com/en-us/blog/windows-azure-web-sites-how-application-strings-and-connection-strings-work/)


## Functionality
----------


### Uploading images to blob storage

This is done by making a **POST** request for *each image* you would like to upload. Navigate to **nameOfWebsite/api/ImageUpload** and pass in the image as a a *.png* or *.jpg*.

This function will  reads the incoming multipart request, and convert it into a memory streams on the server side.

In a tool such as Postman, I would post an image like so:

![Imgur](http://i.imgur.com/FTivg5G.png)

And make sure that the header (in Postman at least) is empty, as Postman will fill it in automatically when it sends the data:

![Imgur](http://i.imgur.com/f19NDwO.png)


This takes the name of the image, pre-pends the current date, and saves the image to a container named after the current date to blob storage. 


### Combining images and uploading to blob storage

You can also make a **Get** call to **nameOfWebsite/api/Images** and it will combine all of the images found in a container matching today's date. 

It is done with this function:

```csharp
        // GET: api/Images
        /// <summary>
        /// Searches for container with current date, then combines all images into one and uploads
        /// </summary>
        public string GetCombineImagesInBlob()
        {
           var    abm = new AzureBlobManager();
           return abm.CombineImgAndUploadToBlob();
        }
```

Which calls:

```csharp
        /// <summary>
        /// All functionality required to upload images from blob storage
        /// </summary>
        public string CombineImgAndUploadToBlob()
        {
            // Grab all images from container matching today's date
            var sContainerName = AppendDateToName(ROOT_CONTAINER_NAME              );
            GetAllBlobsInContainerAsCloudBlob(sContainerName                       );
            ConvertBlobs(sContainerName                                            );
            var combinedImg    = CombineImages(this.ImageList                      );
            var imgAsBytes     = combinedImg.ToByteArray(                          );
            var sFileName      = PrependDateToNameJpg("ImageOfDay"                 );

            // Check if container exists base on today's date
            if (DoesContainerExist(sContainerName) == true)
            {
                PutBlobAsByteArray(sContainerName, sFileName, imgAsBytes);
            }
            else
            {
                CreateContainer(sContainerName);
                PutBlobAsByteArray(sContainerName, sFileName, imgAsBytes);
            }
            ret
```

----------
## Running this on a scheduler

I have the call to *api/Images* being run on an [Azure Scheduler](https://azure.microsoft.com/en-us/services/scheduler/) every 24 hours. What this does it call that endpoint with an **HTTP GET** request, which fires off the **CombineImgAndUploadToBlob()** function.

With this, all of the images in the day's container are merged into one image, and re-uploaded so that I can analyze it later.

  [1]: http://www.daveVoyles.com "My website"

