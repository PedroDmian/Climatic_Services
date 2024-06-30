using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System;

namespace Data
{
    public class Products
    {
        public ConexionBD conexion;
        public Products(ConexionBD con)
        {
            conexion = con;
        }

        public DataTable getProducts()
        {
            return this.conexion.GenericSPSelect("GetProducts");
        }

        public bool save(string name, string description, double sales_price = 0, double purchase_price = 0, int status = 1)
        {
            return this.conexion.GenericBoolScalarQuery("AddProduct", new SqlParameter[]
            {
                new SqlParameter("@name", SqlDbType.Int) {Value = name },
                new SqlParameter("@description", SqlDbType.Int) {Value = description },
                new SqlParameter("@sales_price", SqlDbType.Int) {Value = sales_price },
                new SqlParameter("@purchase_price", SqlDbType.Int) {Value = purchase_price },
                new SqlParameter("@status", SqlDbType.Int) {Value = status }
            });
        }
    }
}