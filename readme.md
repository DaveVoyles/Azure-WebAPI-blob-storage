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


That information can be found in your Azure Portal, Azure Storage Explorer, or through the Functions CLI. Here is where it appears in the portal:

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

But so does the **BlobStorageMultipartStreamProvider** with this line:

```csharp
var connectionString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
```

### If hosting this in azure
Create your connection string in the configuration tool there. [Documentation.](https://azure.microsoft.com/en-us/blog/windows-azure-web-sites-how-application-strings-and-connection-strings-work/)


### Passing an image to the function

This is done by making an HTTP POST request. Inside the body of the message, we want to pass in JSON content, with a key/value pair like this:

```json
{
	"name":"http://snoopdogg.com/wp-content/themes/snoop_2014/assets/images/og-img.jpg"
}
```

In a tool such as Postman, I would write my message like so:

![Imgur](http://i.imgur.com/mjUb0DS.png)

The URL to send the message is found in the console when you start your Azure Function app. 
![Imgur](http://i.imgur.com/wy8ABfa.png)


This takes the name of the image, pre-pends the current date, and saves the image to blob storage. 

  [1]: http://www.daveVoyles.com "My website"

