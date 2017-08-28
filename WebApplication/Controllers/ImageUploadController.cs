using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplication.Classes;

namespace WebApplication.Controllers
{
    public class ImageUploadController : ApiController
    {
        public async Task<HttpResponseMessage> PostFormData()
        {
            // Must be correct media type
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            try
            {
                // Main part of the function
                MultipartStreamProvider provider = new BlobStorageMultipartStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
