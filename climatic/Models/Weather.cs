using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using climatic.Class;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace climatic.Models
{
    public class Weather
    {
        private static readonly HttpClient client = new HttpClient();
        
        public string temperature { get; set; }
        public string min { get; set; }
        public string max { get; set; }
        public string airQuality { get; set; }
        
        public string wind { get; set; }
        public string humidity { get; set; }
        public string precipitation_pro { get; set; }

        public async static Task<Response<Weather>> getWeather(int latitud, int longitud)
        {
            Response<Weather> res = new Response<Weather>()
            {
                Estatus = Estatus.Error
            };
            
            Models.Weather weather = new Models.Weather()
            {
                temperature = "28",
                min = "20",
                max = "30",
                airQuality = "23",
                wind = "11",
                humidity = "72",
                precipitation_pro = "7"
            };
            
            res.Datos = weather;
            res.Mensaje = "Se obtuvo el weather correctamente";
            res.Estatus = Estatus.Exito;

            /*try
            {
                string apiKey = "o2v9L5xFPSzgNdcn";
                string url = $"http://my.meteoblue.com/packages/basic-1h_basic-day?lat={latitud}&lon={longitud}&apikey={apiKey}";
                
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                
                Weather weatherData = JsonConvert.DeserializeObject<Weather>(responseBody);

                return null;
            }
            catch (Exception ex)
            {
                res.Mensaje = "Hay un error al intentar registrar el usuario";
                res.MensajeTecnico = ex.Message;
            }*/

            return res;
        }
    }
}