using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orev.Domain;
using Orev.Helpers;
using NHibernate;

using TopicsRepository = Orev.Repositories.NHibernateRepository<Orev.Domain.Topic>;

namespace Orev.Tests.Domain
{
    /// <summary>
    /// Summary description for Topics
    /// </summary>
    [TestClass]
    [DeploymentItem(@"Orev.Tests\Orev.sdf")]
    [DeploymentItem(@"Orev.Tests\hibernate.cfg.xml")]
    public class Topics
    {
        public Topics()
        {
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }
        
        public static ISession session;

        [ClassInitialize]
        public static void InitTests(TestContext testContext)
        {
            Schema.GenerateSchema(); // Reset DB
        }

        [TestMethod]
        public void CanAddTopic()
        {
            var topic = new Topic { Title = "test topic", Description = "desc", Narrator = "narr", Language = "en-US" };
            
            using (var rep = new TopicsRepository())
                rep.Add(topic);

            Assert.IsTrue(topic.Id > 0);
        }

        [TestMethod]
        public void CanLoadTopicById()
        {
            var topic = new Topic { Title = "test topic", Description = "desc", Narrator = "narr", Language = "en-US" };

            using (var rep = new TopicsRepository())
                rep.Add(topic);

            Assert.IsTrue(topic.Id > 0);
            int topicId = topic.Id;

            using (var rep = new TopicsRepository())
            {
                Topic t = rep.SingleOrDefault(T => T.Id == topicId);
                Assert.IsTrue(t != null);
                Assert.IsTrue(topicId == t.Id);
            }
        }
    }
}
