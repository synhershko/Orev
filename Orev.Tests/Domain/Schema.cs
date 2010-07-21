using System;
using Orev.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Orev.Tests.Domain
{
    [TestClass]
    [DeploymentItem(@"Orev.Tests\Orev.sdf")]
    [DeploymentItem(@"Orev.Tests\hibernate.cfg.xml")]
    public class Schema
    {
        public static Configuration GenerateSchema()
        {
            var cfg = new Configuration();
            cfg.Configure();
            new SchemaExport(cfg).Execute(true, true, false);

            return cfg;
        }

        [TestMethod]
        public void CanGenerateSchema()
        {
            GenerateSchema();
        }
    }
}
