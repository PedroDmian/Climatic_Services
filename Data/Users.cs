using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System;


namespace Data
{
    public class Users
    {
        public ConexionBD conexion;

        public Users(ConexionBD con)
        {
            conexion = con;
        }
        
        public DataTable getUsers()
        {
            return this.conexion.GenericSPSelect("GetAllUsers");
        }

        public bool login(string email, string password)
        {
            return this.conexion.GenericBoolScalarQuery("AuthLogin", new SqlParameter[]
            {
                new SqlParameter("@email", SqlDbType.VarChar) {Value = email },
                new SqlParameter("@password", SqlDbType.VarChar) {Value = password }
            });
        }
        
        public bool userRegister(
            string email, 
            string password,
            string birthdate,
            string activity
        )
        {
            return this.conexion.GenericBoolScalarQuery("UserRegister", new SqlParameter[]
            {
                new SqlParameter("@email", SqlDbType.VarChar) {Value = email },
                new SqlParameter("@password", SqlDbType.VarChar) {Value = password },
                new SqlParameter("@birthdate", SqlDbType.VarChar) {Value = birthdate }, 
                new SqlParameter("@activity", SqlDbType.VarChar) {Value = activity }, 
            });
        }
        
        public bool validUserEmailExist(string email)
        {
            return this.conexion.GenericBoolScalarQuery("ValidUserEmailExist", new SqlParameter[]
            {
                new SqlParameter("@email", SqlDbType.VarChar) {Value = email } 
            });
        }
    }
}