using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication.Classes;

namespace WebApplication.Controllers
{
    public class ImagesController : ApiController
    {
        // GET: api/Images
        /// <summary>
        /// Searches for container with current date, then combines all images into one and uploads
        /// </summary>
        public string GetCombineImagesInBlob()
        {
           var    abm = new AzureBlobManager();
           return abm.CombineImgAndUploadToBlob();
        }


        // GET: api/Images/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Images
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Images/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Images/5
        public void Delete(int id)
        {
        }
    }
}
