using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orev.Domain;
using Orev.Helpers;
using NHibernate;

using CorpusRepository = Orev.Repositories.NHibernateRepository<Orev.Domain.Corpus>;

namespace Orev.Tests.Domain
{
    /// <summary>
    /// Summary description for Corpora
    /// </summary>
    [TestClass]
    [DeploymentItem(@"Orev.Tests\Orev.sdf")]
    [DeploymentItem(@"Orev.Tests\hibernate.cfg.xml")]
    public class Corpora
    {
        public Corpora()
        {
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void InitTests(TestContext testContext)
        {
            Schema.GenerateSchema(); // Reset DB
        }

        [TestMethod]
        public void CanAddCorpus()
        {
            var crp = new Corpus { Name = "Test corpus", Description = "test", Language = "en-US", Path = "/test/" };
            using (var rep = new CorpusRepository())
                rep.Add(crp);

            Assert.IsTrue(crp.Id > 0);
        }
    }
}
