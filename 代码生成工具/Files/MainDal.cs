using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Utility;
using Model;

namespace DAL
{
    public static class MainDal
    {
        private const string MConnectionString = "$ConnStr$";

        public static object DoDal(object obj, string spName, List<object> spActions, out string errMsg)
        {
            errMsg = "";
            var ds = new DataSet();
            var dt = new DataTable();
            ds.Tables.Add(dt);
            var o = new object();
            var conn = new SqlConnection(MConnectionString);
            conn.Open();
            SqlCommand cmd;
            SetSqlParameters.GetCmd(conn, out cmd, obj, spName);
            
            SqlTransaction myTran = conn.BeginTransaction();
            cmd.Transaction = myTran;
            try
            {
                foreach (var act in spActions)
                {
                    if (!cmd.Parameters.Contains("@SPAction"))
                    {
                        cmd.Parameters.Add(new SqlParameter("@SPAction", DbType.String));
                    }
                    cmd.Parameters["@SPAction"].Value = act.ToString();
                    switch (
                        (AppEnum.SPReturnTypes)(Enum.Parse(typeof(AppEnum.SPReturnTypes), cmd.Parameters["@SPReturnType"].Value.ToString())))
                    {
                        case AppEnum.SPReturnTypes.Object:
                            o = cmd.ExecuteScalar();
                            break;
                        case AppEnum.SPReturnTypes.Null:
                            cmd.ExecuteNonQuery();
                            break;
                        case AppEnum.SPReturnTypes.DataSet:
                            var select = new SqlDataAdapter(cmd);
                            if (((int)cmd.Parameters["@StartIndex"].Value).Equals(0) && ((int)cmd.Parameters["@PageSize"].Value).Equals(0))
                            {
                                ds.Tables.RemoveAt(0);
                                select.Fill(ds, "dt");
                            }
                            else
                            {
                                select.Fill((int)cmd.Parameters["@StartIndex"].Value, (int)cmd.Parameters["@PageSize"].Value, ds.Tables[0]);
                            }
                            break;
                    }
                }
                myTran.Commit();
                long newID;
                if (o != null && !Int64.TryParse(o.ToString(), out newID))
                {
                    return ds.Tables[0].Rows.Count > 0 ? ds : null;
                }
                return o;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return null;
        }
    }
}