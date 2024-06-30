using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System;

namespace Data
{
    public class ConexionBD
    {
        protected SqlConnection Conexion { get; set; }
        protected SqlCommand Comando { get; set; }
        protected SqlDataAdapter Adaptador { get; set; } //Conexion offline
        protected SqlDataReader Lector { get; set; } //Conexion Online

        protected SqlTransaction Transaction { get; set; }
        public bool TransaccionIniciada { get; set; }

        public ConexionBD()
        {
            Conexion = new SqlConnection(getConnectionString());
            Comando = new SqlCommand();
            Comando.Connection = Conexion;
            Comando.CommandType = CommandType.StoredProcedure;
            Comando.CommandTimeout = 0;
            Adaptador = new SqlDataAdapter(Comando);
        }

        string getConnectionString()
        {
            var connection = "";
            var activeConnection = ConfigurationManager.AppSettings["ActiveConnection"];

            if (activeConnection == null)
            {
                throw new Exception("No se ha especificado \"ActiveConnection\" en el WebConfig");
            }
            else
            {
                connection = ConfigurationManager.ConnectionStrings[activeConnection].ConnectionString;
            }

            return connection;
        }

        public void BeginTransaction()
        {
            if (Conexion.State == ConnectionState.Closed)
            {
                Conexion.Open();
            }
            Transaction = Conexion.BeginTransaction();
            this.Comando.Transaction = Transaction;
            TransaccionIniciada = true;
        }

        public void Commit()
        {
            if (TransaccionIniciada)
            {
                Transaction.Commit();
                this.Comando.Transaction = null;
                this.Conexion.Close();
                TransaccionIniciada = false;
            }
        }

        public void Rollback()
        {
            if (TransaccionIniciada)
            {
                Transaction.Rollback();
                this.Comando.Transaction = null;
                this.Conexion.Close();
                TransaccionIniciada = false;
            }
        }

        protected void close()
        {
            if (!TransaccionIniciada)
            {
                if (Conexion.State == ConnectionState.Open)
                {
                    Conexion.Close();
                }
            }
        }

        /// <summary>
        /// Consume un SP que regresa un SELECT
        /// </summary>
        /// <param name="spName">Nombre del SP a Consumir</param>
        /// <param name="parameters">Array de SqlParameter que se pasaran como parametro al SP</param>
        /// <returns></returns>
        public DataTable GenericSPSelect(string spName, SqlParameter[] parameters = null)
        {
            DataTable data = new DataTable();

            try
            {
                if (Conexion.State == ConnectionState.Closed)
                {
                    Conexion.Open();
                }

                Comando.CommandText = spName;
                Comando.Parameters.Clear();
                if (parameters != null)
                {
                    Comando.Parameters.AddRange(parameters);
                }

                Adaptador.Fill(data);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.close();
            }

            return data;
        }

        public bool GenericBoolScalarQuery(string spName, SqlParameter[] parameters = null)
        {
            DataTable data = new DataTable();
            bool resp = false;

            try
            {
                if (Conexion.State == ConnectionState.Closed)
                {
                    Conexion.Open();
                }

                Comando.CommandText = spName;
                Comando.Parameters.Clear();
                if (parameters != null)
                {
                    Comando.Parameters.AddRange(parameters);
                }

                Adaptador.Fill(data);

                if (data.Rows.Count > 0)
                {
                    resp = data.Rows[0][0].ToString() == "1" || Convert.ToBoolean(data.Rows[0][0]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.close();
            }

            return resp;
        }

        public int GenericIntScalarQuery(string spName, SqlParameter[] parameters = null)
        {
            DataTable data = new DataTable();
            int resp = 0;

            try
            {
                if (Conexion.State == ConnectionState.Closed)
                {
                    Conexion.Open();
                }

                Comando.CommandText = spName;
                Comando.Parameters.Clear();
                if (parameters != null)
                {
                    Comando.Parameters.AddRange(parameters);
                }

                Adaptador.Fill(data);

                if (data.Rows.Count > 0)
                {
                    resp = Convert.ToInt32(data.Rows[0][0]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.close();
            }

            return resp;
        }

    }
}