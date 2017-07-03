using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebAPI.Dao;
using WebAPI.Model;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class SatinController : Controller
    {
        //[Authorize]
        [AllowAnonymous]
        [HttpGet]//仅允许授权后的get访问
        public JsonResult getSatin(int pageindex, int pagesize)
        {
            ResBrowseDao dao = new ResBrowseDao();
            List<SatinInfo> list = dao.SelectResSatin(pageindex,pagesize);
            if (list != null)
            {
                //string content = "";
                //for (int i = 0; i <list.Count; i++)
                //{
                //    content += "{ 'ID':'"+list[i].Rid + "','article':'" + list[i].Content+"'},";
                //}
                return new JsonResult(new { msg = "请求成功！", content =list});//生成token,返回给客户端
            }
            else
            {
                return new JsonResult(new { msg = "请求成功！", content = "获取数据失败！" });//生成token,返回给客户端
            }
        }

    }
}
