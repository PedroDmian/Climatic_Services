using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using climatic.Class;

namespace climatic.Controllers
{
    [RoutePrefix("api/weather")]
    public class WeathersController: ApiController
    {
        [HttpGet]
        [Route("")]
        public HttpResponseMessage weather()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            
            response.StatusCode = HttpStatusCode.OK;

            /*Response<Models.User> login = Models.User.login(user);
            
            response.Content = new ObjectContent<Response<Models.User>>(login, Utiles.Formatter);*/

            /*if (login.Estatus == Estatus.Exito)
            {
                response.StatusCode = HttpStatusCode.OK;
            }
            else if (login.Estatus == Estatus.Advertencia)
            {
                response.StatusCode = HttpStatusCode.Unauthorized;
            }
            else
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
            }*/

            return response;
        }
    }
}