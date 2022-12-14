using Proyecto.Modelos;
using System.Data;
using System.Data.SqlClient;

namespace Proyecto.ADO.NET
{
    public class ProductoHandler : DbHandler
    {
        public Producto GetById(int id)
        {
            List<Producto> productos = new List<Producto>();
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.Connection.Open();
                    sqlCommand.CommandText = "SELECT * FROM Producto where Id = @id;";

                    sqlCommand.Parameters.AddWithValue("@id", id);

                    SqlDataAdapter dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = sqlCommand;
                    DataTable table = new DataTable();
                    dataAdapter.Fill(table); 
                    sqlCommand.Connection.Close();
                    foreach (DataRow row in table.Rows)
                    {
                        Producto producto = new Producto();
                        producto.Id = Convert.ToInt32(row["Id"]);
                        producto.Descripciones = row["Descripciones"]?.ToString();
                        producto.Costo = Convert.ToDouble(row["Costo"]);
                        producto.PrecioVenta = Convert.ToDouble(row["PrecioVenta"]);
                        producto.Stock = Convert.ToInt32(row["Stock"]);
                        producto.IdUsuario = Convert.ToInt32(row["IdUsuario"]);

                        productos.Add(producto);
                    }
                }
            }

            return productos?.FirstOrDefault();
        }

        public static List<Producto> GetProducto(int IdUsuario)
        {
            List<Producto> productos = new List<Producto>();
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string queryBuscarProducto = "SELECT * FROM Producto WHERE IdUsuario = @usuario";

                SqlParameter parametroProducto = new SqlParameter();
                parametroProducto.ParameterName = "IdUsuario";
                parametroProducto.SqlDbType = SqlDbType.BigInt;
                parametroProducto.Value = IdUsuario;

                using (SqlCommand sqlCommand = new SqlCommand(queryBuscarProducto, sqlConnection))
                {
                    sqlConnection.Open();

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        // Me aseguro que haya filas que leer
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                Producto producto = new Producto();
                                producto.Id = Convert.ToInt32(dataReader["Id"]);
                                producto.Stock = Convert.ToInt32(dataReader["Stock"]);
                                producto.IdUsuario = Convert.ToInt32(dataReader["IdUsuario"]);
                                producto.Costo = Convert.ToInt32(dataReader["Costo"]);
                                producto.PrecioVenta = Convert.ToInt32(dataReader["PrecioVenta"]);
                                producto.Descripciones = dataReader["Descripciones"].ToString();

                                productos.Add(producto);
                            }
                        }
                    }
                    sqlConnection.Close();
                }
            }

            return productos;
        }

        public List<string> GetTodasLasDescripcionesConDataAdapter()
        {
            List<string> descripciones = new List<string>();
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT * FROM Producto", sqlConnection);

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
                string queryDelete = "DELETE FROM [SistemaGestion].[dbo].[Producto] WHERE Id = @IdProducto";

                SqlParameter borrarParametro = new SqlParameter();
                borrarParametro.ParameterName = "IdProducto";
                borrarParametro.SqlDbType = SqlDbType.BigInt;
                borrarParametro.Value = idProducto;

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryDelete, sqlConnection))
                {
                    sqlCommand.Parameters.Add(borrarParametro);
                    sqlCommand.ExecuteScalar(); 
                }

                sqlConnection.Close();
            }
        }

        public void AgregarProducto(Producto producto)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string queryDelete = "INSERT INTO [SistemaGestion].[dbo].[Producto] " +
                    "(Descripciones, Costo, PrecioVenta, Stock, IdUsuario) " +
                    "VALUES (@Descripciones, @Costo, @PrecioVenta, @Stock, @IdUsuario);";

                SqlParameter descripcionesParameter = new SqlParameter();
                descripcionesParameter.ParameterName = "Descripciones";
                descripcionesParameter.SqlDbType = SqlDbType.VarChar;
                descripcionesParameter.Value = producto.Descripciones;

                SqlParameter costoParameter = new SqlParameter();
                costoParameter.ParameterName = "Costo";
                costoParameter.SqlDbType = SqlDbType.Int;
                costoParameter.Value = producto.Costo;

                SqlParameter precioVentaParameter = new SqlParameter();
                precioVentaParameter.ParameterName = "PrecioVenta";
                precioVentaParameter.SqlDbType = SqlDbType.Int;
                precioVentaParameter.Value = producto.PrecioVenta;

                SqlParameter stockParameter = new SqlParameter();
                stockParameter.ParameterName = "Stock";
                stockParameter.SqlDbType = SqlDbType.Int;
                stockParameter.Value = producto.Stock;

                SqlParameter idUsuarioParameter = new SqlParameter();
                idUsuarioParameter.ParameterName = "IdUsuario";
                idUsuarioParameter.SqlDbType= SqlDbType.Int;
                idUsuarioParameter.Value = producto.IdUsuario;

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