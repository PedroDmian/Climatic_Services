using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;
using System.Linq;
using System.Data;
using System.Net.Http;
using System.Collections.Concurrent;
using System.Globalization;
using System.Net.Http.Headers;
using System.IO;
using System.Web;
using System.Configuration;

namespace climatic.Class
{
    public class Utiles
    {
        public delegate Response<T> Query<T>();

        public delegate Response<T> Obtener<T>(int id);
        public delegate Response<TReturn> Actualizar<TReturn, TValue>(TValue obj);

        public static Response<TReturn> AplicarPatch<TReturn, TValue>(Obtener<TValue> fnObtener, Actualizar<TReturn, TValue> fnActualizar, int id, TValue patch)
        {
            Response<TReturn> resp = new Response<TReturn>();

            try
            {
                var origValue = fnObtener(id).Datos;

                ConcurrentDictionary<Type, PropertyInfo[]> TypePropertiesCache = new ConcurrentDictionary<Type, PropertyInfo[]>();

                PropertyInfo[] properties = TypePropertiesCache.GetOrAdd(patch.GetType(), (type) => type.GetProperties(BindingFlags.Instance | BindingFlags.Public));

                foreach (PropertyInfo prop in properties)
                {
                    PropertyInfo orjProp = origValue.GetType().GetProperty(prop.Name);
                    object value = prop.GetValue(patch);
                    if (value != null)
                    {
                        orjProp.SetValue(origValue, value);
                    }
                }

                resp = fnActualizar(origValue);
                //resp.Estatus = Estatus.Exito;
            }
            catch (Exception ex)
            {
                resp.Mensaje = "No se puede aplicar el parche";
                resp.MensajeTecnico = ex.Message;
            }

            return resp;
        }

        public static HttpResponseMessage RespuestaGeneralServicio<T>(Query<T> query,
            HttpStatusCode exito = HttpStatusCode.OK,
            HttpStatusCode advertencia = HttpStatusCode.NoContent,
            HttpStatusCode error = HttpStatusCode.InternalServerError
        )
        {
            HttpResponseMessage message = new HttpResponseMessage();

            var resp = query();

            message.Content = new ObjectContent<Response<T>>(resp, Utiles.Formatter);

            if (resp.Estatus == Estatus.Exito)
            {
                message.StatusCode = exito;
            }
            else if (resp.Estatus == Estatus.Advertencia)
            {
                message.StatusCode = advertencia;
            }
            else
            {
                message.StatusCode = error;
            }

            return message;
        }

        static CultureInfo culture = new CultureInfo("es-MX");

        public static CultureInfo FormatDateProvider { get; set; }

        static JsonMediaTypeFormatter formatter = new JsonMediaTypeFormatter();

        public static JsonMediaTypeFormatter Formatter { get { return formatter; } }

        public static T Cast<T>(Object myobj)
        {
            if (myobj != null)
            {
                Type objectType = myobj.GetType();
                Type target = typeof(T);
                var x = Activator.CreateInstance(target, false);
                var z = from source in objectType.GetMembers().ToList()
                        where source.MemberType == MemberTypes.Property
                        select source;
                var d = from source in target.GetMembers().ToList()
                        where source.MemberType == MemberTypes.Property
                        select source;
                List<MemberInfo> members = d.Where(memberInfo => d.Select(c => c.Name).ToList().Contains(memberInfo.Name)).ToList();
                PropertyInfo propertyInfo;
                object value;
                foreach (var memberInfo in members)
                {
                    propertyInfo = typeof(T).GetProperty(memberInfo.Name);
                    value = myobj.GetType().GetProperty(memberInfo.Name).GetValue(myobj, null);

                    propertyInfo.SetValue(x, value, null);
                }
                return (T)x;
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// Copy an object to destination object, only matching fields will be copied
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceObject">An object with matching fields of the destination object</param>
        /// <param name="destObject">Destination object, must already be created</param>
        public static void CopyObject<T>(object sourceObject, ref T destObject)
        {
            //  If either the source, or destination is null, return
            if (sourceObject == null || destObject == null)
                return;

            //  Get the type of each object
            Type sourceType = sourceObject.GetType();
            Type targetType = destObject.GetType();

            //  Loop through the source properties
            foreach (PropertyInfo p in sourceType.GetProperties())
            {
                //  Get the matching property in the destination object
                PropertyInfo targetObj = targetType.GetProperty(p.Name);
                //  If there is none, skip
                if (targetObj == null)
                    continue;

                //  Set the value in the destination
                targetObj.SetValue(destObject, p.GetValue(sourceObject, null), null);
            }
        }

        public static string MD5(string source)
        {
            StringBuilder builder = new StringBuilder();

            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(source);
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();

            byte[] hash = md5.ComputeHash(bytes);

            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("X2"));
            }

            return builder.ToString().ToLower();
        }

        public static string GenerarToken()
        {
            StringBuilder builder = new StringBuilder();
            string seed = DateTime.Now.ToString("yyyyMMddHHmmssffff");

            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(seed);
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();

            byte[] hash = md5.ComputeHash(bytes);

            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("X2"));
            }

