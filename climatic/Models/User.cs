using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using climatic.Class;

namespace climatic.Models
{
    public class User
    {
        public string email { get; set; }
        
        public string birthdate { get; set; }
        
        public string password { get; set; }
        
        public string activity { get; set; }
        
        public string token { get; set; }
        
        public DateTime created_at { get; set; }
        
        public DateTime updated_at { get; set; }

        public static Response<List<User>> getUsers()
        {
            Response<List<User>> res = new Response<List<User>>()
            {
                Estatus = Estatus.Error
            };

            List<User> list = new List<User>();

            try
            {
                DataTable items = new Data.Users(new Data.ConexionBD()).getUsers();

                if (items.Rows.Count > 0)
                {
                    foreach (DataRow row in items.Rows)
                    {
                        User user = new User();

                        user.email = row["email"].ToString();
                        user.password = row["password"].ToString();
                        user.birthdate = row["birthdate"].ToString();
                        user.activity = row["activity"].ToString();
                        user.created_at = (DateTime)row["created_at"];
                        user.updated_at = (DateTime)row["updated_at"];

                        list.Add(user);
                    }
                }
                
                res.Datos = list;
                res.Estatus = Estatus.Exito;
            }
            catch (Exception ex)
            {
                res.Mensaje = "No se pueden obtener los usuarios en este momento.";
                res.MensajeTecnico = ex.Message;
            }
            return res;
        }

        public static Response<User> login(User user)
        {
            Response<User> res = new Response<User>()
            {
                Estatus = Estatus.Error
            };

            try
            {
                Boolean loginUser = new Data.Users(new Data.ConexionBD()).login(user.email, user.password);
                
                if (loginUser)
                {
                    User userLogin = new User();
                    userLogin.email = user.email;
                    userLogin.token = "123";
                    
                    res.Datos = userLogin;
                    res.Mensaje = "Se esta logeando correctamente";
                    res.Estatus = Estatus.Exito;
                } 
                else
                {
                    res.Datos = null;
                    res.Mensaje = "La contraseña o usuario estan incorrectamente";
                    res.Estatus = Estatus.Advertencia;
                }
            }
            catch (Exception ex)
            {
                res.Mensaje = "Hay un error al intentar logear el usuario";
                res.MensajeTecnico = ex.Message;
            }
            return res;
        }
        
        public static Response<bool> register(User user)
        {
            Response<bool> res = new Response<bool>()
            {
                Estatus = Estatus.Error
            };

            try
            {
                Boolean isExistEmail = new Data.Users(new Data.ConexionBD()).validUserEmailExist(user.email);

                if (isExistEmail)
                {
                    res.Datos = false;
                    res.Mensaje = "El correo ya existe";
                    res.Estatus = Estatus.Advertencia;

                    return res;
                }
                
                Boolean registerUser = new Data.Users(new Data.ConexionBD()).userRegister(user.email, user.password, user.birthdate, user.activity);
                
                if (registerUser)
                {
                    res.Datos = false;
                    res.Mensaje = "El usuario no pudo registrarse en este momento";
                    res.Estatus = Estatus.Advertencia;

                    return res;
                } 
                
                res.Datos = true;
                res.Estatus = Estatus.Exito;
            }
            catch (Exception ex)
            {
                res.Mensaje = "Hay un error al intentar registrar el usuario";
                res.MensajeTecnico = ex.Message;
            }
            return res;
        }
        
        public static Response<bool> validUserEmailExist(User user)
        {
            Response<bool> res = new Response<bool>()
            {
                Estatus = Estatus.Error
            };

            try
            {
                Boolean registerUser = new Data.Users(new Data.ConexionBD()).validUserEmailExist(user.email);
                
                if (registerUser)
                {
                    res.Datos = true;
                    res.Estatus = Estatus.Exito;
                } 
                else
                {
                    res.Datos = false;
                    res.Estatus = Estatus.Advertencia;
                }
            }
            catch (Exception ex)
            {
                res.Mensaje = "Hay un error al intentar validar el correo del usuario";
                res.MensajeTecnico = ex.Message;
            }
            return res;
        }
    }
}