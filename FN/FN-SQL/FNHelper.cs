using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using System.Configuration;
using System.IO;
using NHibernate.Tool.hbm2ddl;

namespace FN_SQL
{
    /// <summary>
    /// Fluent帮助类
    /// </summary>
    public class FNHelper
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string dbfile = ConfigurationManager.ConnectionStrings["dbfile"].ToString();
        /// <summary>
        /// 初始化工厂类
        /// </summary>
        private ISessionFactory sessionFactory = null;
        /// <summary>
        /// 初始化，在每次运行时删除现有的dbfile
        /// </summary>
        /// <param name="obj"></param>
        private void BuildSchema(NHibernate.Cfg.Configuration obj)
        {
            if (File.Exists(dbfile))
                File.Delete(dbfile);
            new SchemaExport(obj).Create(false, true);
        }
        /// <summary>
        /// 根据模型映射至数据库
        /// </summary>
        private void InitSessionFactory()
        {
            //.ConnectionString(db => db.Server(".").Database("Game").Username("sa").Password("123456")
            //.ConnectionString(db => db.FromConnectionStringWithKey(dbfile)
            sessionFactory = Fluently.Configure()
                 .Database(MsSqlConfiguration.MsSql2008
                 .ConnectionString(db => db.Server(".").Database("Game").Username("sa").Password("123456")
                 )).Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserName>())
                    .ExposeConfiguration(BuildSchema)
                    .BuildSessionFactory();
        }
        /// <summary>
        /// 工厂类
        /// </summary>
        private ISessionFactory SessionFactory
        {
            get
            {
                if (sessionFactory == null)
                {
                    InitSessionFactory();
                }
                return sessionFactory;
            }
        }
        /// <summary>
        /// 工厂类操作
        /// </summary>
        public ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}
