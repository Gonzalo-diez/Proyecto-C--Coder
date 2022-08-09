using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Proyecto.Modelos;
using Proyecto.ADO.NET;

namespace Proyecto.ADO.NET
{
    public class PedidoHandler : DbHandler
    {
        public static List<Venta> GetPedido(int id)
        {
            List<Venta> ventas = new List<Venta>();
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.Connection.Open();
                    sqlCommand.CommandText = @"select * from Pedido where IdUsuario = @idUsuario;";

                    sqlCommand.Parameters.AddWithValue("@idUsuario", id);

                    SqlDataAdapter dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = sqlCommand;
                    DataTable table = new DataTable();
                    dataAdapter.Fill(table); //Se ejecuta el Select

                    foreach (DataRow row in table.Rows)
                    {
                        Venta venta = new Venta();
                        venta.Id = Convert.ToInt32(row["Id"]);
                        venta.Comentarios = row["Comentarios"].ToString();
                        venta.IdUsuario = Convert.ToInt32(row["IdUsuario"]);

                        ventas.Add(venta);
                    }
                    sqlCommand.Connection.Close();
                }
            }
            return ventas;
        }
        public static void InsertPedido(List<Articulo> articulos, int IdUsuario)
        {
            Venta venta = new Venta();

            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.Connection.Open();
            sqlCommand.CommandText = @"INSERT INTO [FinalDatabase].[dbo].[Pedido] ([Comentarios], [IdUsuario]) VALUES (@Comentarios, @IdUsuario)";

            sqlCommand.Parameters.AddWithValue("@Comentarios", "");
            sqlCommand.Parameters.AddWithValue("@IdUsuario", IdUsuario);

            sqlCommand.ExecuteNonQuery();
            venta.Id = GetId.Get(sqlCommand);
            venta.IdUsuario = IdUsuario;

            foreach (Articulo articulo in articulos)
            {
                sqlCommand.CommandText = @"INSERT INTO [FinalDatabase].[dbo].[ArticuloVendido] ([Stock], [IdProducto], [IdVenta]) VALUES (@Stock, @IdProducto, @IdVenta)";

                sqlCommand.Parameters.AddWithValue("@Stock", articulo.Stock);
                sqlCommand.Parameters.AddWithValue("@IdProducto", articulo.Id);
                sqlCommand.Parameters.AddWithValue("@IdVenta", venta.Id);

                sqlCommand.ExecuteNonQuery(); 
                sqlCommand.Parameters.Clear();

                sqlCommand.CommandText = @" UPDATE [FinalDatabase].[dbo].[Articulo] SET Stock = Stock - @Stock WHERE id = @IdProducto";

                sqlCommand.Parameters.AddWithValue("@Stock", articulo.Stock);
                sqlCommand.Parameters.AddWithValue("@IdProducto", articulo.Id);

                sqlCommand.ExecuteNonQuery();
                sqlCommand.Parameters.Clear();
            }
            sqlCommand.Connection.Close();
        }
    }
}