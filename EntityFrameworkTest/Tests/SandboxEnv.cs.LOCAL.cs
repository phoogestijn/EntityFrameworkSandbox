using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Linq;
using System.Transactions;
using EntityFrameworkTest.Foundation.EF;
using EntityFrameworkTest.Foundation.EF.Repository;
using EntityFrameworkTest.Model;
using NUnit.Framework;

namespace EntityFrameworkTest.Tests.Create
{
    /// <summary>    
    /// <see cref="http://blog.oneunicorn.com/"/>
    /// <see cref="http://blog.oneunicorn.com/2011/12/05/should-you-use-entity-framework-change-tracking-proxies/"/>
    /// </summary>
    [Ignore]
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

                var arrangement2 = repository.Add();
                arrangement2.Name = "H4";
                arrangement2.BpNumber = "987654321";


                var arrangementVersion1 = new ArrangementVersion();
                arrangementVersion1.Version = 1;
                arrangementVersion1.Status = VersionStatus.Editing;


                var arrangementVersion2 = new ArrangementVersion();
                arrangementVersion2.Version = 2;
                arrangementVersion2.Status = VersionStatus.Archived;

                arrangement.Versions.Add(arrangementVersion1);
                arrangement.Versions.Add(arrangementVersion2);

                ctx.SaveChanges();
            }


            using (DatabaseContext ctx = new DatabaseContext())
            {
                var arrangement = ctx.Set<Arrangement>().First();
                Assert.AreEqual(2, arrangement.Versions.Count());
                var av = ctx.ObjectContext.CreateObject<ArrangementVersion>();
                av.Version = 33;
                av.Status = VersionStatus.Published;
                
                arrangement.Versions.Add(av);

                ctx.SaveChanges();
                av.Version = 333;
                ctx.SaveChanges();
            }

            using (DatabaseContext ctx = new DatabaseContext())
            {
                int aantalversies = ctx.Set<Arrangement>().First().Versions.Count();
                Assert.AreEqual(3, aantalversies);
            }

            using (DatabaseContext ctx = new DatabaseContext())
            {
                var arrangement = ctx.Set<Arrangement>().First(a => a.Id == 1);
                var arrangement2 = ctx.Set<Arrangement>().First(a => a.Id == 2);

                var versie = arrangement.Versions.First(av => av.Version == 1);

                Assert.AreEqual(3, arrangement.Versions.Count);

                versie.Arrangement = arrangement2;

                Assert.IsFalse(arrangement.Versions.Any(v => v.Id == versie.Id));
                Assert.IsTrue(arrangement2.Versions.Any(v => v.Id == versie.Id));

                //arrangement.Versions.Remove(versie);
                //arrangement2.Versions.Add(versie);
                
                ctx.SaveChanges();
            }

            using (DatabaseContext ctx = new DatabaseContext())
            {
                

                Arrangement arrangement = ctx.Set<Arrangement>().First(a => a.Id == 1);

                var entry = ctx.Entry<Arrangement>(arrangement);
                Assert.AreEqual(EntityState.Unchanged, entry.State);
                var temp1 = ctx.ChangeTracker.Entries().Single();

                arrangement.Name = "Test 1234";

                Assert.AreEqual(EntityState.Modified, entry.State);
                var temp2 = ctx.ChangeTracker.Entries();

                ctx.SaveChanges();
                Assert.AreEqual(EntityState.Unchanged, entry.State);
            }

            //using (DatabaseContext ctx = new DatabaseContext())
            //{
            //    var arrangement = ctx.Set<Arrangement>().First(a => a.Id == 2);
            //    ctx.Arrangements.Remove(arrangement);

            //    ctx.SaveChanges();
            //}
        }
    }
}
