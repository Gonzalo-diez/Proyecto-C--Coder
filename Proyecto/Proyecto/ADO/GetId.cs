using System;
using System.Data.SqlClient;

namespace Proyecto.ADO.NET
{
    public static class GetId
    {
        public static int Get(SqlCommand sqlCommand)
        {
            sqlCommand.CommandText = "Select @@IDENTITY";
            sqlCommand.Parameters.Clear();

            object objID = sqlCommand.ExecuteScalar();

            int id = Convert.ToInt32(objID);

            return id;
        }
    }
}
