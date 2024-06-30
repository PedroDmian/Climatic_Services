using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace climatic.Class
{
    public class Response<T>
    {
        public Estatus Estatus { get; set; }
        public string Mensaje { get; set; }
        public string MensajeTecnico { get; set; }
        public string Token { get; set; }
        public T Datos { get; set; }

        public Response()
        {
            this.Estatus = Estatus.Error;
            this.Mensaje = string.Empty; ;
            this.MensajeTecnico = string.Empty; ;
            this.Token = string.Empty;
            if (typeof(T).IsClass)
            {
                try
                {
                    var cons = typeof(T).GetConstructors().Any(c => c.GetParameters().Count() == 0);

                    if (cons)
                    {
                        this.Datos = (T)Activator.CreateInstance(typeof(T));
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
    }

    public enum Estatus : short
    {
        Error = -1,
        Exito = 1,
        Advertencia = 2
    }
}