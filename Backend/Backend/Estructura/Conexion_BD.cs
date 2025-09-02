using Microsoft.Data.SqlClient;

namespace Backend.Estructura
{
    public class Conexion_BD
    {
        private readonly string _connectionString;

        public Conexion_BD(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("MiPrueba");
        }

        public SqlConnection Abrir_Conexion()
        {
            var conn = new SqlConnection(_connectionString);
            conn.Open();
            return conn;
        }
    }
}
