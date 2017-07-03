using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Model
{
    public partial class CoustomInfo
    {
        public int id { get; set; }
        public string cid { get; set; }
        public string account { get; set; }
        public string passwd { get; set; }
        public string name { get; set; }
        public string qq { get; set; }
        public string email { get; set; }
        public string mobilephone { get; set; }
        public string vip { get; set; }
        public string remark { get; set; }
        public string creattime { get; set; }
    }
}
