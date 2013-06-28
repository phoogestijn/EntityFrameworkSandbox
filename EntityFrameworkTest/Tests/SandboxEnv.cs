using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Transactions;
using EntityFrameworkTest.Foundation.EF;
using EntityFrameworkTest.Foundation.EF.Repository;
using EntityFrameworkTest.Model;
using NUnit.Framework;

namespace EntityFrameworkTest.Tests.Create
{
    public class SandboxEnv : EFTestBase
    {
        [Test]
        public void Sandbox()
        {

            using (DatabaseContext ctx = new DatabaseContext())
            {
                ArrangementRepository repository = new ArrangementRepository(ctx);
                var arrangement = repository.Add();
                arrangement.Name = "Koops Furness";
                arrangement.BpNumber = "123456789";

                var arrangementVersion1 = new ArrangementVersion();
                arrangementVersion1.Version = 1;

                var arrangementVersion2 = new ArrangementVersion();
                arrangementVersion2.Version = 2;

                arrangement.Versions.Add(arrangementVersion1);
                arrangement.Versions.Add(arrangementVersion2);

                ctx.SaveChanges();
            }


            using (DatabaseContext ctx = new DatabaseContext())
            {
                var arrangement = ctx.Set<Arrangement>().Single();
                Assert.AreEqual(2, arrangement.Versions.Count());
                arrangement.Versions.Add(new ArrangementVersion() { Version = 33 });
                ctx.SaveChanges();
            }

            using (DatabaseContext ctx = new DatabaseContext())
            {
                int aantalversies = ctx.Set<Arrangement>().Single().Versions.Count();
                Assert.AreEqual(3, aantalversies);
            }

            using (DatabaseContext ctx = new DatabaseContext())
            {
                Arrangement a = new Arrangement();
                //a.Id = 1;
                a.Name = "Oeps....";
                a.Versions.Add(new ArrangementVersion() { Version = 10000 });

                var a2 = ctx.Set<Arrangement>().Where(dbarr => dbarr.Id == 1).Single();
                ctx.Entry(a2).CurrentValues.SetValues(a);

                ctx.SaveChanges();

                Arrangement foo = ctx.Set<Arrangement>().First();

                Assert.AreEqual(1, a.Versions.Count());
                var versies = from ver in ctx.Set<ArrangementVersion>() where ver.Arrangement.Id == 1 select ver;
                Assert.AreEqual(1, versies.Count());
            }

            using (DatabaseContext ctx = new DatabaseContext())
            {
                var versies = from ver in ctx.Set<ArrangementVersion>() where ver.Arrangement.Id == 1 select ver;
                var thing = versies.ToList();

                var arrangement = ctx.Set<Arrangement>().Include(a => a.Versions).Single();
                {
                    Assert.AreEqual(arrangement.Versions.Count(), ctx.Set<ArrangementVersion>().Count());

                    Assert.AreEqual(arrangement, arrangement.Versions.First().Arrangement);
                    Assert.AreEqual(arrangement, arrangement.Versions.Last().Arrangement);
                }
            }

        }
    }
}
