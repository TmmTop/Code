using System;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using WebAPI.Common;
using WebAPI.Model;

namespace WebAPI.Dao
{
    public class LoginDao
    {
        public bool login(string account, string passwd)//登录
        {
            IDbConnection conn = new SqlConnection(SqlCommon.ConnectionString);
            SqlCommon.OpenConnection();
            var isAdd = conn.Query(string.Format("Select account,passwd from CoustomInfo where account='"+account+"' and passwd='"+passwd+"'")).AsList();//检查账号是否存在
            if (isAdd.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
