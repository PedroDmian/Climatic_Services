using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using climatic.Class;
using System.Threading.Tasks;

namespace climatic.Controllers
{
    [RoutePrefix("api/weather")]
    public class WeathersController: ApiController
    {
        [HttpGet]
        [Route("")]
        public async Task<HttpResponseMessage> weather()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            
            response.StatusCode = HttpStatusCode.OK;
            
            Response<Models.Weather> weather = await Models.Weather.getWeather(1334, 223);
            
            response.Content = new ObjectContent<Response<Models.Weather>>(weather, Utiles.Formatter);

            if (weather.Estatus == Estatus.Exito)
            {
                response.StatusCode = HttpStatusCode.OK;
            }
            else if (weather.Estatus == Estatus.Advertencia)
            {
                response.StatusCode = HttpStatusCode.Unauthorized;
            }
            else
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }
    }
}