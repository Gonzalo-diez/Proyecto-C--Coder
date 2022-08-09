using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Proyecto.Modelos;
using Proyecto.ADO.NET;

namespace Proyecto.ADO
{
    public class ArticuloVendidoHandler : DbHandler
    {
        public static List<ArticuloVendido> GetArticulosVendidos(int id)
        {
            List<ArticuloVendido> ListArticulosVendidos = new List<ArticuloVendido>();
            List<Articulo> listArticulos = ArticuloHandler.GetProductos(id);
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    foreach (Articulo producto in listArticulos)
                    {
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.Connection.Open();
                        sqlCommand.CommandText = @"select * from ArticuloVendido
                                                where IdProducto = @idProducto";
                        
                        sqlCommand.Parameters.AddWithValue("@idProducto", producto.Id);
                        

                        SqlDataAdapter dataAdapter = new SqlDataAdapter();
                        dataAdapter.SelectCommand = sqlCommand;
                        DataTable table = new DataTable();
                        dataAdapter.Fill(table);
                        sqlCommand.Parameters.Clear();

                        foreach (DataRow row in table.Rows)
                        {
                            ArticuloVendido articuloVendido = new ArticuloVendido();
                            articuloVendido.Id = Convert.ToInt32(row["Id"]);
                            articuloVendido.Stock = Convert.ToInt32(row["Stock"]);
                            articuloVendido.IdProducto = Convert.ToInt32(row["IdProducto"]);
                            articuloVendido.IdVenta = Convert.ToInt32(row["IdVenta"]);

                            ListArticulosVendidos.Add(articuloVendido);
                        }
                        sqlCommand.Connection.Close();
                    }
                }
            }
            return ListArticulosVendidos;
        }
    }
}