            return builder.ToString().ToLower();
        }

        public static string ConvertToJson(object obj)
        {
            return new JavaScriptSerializer().Serialize(obj);
        }

        public static string ConvertIPToHost(string ip)
        {
            string host = "";

            try
            {
                IPHostEntry entrys = Dns.GetHostEntry(ip);

                host = entrys.HostName;
            }
            catch (Exception ex)
            {
                host = "";
            }

            return host;
        }

        public static string ConvertHostToIP(string host)
        {
            string ip = "";
            try
            {
                IPHostEntry entrys = Dns.GetHostEntry(host);

                if (entrys.AddressList.Length > 0)
                {
                    ip = entrys.AddressList[0].ToString();
                }
            }
            catch (Exception ex)
            {
                ip = "";
            }

            return ip;
        }

        static string getAppConfig(string key)
        {
            string value = "";

            try
            {
                value = System.Configuration.ConfigurationManager.AppSettings.Get(key);

                if (value == null)
                {
                    value = "";
                }
            }
            catch (Exception ex)
            {
                value = "";
            }


            return value;
        }

        public static string GuardarImagen(string base64Img)
        {
            bool guardarImagen = bool.Parse(ConfigurationManager.AppSettings["GuardarImagenes"]);
            if (base64Img != null && guardarImagen)
            {
                base64Img = base64Img.Replace("\n", "");

                string hash = Utiles.MD5(base64Img);

                string rutaImagen = getAppConfig("imgPath") + "\\";

                rutaImagen += DateTime.Now.ToString("yyyy");
                if (!Directory.Exists(rutaImagen))
                {
                    Directory.CreateDirectory(rutaImagen);
                }
                rutaImagen += "\\";

                rutaImagen += DateTime.Now.ToString("MM");
                if (!Directory.Exists(rutaImagen))
                {
                    Directory.CreateDirectory(rutaImagen);
                }

                rutaImagen += "\\";

                rutaImagen += DateTime.Now.ToString("dd");
                if (!Directory.Exists(rutaImagen))
                {
                    Directory.CreateDirectory(rutaImagen);
                }

                rutaImagen += "\\";

                rutaImagen += DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + "_" + hash + ".png";

                byte[] img = Convert.FromBase64String(base64Img);

                System.IO.FileStream imagen = System.IO.File.Open(rutaImagen, System.IO.FileMode.Create, System.IO.FileAccess.Write);

                imagen.Write(img, 0, img.Length);

                imagen.Flush();
                imagen.Close();

                return rutaImagen;
            }
            else
            {
                return "";
            }
        }


        public static Nullable<T> NullOrValue<T>(DataRow row, string key) where T : struct
        {
            return DBNull.Value.Equals(row[key]) ? new Nullable<T>() : row.Field<T>(key);
        }

        public static string GetUserFromHeaders(HttpRequestHeaders headers, bool useFake = false)
        {
            string usuario = "";

            usuario = headers.GetValues("user").FirstOrDefault();

            if (useFake && headers.Contains("personificado") && headers.GetValues("personificado").FirstOrDefault() != null)
            {
                usuario = headers.GetValues("personificado").FirstOrDefault();
            }

            return usuario;
        }

        public static T? GetQueryParamOrNull<T>(HttpRequestMessage request, string key) where T : struct
        {
            List<KeyValuePair<string, string>> lista = request.GetQueryNameValuePairs().ToList();

            KeyValuePair<string, string> param = lista.FirstOrDefault(p => p.Key == key);

            T? resp = null;

            if (param.Value != null)
            {
                if (typeof(T) == typeof(DateTime))
                {
                    DateTime conv;
                    if (DateTime.TryParse(param.Value, FormatDateProvider, DateTimeStyles.None, out conv))
                    {
                        resp = (T)Convert.ChangeType(conv, typeof(T));
                    }
                    else
                    {
                        resp = null;
                    }
                }
                else
                {
                    resp = (T)Convert.ChangeType(param.Value, typeof(T));
                }
            }

            return resp;
        }

        public static string GetQueryParamOrNull(HttpRequestMessage request, string key)
        {
            List<KeyValuePair<string, string>> lista = request.GetQueryNameValuePairs().ToList();

            KeyValuePair<string, string> param = lista.FirstOrDefault(p => p.Key == key);

            string resp = null;

            if (param.Value != null)
            {
                resp = param.Value;
            }

            return resp;
        }

        public static T GetQueryParamOrDefaultValue<T>(HttpRequestMessage request, string key)
        {
            return GetQueryParamOrDefaultValue(request, key, default(T));
        }

        public static T GetQueryParamOrDefaultValue<T>(HttpRequestMessage request, string key, T defValue)
        {
            List<KeyValuePair<string, string>> lista = request.GetQueryNameValuePairs().ToList();

            KeyValuePair<string, string> param = lista.FirstOrDefault(p => p.Key == key);

            T resp = defValue;

            if (param.Value != null)
            {
                resp = (T)Convert.ChangeType(param.Value, typeof(T));
            }

            return resp;
        }

        public static bool QueryParamIsPresent(HttpRequestMessage request, string key)
        {
            return request.GetQueryNameValuePairs().ToList().Count(kv => kv.Key == key) > 0;
        }


        /*
        public static string GuardarTemporalmenteArchivo(Stream stream, string nombreArchivo = null, string extencion = null)
        {
            string ruta = "C:\\ReportesDH\\";
            nombreArchivo = nombreArchivo ?? Guid.NewGuid().ToString();
            extencion = extencion ?? ".xlsx";

            if (!Directory.Exists(ruta))
            {
                Directory.CreateDirectory(ruta);
            }

            FileStream archivo = File.Create(ruta + nombreArchivo + extencion);
            BinaryReader lector = new BinaryReader(stream);
            BinaryWriter escritor = new BinaryWriter(archivo);

            escritor.Write(lector.ReadBytes((int)stream.Length));

            lector.Close();
            escritor.Flush();
            escritor.Close();
            stream.Flush();
            stream.Close();

            return nombreArchivo;
        }*/
    }
}