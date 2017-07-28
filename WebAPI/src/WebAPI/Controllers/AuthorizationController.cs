using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using static WebAPI.Tokens.TokenClass;
using System.Text;
using WebAPI.Dao;
using WebAPI.Model;
using Newtonsoft.Json;
using WebAPI.Common;
namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AuthorizationController : Controller
    {
        /// <summary>
        /// 登录action
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="role">角色</param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [AllowAnonymous]//仅允许匿名的post访问
        public async Task<JsonResult> Authorization([FromBody]CoustomInfo Coustom)
        {
            LoginDao dao = new LoginDao();
            var data = JsonConvert.SerializeObject(Coustom);//把客户端post来的匿名对象转json字符串
            Coustom = JsonConvert.DeserializeObject<CoustomInfo>(data);//把解析后的json字符串转为Coustom实体类，并赋值给Coustom实体类
            string username = Coustom.account;
            string password = Coustom.passwd;
            string power = "";//权限
            if (username.Trim() == null && password.Trim() == null)
            {
                return new JsonResult(new { msg = Commons.GetEnumDesc(Commons.AutocationLogin.nullAccount) });
            }
            if (username == "admin" && password == "admin")
            {
                if (password == "admin")//验证账号权限   power == "管理员"  
                {
                    var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("./?'.][#!@%$#*fgwq12F~"));
                    var options = new TokenProviderOptions
                    {
                        Audience = "audience",
                        Issuer = "issuer",
                        SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                    };
                    var tpm = new TokenProvider(options);
                    var token = await tpm.GenerateToken(HttpContext, username, password,"");
                    if (token != null)
                    {
                        return new JsonResult(new {type="OK",msg = Commons.GetEnumDesc(Commons.AutocationLogin.okAuto), token_ticket = token});//生成token,返回给客户端
                    }
                    else
                    {
                        return new JsonResult(new {type = "Wrong", msg = Commons.GetEnumDesc(Commons.AutocationLogin.failAuto)});
                    }
                }
                else {
                    return new JsonResult(new { type = "Wrong", msg = Commons.GetEnumDesc(Commons.AutocationLogin.noPower)});
                }
            }
            else
            {
                return new JsonResult(new { type = "Wrong", msg = Commons.GetEnumDesc(Commons.AutocationLogin.failAccount)});
            }
        }
    }
}
