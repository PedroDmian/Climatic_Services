using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using climatic.Class;

namespace climatic.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController: ApiController
    {
        [HttpGet]
        [Route("")]
        public HttpResponseMessage getUsers()
        {
            HttpResponseMessage response = new HttpResponseMessage();

            Response<List<Models.User>> list = Models.User.getUsers();

            response.Content = new ObjectContent<Response<List<Models.User>>>(list, Utiles.Formatter);

            if (list.Estatus == Estatus.Exito)
            {
                response.StatusCode = HttpStatusCode.OK;
            }
            else if (list.Estatus == Estatus.Advertencia)
            {
                response.StatusCode = HttpStatusCode.NoContent;
            }
            else
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }
    }
}