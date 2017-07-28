using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace WebAPI.Common
{
    public class SqlCommon
    {
        public static string ConnectionString
        {
            get
            {
                string _connectionString = "data source=.;initial catalog=brnshop;persist security info=True;user id=sa;password=123456;MultipleActiveResultSets=True;App=EntityFramework";
                return _connectionString;
            }
        }
        public static SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            return connection;
        }
        public static SqlConnection CloseConnection()
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Close();
            return connection;
        }
    }
}
