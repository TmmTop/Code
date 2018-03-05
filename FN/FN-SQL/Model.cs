using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FN_SQL
{
    //用户表
    public class UserInfo
    {
        public virtual int id { get; protected set; }
        public virtual string uid { get; set; }
        public virtual string account { get; set; }
        public virtual string passwd { get; set; }
        public virtual string name { get; set; }
        public virtual int age { get; set; }
        public virtual string sex { get; set; }
    }
    //用户表映射
    public class UserName : ClassMap<UserInfo>
    {
        public UserName()
        {
            Id(x => x.id).Not.Nullable().GeneratedBy.Identity();//设置id为主键,自动创建
            Map(x => x.uid).Not.Nullable().Length(50);
            Map(x => x.account).Not.Nullable().Length(80);
            Map(x => x.passwd).Not.Nullable().Length(100);
            Map(x => x.name).Not.Nullable().Length(50);
            Map(x => x.age).Not.Nullable();
            Map(x => x.sex).Not.Nullable().Length(10);
            Table("UserInfo");
        }
    }
}
