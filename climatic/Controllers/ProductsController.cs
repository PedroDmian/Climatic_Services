using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using climatic.Class;

namespace climatic.Controllers
{
    [RoutePrefix("api/products")]
    public class ProductsController : ApiController
    {
        [HttpGet]
        [Route("getProducts")]
        public HttpResponseMessage getProducts()
        {
            HttpResponseMessage ressponse = new HttpResponseMessage();

            Response<List<Models.Products>> lista = Models.Products.getProducts();

            ressponse.Content = new ObjectContent<Response<List<Models.Products>>>(lista, Utiles.Formatter);

            if (lista.Estatus == Estatus.Exito)
            {
                ressponse.StatusCode = HttpStatusCode.OK;
            }
            else if (lista.Estatus == Estatus.Advertencia)
            {
                ressponse.StatusCode = HttpStatusCode.NoContent;
            }
            else
            {
                ressponse.StatusCode = HttpStatusCode.InternalServerError;
            }

            return ressponse;
        }

        [HttpPost]
        [Route("saveProducts")]
        public HttpResponseMessage saveProducts(Models.Products product)
        {
            HttpResponseMessage ressponse = new HttpResponseMessage();

            Response<bool> respuesta = Models.Products.save(product);

            if (respuesta.Estatus == Estatus.Exito)
            {
                ressponse.StatusCode = HttpStatusCode.OK;
            }
            else if (respuesta.Estatus == Estatus.Advertencia)
            {
                ressponse.StatusCode = HttpStatusCode.NoContent;
            }
            else
            {
                ressponse.StatusCode = HttpStatusCode.InternalServerError;
            }

            return ressponse;
        }
    }
}
