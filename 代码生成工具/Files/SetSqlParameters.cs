using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Web;
using System.Linq;

namespace Utility
{
    public class SetSqlParameters
    {
        public static DataTable MapDs;

        public static SqlConnection GetCmd(SqlConnection conn, out SqlCommand cmd, object obj, string spName)
        {
            cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = spName;
            SetParameters(cmd, obj);
            return conn;
        }

        private static void SetParameters(SqlCommand cmd, object obj)
        {
            string spReturnType = null;
            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach (var propertyInfo in properties)
            {
                object value = propertyInfo.GetValue(obj, null);
                if (value != null)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory;
                    if (MapDs == null)
                    {
                        var ds = new DataSet();
                        ds.ReadXml(path + "SqlParasMap.xml");
                        MapDs = ds.Tables[0];
                    }
                    DataRow[] dr =
                        (MapDs.Select("System='" +
                                      propertyInfo.PropertyType.Name +
                                      "'"));
                    if (dr.Any() && !value.ToString().Equals(dr[0]["Filter"]))
                    {
                        cmd.Parameters.Add(new SqlParameter("@" + propertyInfo.Name, dr[0]["SqlDbType"]));
                        cmd.Parameters["@" + propertyInfo.Name].Value = value;
                    }
                    if (propertyInfo.PropertyType.Name.Equals("SPReturnTypes"))
                    {
                        spReturnType = value.ToString();
                    }
                }
            }
            cmd.Parameters.Add(new SqlParameter("@SPReturnType", DbType.String));
            cmd.Parameters["@SPReturnType"].Value = spReturnType;
        }

        public static object SetModelByDataSet(DataSet ds, object obj)
        {
            foreach (DataRow dr1 in ds.Tables[0].Rows)
            {
                foreach (DataColumn dc in dr1.Table.Columns)
                {
                    PropertyInfo pi = obj.GetType().GetProperty(dc.ColumnName);
                    if (pi != null)
                    {
                        pi.SetValue(obj, dc.Table.Rows[0][0], null);
                    }
                }
            }
            return obj;
        }
    }
}
