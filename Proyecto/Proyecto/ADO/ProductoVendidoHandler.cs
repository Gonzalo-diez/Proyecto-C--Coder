using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Proyecto.Modelos;
using Proyecto.ADO.NET;

namespace Proyecto.ADO
{
    public class ProductoVendidoHandler : DbHandler
    {
        public static List<ProductoVendido> GetProductosVendidos(int id)
        {
            List<ProductoVendido> ListProductoVendidos = new List<ProductoVendido>();
            List<Producto> listProducto = ProductoHandler.GetProducto(id);
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    foreach (Producto producto in listProducto)
                    {
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.Connection.Open();
                        sqlCommand.CommandText = @"select * from ProductoVendido" +
                                                "where IdProducto = @idProducto";

                        sqlCommand.Parameters.AddWithValue("@idProducto", producto.Id);


                        SqlDataAdapter dataAdapter = new SqlDataAdapter();
                        dataAdapter.SelectCommand = sqlCommand;
                        DataTable table = new DataTable();
                        dataAdapter.Fill(table);
                        sqlCommand.Parameters.Clear();

                        foreach (DataRow row in table.Rows)
                        {
                            ProductoVendido productoVendido = new ProductoVendido();
                            productoVendido.Id = Convert.ToInt32(row["Id"]);
                            productoVendido.Stock = Convert.ToInt32(row["Stock"]);
                            productoVendido.IdProducto = Convert.ToInt32(row["IdProducto"]);
                            productoVendido.IdVenta = Convert.ToInt32(row["IdVenta"]);

                            ListProductoVendidos.Add(productoVendido);
                        }
                        sqlCommand.Connection.Close();
                    }
                }
            }
            return ListProductoVendidos;
        }
    }
}

