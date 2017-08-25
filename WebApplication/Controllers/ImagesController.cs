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
        public string Get()
        {
           // Create instance of Azure Blob Manager and run task to update images
           var    abm = new AzureBlobManager();
           return abm.CombineImgAndUploadToBlob("dumpster");
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
