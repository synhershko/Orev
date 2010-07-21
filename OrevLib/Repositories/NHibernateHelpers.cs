using System;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Orev.Domain
{
    public class NHibernateHelpers
    {
        public static Configuration GenerateSchema()
        {
            var cfg = new Configuration();
            cfg.Configure();
            new SchemaExport(cfg).Execute(true, true, false);

            return cfg;
        }

        /*private static ISessionFactory _sessionFactory;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    var configuration = new Configuration();
                    configuration.Configure();
                    _sessionFactory = configuration.BuildSessionFactory();
                }
                return _sessionFactory;
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }*/
    }
}
