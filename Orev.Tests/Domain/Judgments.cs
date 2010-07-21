using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orev.Domain;
using Orev.Helpers;
using NHibernate;

using UsersRepository = Orev.Repositories.NHibernateRepository<Orev.Domain.User>;
using TopicsRepository = Orev.Repositories.NHibernateRepository<Orev.Domain.Topic>;
using CorpusRepository = Orev.Repositories.NHibernateRepository<Orev.Domain.Corpus>;
using JudgmentsRepository = Orev.Repositories.NHibernateRepository<Orev.Domain.Judgment>;

namespace Orev.Tests.Domain
{
    /// <summary>
    /// Summary description for Judgements
    /// </summary>
    [TestClass]
    [DeploymentItem(@"Orev.Tests\Orev.sdf")]
    [DeploymentItem(@"Orev.Tests\hibernate.cfg.xml")]
    public class Judgments
    {
        public Judgments()
        {
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        public static Corpus testCorpus;
        public static Topic testTopic;
        public static User testUser;

        [ClassInitialize]
        public static void InitTests(TestContext testContext)
        {
            Schema.GenerateSchema(); // Reset DB

            // Add a corpus
            testCorpus = new Corpus { Name = "Test corpus", Description = "test", Language = "en-US", Path = "/test/" };
            using (var rep = new CorpusRepository())
                rep.Add(testCorpus);

            // Add a topic
            testTopic = new Topic { Title = "test topic", Description = "desc", Narrator = "narr", Language = "en-US" };
            using (var rep = new TopicsRepository())
                rep.Add(testTopic);

            // Add a test user
            testUser = new User { Email = "test@test.com", Registered = DateTime.Now, LastSeen = DateTime.Now };
            testUser.SetPassword("testPassword");

            using (var rep = new UsersRepository())
                rep.Add(testUser);
        }

        [TestMethod]
        public void CanAddJudgment()
        {
            var j = new Judgment
            {
                CorpusId = testCorpus.Id,
                DocumentId = "A",
                TopicId = testTopic.Id,
                UserId = testUser.Id,
                UserJudgement = false
            };

            using (var rep = new JudgmentsRepository())
                rep.Add(j);

            Assert.IsTrue(j.Id > 0);
        }

        [TestMethod]
        public void CanLoadByCorpusId()
        {
            // Add a judgement
            var j = new Judgment
            {
                CorpusId = testCorpus.Id,
                DocumentId = "B",
                TopicId = testTopic.Id,
                UserId = testUser.Id,
                UserJudgement = false
            };

            using (var rep = new JudgmentsRepository()){
                rep.Add(j);

                var judgments = rep.All(J => J.CorpusId == testCorpus.Id);

                Assert.IsTrue(judgments.Count() > 0);
            }
        }
    }
}
