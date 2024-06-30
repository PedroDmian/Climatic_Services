using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using climatic.Class;

namespace climatic.Controllers
{
    [RoutePrefix("api/help")]
    public class HelpController : ApiController
    {
        [HttpGet]
        [Route("")]
        public HttpResponseMessage help()
        {
            HttpResponseMessage response = new HttpResponseMessage();

            response.StatusCode = HttpStatusCode.NoContent;

            return response;
        }
        
    }
}