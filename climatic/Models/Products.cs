using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using climatic.Class;

namespace climatic.Models
{
    public class Products
    {
        public string name { get; set; }
        public string description { get; set; }
        public double sales_price { get; set; }
        public double purchase_price { get; set; }
        public int status { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

        public static Response<List<Products>> getProducts()
        {
            Response<List<Products>> res = new Response<List<Products>>()
            {
                Estatus = Estatus.Error
            };

            List<Products> list = new List<Products>();

            try
            {
                DataTable items = new Data.Products(new Data.ConexionBD()).getProducts();

                if (items.Rows.Count > 0)
                {
                    foreach (DataRow row in items.Rows)
                    {
                        Products product = new Products();
                        product.name = row["name"].ToString();
                        product.description = row["description"].ToString();
                        product.sales_price = Convert.ToDouble(row["sales_price"]);
                        product.purchase_price = Convert.ToDouble(row["purchase_price"]);
                        product.status = Convert.ToInt32(row["status"]);
                        product.created_at = (DateTime)row["created_at"];
                        product.updated_at = (DateTime)row["updated_at"];

                        list.Add(product);
                    }

                    res.Datos = list;
                    res.Estatus = Estatus.Exito;
                }
            }
            catch (Exception ex)
            {
                res.Mensaje = "No se pueden obtener los productos en este momento.";
                res.MensajeTecnico = ex.Message;
            }
            return res;
        }

        public static Response<bool> save(Products product)
        {
            Response<bool> res = new Response<bool>()
            {
                Estatus = Estatus.Error
            };

            try
            {
                bool saveProduct = new Data.Products(new Data.ConexionBD()).save(product.name, product.description, product.sales_price, product.purchase_price, product.status);

                if (saveProduct)
                {
                    res.Datos = true;
                    res.Estatus = Estatus.Exito;
                } else
                {
                    res.Datos = false;
                    res.Estatus = Estatus.Advertencia;
                }
            }
            catch (Exception ex)
            {
                res.Mensaje = "No se pueden obtener los productos en este momento.";
                res.MensajeTecnico = ex.Message;
            }
            return res;
        }
    }
}