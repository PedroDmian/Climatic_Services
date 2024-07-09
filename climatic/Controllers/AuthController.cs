using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using climatic.Class;


namespace climatic.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthController: ApiController
    {
        [HttpPost]
        [Route("login")]
        public HttpResponseMessage login(Models.User user)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            Response<Models.User> login = Models.User.login(user);
            
            response.Content = new ObjectContent<Response<Models.User>>(login, Utiles.Formatter);

            if (login.Estatus == Estatus.Exito)
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
            }

            return response;
        }
        
        [HttpPost]
        [Route("register")]
        public HttpResponseMessage register(Models.User user)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            Response<bool> register = Models.User.register(user);
            
            response.Content = new ObjectContent<Response<bool>>(register, Utiles.Formatter);

            if (register.Estatus == Estatus.Exito)
            {
                response.StatusCode = HttpStatusCode.OK;
            }
            else if (register.Estatus == Estatus.Advertencia)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            else
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }
    }
}