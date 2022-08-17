using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Proyecto.Modelos;
using Proyecto.ADO.NET;

namespace Proyecto.ADO.NET
{
    public class VentaHandler : DbHandler
    {
        public static List<Venta> GetVenta(int IdUsuario)
        {
            List<Venta> ventas = new List<Venta>();
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string queryBuscarVentas = "SELECT * FROM Venta WHERE IdUsuario = @IdUsuario;";

                double valorId = 1;
                SqlParameter ParametroId = new SqlParameter();
                ParametroId.ParameterName = "IdUsuario";
                ParametroId.SqlDbType = SqlDbType.BigInt;
                ParametroId.Value = valorId;

                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Parameters.Add(ParametroId);
                    sqlCommand.ExecuteNonQuery();
                }
                sqlConnection.Close();
            }
            return ventas;
        }
        public static void InsertPedido(List<Producto> productos, int IdUsuario)
        {
            Venta venta = new Venta();

            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.Connection.Open();
            sqlCommand.CommandText = "INSERT INTO [SistemaGestion].[dbo].[Venta] ([Comentarios], [IdUsuario]) VALUES (@Comentarios, @IdUsuario)";

            sqlCommand.Parameters.AddWithValue("@Comentarios", "");
            sqlCommand.Parameters.AddWithValue("@IdUsuario", IdUsuario);

            sqlCommand.ExecuteNonQuery();
            venta.Id = GetId.Get(sqlCommand);
            venta.IdUsuario = IdUsuario;

            foreach (Producto producto in productos)
            {
                sqlCommand.CommandText = "INSERT INTO [SistemaGestion].[dbo].[ProductoVendido] ([Stock], [IdProducto], [IdVenta]) VALUES (@Stock, @IdProducto, @IdVenta)";

                sqlCommand.Parameters.AddWithValue("@Stock", producto.Stock);
                sqlCommand.Parameters.AddWithValue("@IdProducto", producto.Id);
                sqlCommand.Parameters.AddWithValue("@IdVenta", venta.Id);

                sqlCommand.ExecuteNonQuery(); 
                sqlCommand.Parameters.Clear();

                sqlCommand.CommandText = "UPDATE [SistemaGestion].[dbo].[Producto] SET Stock = Stock - @Stock WHERE id = @IdProducto";

                sqlCommand.Parameters.AddWithValue("@Stock", producto.Stock);
                sqlCommand.Parameters.AddWithValue("@IdProducto", producto.Id);

                sqlCommand.ExecuteNonQuery();
                sqlCommand.Parameters.Clear();
            }
            sqlCommand.Connection.Close();
        }
    }
}