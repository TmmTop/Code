using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Common
{
    public class Commons
    {
        ///<summary>
        ///获取时间戳
        ///</summary>
        ///<returns></returns>
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
        ///<summary>
        ///获取枚举项描述信息例如GetEnumDesc(Days.Sunday)
        ///</summary>
        ///<paramname="en">枚举项如Days.Sunday</param>
        ///<returns></returns>
        public static string GetEnumDesc(Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false).ToArray();
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }
            return en.ToString();
        }

        public enum AutocationLogin//验证用户
        {
            [Description("授权失败，账号密码为空！")]
            nullAccount = 0,
            [Description("授权失败，账号密码有误！")]
            failAccount = 1,
            [Description("授权成功！")]
            okAuto = 2,
            [Description("授权失败，票据生成错误！")]
            failAuto = 3,
            [Description("授权失败，没有访问权限！")]
            noPower = 4,
        }
    }
}
