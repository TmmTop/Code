using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FN_SQL;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FN
{
    class Program
    {
        static void Main(string[] args)
        {
            var session = new FNHelper().OpenSession();
            using (var tran = session.BeginTransaction())
            {
                for (int i = 0; i <= 100; i++)
                {
                    var name = new UserInfo
                    {
                        account = "666" + i,
                        age = i,
                        name = "李四" + i,
                        passwd = "123456" + i,
                        sex = "男" + i,
                        uid = "u0000" + i
                    };
                  session.Save(name);
                }
                tran.Commit();
                if (tran.WasCommitted)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("数据表创建成功！");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("数据表创建失败！");
                }
            }
        }
    }
}
