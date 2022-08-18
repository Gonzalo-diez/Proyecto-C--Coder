using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Proyecto.ADO.NET;
using Proyecto.Modelos;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace Proyecto.ADO.NET
{
    public class UsuarioHandler : DbHandler
    {
        public static List<Usuario> GetUsuarios(DataTable table)
        {
            List<Usuario> usuarios = new List<Usuario>();
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string mostrarUsuarios = "SELECT * FROM Usuario";

                using (SqlCommand sqlCommand = new SqlCommand(mostrarUsuarios, sqlConnection))
                {
                    sqlConnection.Open();

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        // Me aseguro que haya filas que leer
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                Usuario usuario = new Usuario();

                                usuario.Id = Convert.ToInt32(dataReader["Id"]);
                                usuario.NombreUsuario = dataReader["NombreUsuario"].ToString();
                                usuario.Nombre = dataReader["Nombre"].ToString();
                                usuario.Apellido = dataReader["Apellido"].ToString();
                                usuario.Contraseña = dataReader["Contraseña"].ToString();
                                usuario.Mail = dataReader["Mail"].ToString();
                            }
                        }
                    }
                    sqlConnection.Close();
                }
            }

            return usuarios;
        }

        public static Usuario GetUsuarioByContraseña(string NombreUsuario, string Contraseña)
        {
            Usuario usuario = new Usuario();
            using (SqlConnection SqlConnection = new SqlConnection(ConnectionString))
            {
                string queryBusqueda = "SELECT * FROM Usuario WHERE NombreUsuario = @NombreUsuario AND Contraseña = @Contraseña;";
                
                SqlParameter parametroNombre = new SqlParameter();
                parametroNombre.ParameterName = NombreUsuario;
                parametroNombre.SqlDbType = SqlDbType.VarChar;
                parametroNombre.Value = usuario.NombreUsuario;

                SqlParameter parametroContraseña = new SqlParameter();
                parametroContraseña.ParameterName = Contraseña;
                parametroContraseña.SqlDbType = SqlDbType.VarChar;
                parametroContraseña.Value = usuario.Contraseña;

                SqlConnection.Open();
                using (SqlCommand Sqlcommand = new SqlCommand(queryBusqueda, SqlConnection))
                {
                    Sqlcommand.Parameters.Add(parametroNombre);
                    Sqlcommand.Parameters.Add(parametroContraseña);
                    Sqlcommand.ExecuteNonQuery(); //Ejecuta la busqueda de usuario

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = Sqlcommand;
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    if (table.Rows.Count < 1)
                    {
                        return new Usuario();
                    }

                    List<Usuario> usuarios = GetUsuarios(table);
                    usuario = usuarios[0];
                }
                SqlConnection.Close();
            }
            return usuario;
        }

        public static Usuario GetUsuarioByUserName(string NombreUsuario)
        {
            Usuario usuario = new Usuario();
            using (SqlConnection SqlConnection = new SqlConnection(ConnectionString))
            {
                string queryBusquedaPorNombre = "SELECT * FROM Usuario WHERE NombreUsuario = @NombreUsuario;";

                SqlParameter parametroNombre = new SqlParameter();
                parametroNombre.ParameterName = NombreUsuario;
                parametroNombre.SqlDbType = SqlDbType.VarChar;
                parametroNombre.Value = usuario.NombreUsuario;

                SqlConnection.Open();
                using (SqlCommand Sqlcommand = new SqlCommand(queryBusquedaPorNombre, SqlConnection))
                {
                    Sqlcommand.Parameters.Add(parametroNombre);
                    Sqlcommand.ExecuteNonQuery();
                }
                SqlConnection.Close();
            }
            return usuario;
        }

        public void Delete(Usuario usuario)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    string queryDelete = "DELETE FROM Usuario WHERE Id = @idUsuario";

                    SqlParameter parametro = new SqlParameter();
                    parametro.ParameterName = "idUsuario";
                    parametro.SqlDbType = SqlDbType.BigInt;
                    parametro.Value = usuario.Id;

                    sqlConnection.Open();

                    using (SqlCommand sqlCommand = new SqlCommand(queryDelete, sqlConnection))
                    {
                        sqlCommand.Parameters.Add(parametro);
                        sqlCommand.ExecuteNonQuery(); // Se ejecuta el delete
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void UpdateContraseña(Usuario usuario)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    string queryUpdate = "UPDATE [SistemaGestion].[dbo].[Usuario] SET Contraseña = @nuevaContraseña WHERE Id = @idUsuario;";

                    SqlParameter parametroNuevaContraseña = new SqlParameter();
                    parametroNuevaContraseña.ParameterName = "nuevaContraseña";
                    parametroNuevaContraseña.SqlDbType = SqlDbType.VarChar;
                    parametroNuevaContraseña.Value = usuario.Contraseña;

                    SqlParameter parametroUsuarioId = new SqlParameter();
                    parametroUsuarioId.ParameterName = "idUsuario";
                    parametroUsuarioId.SqlDbType = SqlDbType.BigInt;
                    parametroUsuarioId.Value = usuario.Id;

                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand(queryUpdate, sqlConnection))
                    {
                        sqlCommand.Parameters.Add(parametroUsuarioId);
                        sqlCommand.Parameters.Add(parametroNuevaContraseña);
                        sqlCommand.ExecuteNonQuery(); // Se ejecuta el update
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void Insert(Usuario usuario)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    string queryInsert = "INSERT INTO [SistemaGestion].[dbo].[Usuario] " +
                        "(Nombre, Apellido, NombreUsuario, Contraseña, Mail) VALUES " +
                        "('Rodrigo', 'Perez', 'rperez', 'ContraseñaDePerez', 'RPerez@gmail.com');";

                    SqlParameter nombreParameter = new SqlParameter();
                    nombreParameter.ParameterName = "Nombre";
                    nombreParameter.SqlDbType = SqlDbType.VarChar;
                    nombreParameter.Value = usuario.Nombre;

                    SqlParameter apellidoParameter = new SqlParameter();
                    apellidoParameter.ParameterName = "Apellido";
                    apellidoParameter.SqlDbType = SqlDbType.VarChar;
                    apellidoParameter.Value = usuario.Apellido;

                    SqlParameter nombreUsuarioParameter = new SqlParameter();
                    nombreUsuarioParameter.ParameterName = "NombreUsuario";
                    nombreUsuarioParameter.SqlDbType = SqlDbType.VarChar;
                    nombreParameter.Value = usuario.NombreUsuario;

                    SqlParameter contraseñaParameter = new SqlParameter("Contraseña", SqlDbType.VarChar) { Value = usuario.Contraseña };
                    contraseñaParameter.ParameterName = "Contraseña";
                    contraseñaParameter.SqlDbType = SqlDbType.VarChar;
                    contraseñaParameter.Value = usuario.Contraseña;

                    SqlParameter mailParameter = new SqlParameter();
                    mailParameter.ParameterName = "Mail";
                    mailParameter.SqlDbType = SqlDbType.VarChar;
                    mailParameter.Value = usuario.Mail;

                    sqlConnection.Open();

                    using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection))
                    {
                        sqlCommand.Parameters.Add(nombreParameter);
                        sqlCommand.Parameters.Add(apellidoParameter);
                        sqlCommand.Parameters.Add(nombreUsuarioParameter);
                        sqlCommand.Parameters.Add(contraseñaParameter);
                        sqlCommand.Parameters.Add(mailParameter);

                        sqlCommand.ExecuteScalar(); // Se ejecuta la sentencia sql
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}