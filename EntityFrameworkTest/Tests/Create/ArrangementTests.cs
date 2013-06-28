using System.Linq;
using EntityFrameworkTest.Foundation.EF;
using EntityFrameworkTest.Foundation.EF.Repository;
using EntityFrameworkTest.Model;
using NUnit.Framework;

namespace EntityFrameworkTest.Tests.Create
{
    public class ArrangementTests : EFTestBase
    {
        [Test]
        public void CreateEmptyArrangement()
        {
            using (DatabaseContext ctx = new DatabaseContext())
            {
                ArrangementRepository repository = new ArrangementRepository(ctx);
                var arrangement = repository.Add();

                Assert.IsTrue(arrangement.Id == 0);
                arrangement.Name = "Koops Furness";
                arrangement.BpNumber = "123456789";

                ctx.SaveChanges();
                Assert.IsTrue(arrangement.Id > 0);
            }

            using (DatabaseContext ctx = new DatabaseContext())
            {
                var arrangement = ctx.Set<Model.Arrangement>().Single();
                Assert.AreEqual("Koops Furness", arrangement.Name);
                Assert.AreEqual("123456789", arrangement.BpNumber);

                Assert.IsEmpty(arrangement.Users);
                Assert.IsEmpty(arrangement.Versions);
            }
        }

        [Test]
        public void CreateArrangementWithVersions()
        {
            using (DatabaseContext ctx = new DatabaseContext())
            {
                ArrangementRepository repository = new ArrangementRepository(ctx);
                var arrangement = repository.Add();

                arrangement.Name = "Koops Furness";
                arrangement.BpNumber = "123456789";

                var version1 = new ArrangementVersion {Version = 1};
                var version2 = new ArrangementVersion {Version = 2};

                arrangement.Versions.Add(version1);
                arrangement.Versions.Add(version2);

                Assert.IsTrue(arrangement.Id == 0);
                Assert.IsTrue(version1.Id == 0);
                Assert.IsTrue(version2.Id == 0);

                ctx.SaveChanges();
                Assert.IsTrue(arrangement.Id > 0);
                Assert.IsTrue(version1.Id > 0);
                Assert.IsTrue(version2.Id > 0);
                Assert.AreEqual(arrangement, version1.Arrangement);
                Assert.AreEqual(arrangement, version2.Arrangement);
            }

            using (DatabaseContext ctx = new DatabaseContext())
            {
                var arrangement = ctx.Set<Arrangement>().Single();
                Assert.AreEqual("Koops Furness", arrangement.Name);
                Assert.AreEqual("123456789", arrangement.BpNumber);

                Assert.IsEmpty(arrangement.Users);
                Assert.IsNotEmpty(arrangement.Versions);
                Assert.AreEqual(2, arrangement.Versions.Count);
                Assert.AreEqual(arrangement, arrangement.Versions.First().Arrangement);
                Assert.AreEqual(arrangement, arrangement.Versions.Last().Arrangement);
            }
        }

        [Test]
        public void CreateArrangementWithUsers()
        {
            using (DatabaseContext ctx = new DatabaseContext())
            {
                ArrangementRepository repository = new ArrangementRepository(ctx);
                var arrangement = repository.Add();

                arrangement.Name = "Koops Furness";
                arrangement.BpNumber = "123456789";

                var user1 = new User() {UserName = "user1", Email = "user1@test.com", FullName = "User One"};
                var user2 = new User() {UserName = "user2", Email = "user2@test.com", FullName = "User Two"};

                arrangement.Users.Add(user1);
                arrangement.Users.Add(user2);

                Assert.IsTrue(arrangement.Id == 0);
                Assert.IsTrue(user1.Id == 0);
                Assert.IsTrue(user2.Id == 0);

                ctx.SaveChanges();

                Assert.IsTrue(arrangement.Id > 0);
                Assert.IsTrue(user1.Id > 0);
                Assert.IsTrue(user2.Id > 0);
                Assert.AreEqual(arrangement, user1.Arrangements.Single());
                Assert.AreEqual(arrangement, user2.Arrangements.Single());
            }
        }
    }
}
