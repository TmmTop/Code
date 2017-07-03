using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.OleDb;

    public static class DBHelper
    {

        private static SqlConnection connection;
        private static OleDbConnection oleConn;
        private static string connString;
        private static string oleconnString;
        static DBHelper()
        {
            connString = ConfigurationManager.ConnectionStrings["DBString"].ConnectionString;

            //oleconnString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "App_Data\\StuInfo.accdb;Persist Security Info=True";
             //oleconnString=@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\juntang.liu\Desktop\StuInfo\App_Data\StuInfo.accdb;Persist Security Info=True"; 
        }

        /// <summary>
        /// 数据库连接对象
        /// </summary>
        public static SqlConnection Connection
        {
            get
            {
                if (connection == null)
                {
                    connection = new SqlConnection(connString);
                    connection.Open();
                }
                else if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                else if (connection.State == System.Data.ConnectionState.Broken)
                {
                    connection.Close();
                    connection.Open();
                }
                return connection;
            }
        }

        public static OleDbConnection OleConn
        {
            get
            {
                if (oleConn == null)
                {
                    oleConn = new OleDbConnection(oleconnString);
                    oleConn.Open();
                }
                else if (oleConn.State == System.Data.ConnectionState.Closed)
                {
                    oleConn.Open();
                }
                else if (oleConn.State == System.Data.ConnectionState.Broken)
                {
                    oleConn.Close();
                    oleConn.Open();
                }
                return oleConn;
            }
        }

        /// <summary>
        /// 获取command对象
        /// </summary>
        /// <param name="safeSql">sql</param>
        /// <returns></returns>
        public static SqlCommand GetCommand(string safeSql)
        {
            SqlCommand cmd = new SqlCommand(safeSql, Connection);
            return cmd;
        }

        public static OleDbCommand GetOleCommand(string safeSql)
        {
            OleDbCommand cmd = new OleDbCommand(safeSql, OleConn);
            return cmd;
        }

        /// <summary>
        /// 获取command对象
        /// </summary>
        /// <param name="safeSql">sql</param>
        /// <param name="values">参数|参数数组</param>
        /// <returns></returns>
        public static SqlCommand GetCommand(string safeSql, params SqlParameter[] values)
        {
            SqlCommand cmd = new SqlCommand(safeSql, Connection);
            foreach (SqlParameter p in values)
            {
                if (p.Value == null)
                {
                    p.Value = DBNull.Value;
                }
            }

            cmd.Parameters.AddRange(values);

            return cmd;
        }

        public static OleDbCommand GetOleCommand(string safeSql, params OleDbParameter[] values)
        {
            OleDbCommand cmd = new OleDbCommand(safeSql, OleConn);

            cmd.Parameters.AddRange(values);

            return cmd;
        }

        /// <summary>
        /// 对数据库增删改操作
        /// </summary>
        /// <param name="safeSql">sql</param>
        /// <returns></returns>
        public static int ExecuteCommand(string safeSql)
        {
            int result = GetCommand(safeSql).ExecuteNonQuery();
            return result;
        }

        public static int ExecuteOleCommand(string safeSql)
        {
            int result = GetOleCommand(safeSql).ExecuteNonQuery();
            return result;
        }

        /// <summary>
        /// 对数据库增删改操作
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="values">参数</param>
        /// <returns></returns>
        public static int ExecuteCommand(string sql, params SqlParameter[] values)
        {
            int result = GetCommand(sql, values).ExecuteNonQuery();
            return result;
        }

        public static int ExecuteOleCommand(string sql, params OleDbParameter[] values)
        {
            int result = GetOleCommand(sql, values).ExecuteNonQuery();
            return result;
        }

        /// <summary>
        /// 返回第一行第一列数据
        /// </summary>
        /// <param name="safeSql">sql</param>
        /// <returns></returns>
        public static int GetScalar(string safeSql)
        {
            object result = GetCommand(safeSql).ExecuteScalar();
            int number = 0;
            if (result.ToString().Length>0)
            {
                number = (int)result;
            }
            return number;
        }

        public static int GetOleScalar(string safeSql)
        {
            int result = (int)GetOleCommand(safeSql).ExecuteScalar();
            return result;
        }

        /// <summary>
        /// 返回第一行第一列数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="values">参数</param>
        /// <returns></returns>
        public static object GetScalar(string safeSql, params SqlParameter[] values)
        {

            object result = GetCommand(safeSql, values).ExecuteScalar();
            return result;
        }

        public static object GetOleScalar(string safeSql, params OleDbParameter[] values)
        {

            int result = GetOleCommand(safeSql, values).ExecuteNonQuery();
            return result;
        }


        /// <summary>
        /// 获取DataReader对象
        /// </summary>
        /// <param name="safeSql"></param>
        /// <returns></returns>
        public static SqlDataReader GetReader(string safeSql)
        {
            SqlDataReader reader = GetCommand(safeSql).ExecuteReader();
            return reader;
        }

        public static OleDbDataReader GetOleReader(string safeSql)
        {
            OleDbDataReader reader = GetOleCommand(safeSql).ExecuteReader();
            return reader;
        }

        public static SqlDataReader GetReader(string safeSql, params SqlParameter[] values)
        {
            SqlDataReader reader = GetCommand(safeSql, values).ExecuteReader();
            return reader;
        }

        public static OleDbDataReader GetOleReader(string safeSql, params OleDbParameter[] values)
        {
            OleDbDataReader reader = GetOleCommand(safeSql, values).ExecuteReader();
            return reader;
        }
        
        /// <summary>
        /// 返回数据集
        /// </summary>
        /// <param name="safeSql"></param>
        /// <returns></returns>
        public static DataSet GetDataSet(string safeSql)
        {
            DataSet ds = new DataSet();

            SqlDataAdapter da = new SqlDataAdapter(GetCommand(safeSql));
            da.Fill(ds);
            return ds;
        }

        public static DataSet GetOleDataSet(string safeSql)
        {
            DataSet ds = new DataSet();

            OleDbDataAdapter da = new OleDbDataAdapter(GetOleCommand(safeSql));
            da.Fill(ds);
            return ds;
        }


        public static DataSet GetDataSet(string sql, params SqlParameter[] values)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(GetCommand(sql, values));
            da.Fill(ds);
            return ds;
        }

        public static DataSet GetOleDataSet(string sql, params OleDbParameter[] values)
        {
            DataSet ds = new DataSet();
            OleDbDataAdapter da = new OleDbDataAdapter(GetOleCommand(sql, values));
            da.Fill(ds);
            return ds;
        }


        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="safeSql">sql</param>
        /// <returns></returns>
        public static SqlDataReader ExecuteProcedure(string safeSql)
        {

            SqlCommand cmd = GetCommand(safeSql);
            cmd.CommandType = CommandType.StoredProcedure;

            return cmd.ExecuteReader();
        }


}

