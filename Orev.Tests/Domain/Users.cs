using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orev.Domain;
using Orev.Helpers;
using NHibernate;

using UsersRepository = Orev.Repositories.NHibernateRepository<Orev.Domain.User>;

namespace Orev.Tests.Domain
{
    /// <summary>
    /// Summary description for Users
    /// </summary>
    [TestClass]
    [DeploymentItem(@"Orev.Tests\Orev.sdf")]
    [DeploymentItem(@"Orev.Tests\hibernate.cfg.xml")]
    public class Users
    {
        public Users()
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
        public void CanCreateUser()
        {
            var usr = new User { Email = "test@test.com", Registered = DateTime.Now, LastSeen = DateTime.Now };
            usr.SetPassword("testPassword");
            
            using (var rep = new UsersRepository())
                rep.Add(usr);

            Assert.IsTrue(usr.Id > 0);
        }

        [TestMethod]
        public void VerifyDistinctEmails()
        {
            try
            {
                // Depending on the execution order, the first call could raise the expected exception
                CanCreateUser();
                CanCreateUser();
            }
            catch
            {
                return;
            }

            Assert.IsTrue(false, "More than one user with the same e-mail exist");
        }

        [TestMethod]
        public void CanAuthenticate()
        {
            const String email = "auth_test@test.com";
            const String pwd = "testPasswordAuth";

            var usr = new User { Email = email, Registered = DateTime.Now, LastSeen = DateTime.Now };
            usr.SetPassword(pwd);

            using (var rep = new UsersRepository())
            {
                rep.Add(usr);
                Assert.IsTrue(usr.Id > 0);
            }

            using (var rep = new UsersRepository())
            {
                User u = rep.SingleOrDefault(U => U.Email == email);

                Assert.IsTrue(u.Authenticate(pwd));
                Assert.IsTrue(usr.Id == u.Id);
            }
        }

        [TestMethod]
        public void CanChangePassword()
        {
            var usr = new User { Email = "pwd@test.com", Registered = DateTime.Now, LastSeen = DateTime.Now };
            usr.SetPassword("1234");

            Assert.IsTrue(usr.Authenticate("1234"));
            
            usr.SetPassword("4321");
            Assert.IsTrue(usr.Authenticate("4321"));
        }
    }
}
