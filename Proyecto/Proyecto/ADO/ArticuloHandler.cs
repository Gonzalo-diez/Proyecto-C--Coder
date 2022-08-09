using Proyecto.Modelos;
using System.Data;
using System.Data.SqlClient;

namespace Proyecto.ADO.NET
{
    public class ArticuloHandler : DbHandler
    {
        public Articulo GetById(int id)
        {
            List<Articulo> articulos = new List<Articulo>();
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.Connection.Open();
                    sqlCommand.CommandText = "select * from Articulo where Id = @id;";

                    sqlCommand.Parameters.AddWithValue("@id", id);

                    SqlDataAdapter dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = sqlCommand;
                    DataTable table = new DataTable();
                    dataAdapter.Fill(table); 
                    sqlCommand.Connection.Close();
                    foreach (DataRow row in table.Rows)
                    {
                        Articulo articulo = new Articulo();
                        articulo.Id = Convert.ToInt32(row["Id"]);
                        articulo.Descripciones = row["Descripciones"]?.ToString();
                        articulo.Costo = Convert.ToDouble(row["Costo"]);
                        articulo.PrecioVenta = Convert.ToDouble(row["PrecioVenta"]);
                        articulo.Stock = Convert.ToInt32(row["Stock"]);
                        articulo.IdUsuario = Convert.ToInt32(row["IdUsuario"]);

                        articulos.Add(articulo);
                    }
                }
            }

            return articulos?.FirstOrDefault();
        }

        public List<Articulo> GetProductos()
        {
            List<Articulo> articulos = new List<Articulo>();
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(
                    "SELECT * FROM Producto", sqlConnection))
                {
                    sqlConnection.Open();

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        // Me aseguro que haya filas que leer
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                Articulo articulo = new Articulo();
                                articulo.Id = Convert.ToInt32(dataReader["Id"]);
                                articulo.Stock = Convert.ToInt32(dataReader["Stock"]);
                                articulo.IdUsuario = Convert.ToInt32(dataReader["IdUsuario"]);
                                articulo.Costo = Convert.ToInt32(dataReader["Costo"]);
                                articulo.PrecioVenta = Convert.ToInt32(dataReader["PrecioVenta"]);
                                articulo.Descripciones = dataReader["Descripciones"].ToString();

                                articulos.Add(articulo);
                            }
                        }
                    }

                    sqlConnection.Close();
                }
            }

            return articulos;
        }

        public List<string> GetTodasLasDescripcionesConDataAdapter()
        {
            List<string> descripciones = new List<string>();
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT * FROM Articulo", sqlConnection);

                sqlConnection.Open();

                DataSet resultado = new DataSet();
                sqlDataAdapter.Fill(resultado);

                sqlConnection.Close();
            }

            return descripciones;
        }

        public List<string> GetTodasLasDescripcionesConDataReader()
        {
            List<string> descripciones = new List<string>();
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(
                    "SELECT * FROM Articulo", sqlConnection))
                {
                    sqlConnection.Open();

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        // Me aseguro que haya filas que leer
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                string descripcionProducto = dataReader.GetString(1);
                                descripciones.Add(descripcionProducto);
                            }
                        }
                    }

                    sqlConnection.Close();
                }
            }

            return descripciones;
        }

        public void BorrarUnProducto(int idProducto)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string queryDelete = "DELETE FROM [FinalDatabase].[dbo].[Articulo] WHERE Id = @idProducto";

                SqlParameter sqlParameter = new SqlParameter("idProducto", SqlDbType.BigInt);
                sqlParameter.Value = idProducto;

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryDelete, sqlConnection))
                {
                    sqlCommand.Parameters.Add(sqlParameter);
                    sqlCommand.ExecuteScalar(); 
                }

                sqlConnection.Close();
            }
        }

        public void AgregarProducto(Articulo articulo)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string queryDelete = "INSERT INTO [FinalDatabase].[dbo].[Articulos] " +
                    "(Descripciones, Costo, PrecioVenta, Stock, IdUsuario) " +
                    "VALUES (@Descripciones, @Costo, @PrecioVenta, @Stock, @IdUsuario);";

                SqlParameter descripcionesParameter = new SqlParameter("Descripciones", SqlDbType.VarChar) { Value = articulo.Descripciones };
                SqlParameter costoParameter = new SqlParameter("Costo", SqlDbType.Int) { Value = articulo.Costo };
                SqlParameter precioVentaParameter = new SqlParameter("PrecioVenta", SqlDbType.Int) { Value = articulo.PrecioVenta };
                SqlParameter stockParameter = new SqlParameter("Stock", SqlDbType.Int) { Value = articulo.Stock };
                SqlParameter idUsuarioParameter = new SqlParameter("IdUsuario", SqlDbType.Int) { Value = articulo.IdUsuario };

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryDelete, sqlConnection))
                {
                    sqlCommand.Parameters.Add(descripcionesParameter);
                    sqlCommand.Parameters.Add(costoParameter);
                    sqlCommand.Parameters.Add(precioVentaParameter);
                    sqlCommand.Parameters.Add(stockParameter);
                    sqlCommand.Parameters.Add(idUsuarioParameter);
                    sqlCommand.ExecuteNonQuery(); // aca se ejecuta el insert
                }

                sqlConnection.Close();
            }
        }
    }
}