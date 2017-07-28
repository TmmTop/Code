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
    public class TestController : Controller
    {
        //[Authorize]
        [AllowAnonymous]
        [HttpGet]//仅允许授权后的get访问
        public JsonResult getData()
        {
            DataDao dal = new DataDao();
            IList<Data> list = dal.Select();
            return new JsonResult(new {list });//生成token,返回给客户端
        }
    }
}
