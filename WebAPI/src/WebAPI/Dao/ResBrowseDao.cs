using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Common;
using WebAPI.Model;

namespace WebAPI.Dao
{
    public class ResBrowseDao
    {
        public List<SatinInfo> SelectResSatin(int PageIndex, int PageSize) {//每页获取多少条数据
            int Sumpage = PageIndex * PageSize;
            PageIndex = (PageIndex - 1) * PageSize;
            IDbConnection conn = new SqlConnection(SqlCommon.ConnectionString);
            SqlCommon.OpenConnection();
            var isAdd = conn.Query<SatinInfo>(string.Format("select * from (select row_number() over(order by id) rownumber, * from Satin)a where rownumber >="+ PageIndex + " and rownumber<="+ Sumpage)).AsList();//检查账号是否存在
            if (isAdd.Count > 0)
            {
                return isAdd;
            }
            else
            {
                return null;
            }
        }
    }
}
