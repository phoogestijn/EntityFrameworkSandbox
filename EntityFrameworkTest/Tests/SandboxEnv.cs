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
                arrangementVersion1.Version = 11;

                var arrangementVersion2 = new ArrangementVersion();
                arrangementVersion2.Version = 22;

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
                // opmerking: a.Id zal hier 0 zijn (default waarde)
                a.Name = "Oeps....";
                a.Versions.Add(new ArrangementVersion() { Version = 9999 });

                var a2 = ctx.Set<Arrangement>().Where(dbarr => dbarr.Id == 1).Single();

                // opmerking: 
                // als je niet de Id (PK) van het nieuwe Arrangement object gelijk maakt aan de bestaande PK,
                // dan zal SetValues crashen met de melding dat je 'Id' niet mag veranderen omdat deze deel uitmaakt
                // van de PK van (in dit geval) a2. (want 0 is niet gelijk aan de huidige PK, 1)
                a.Id = a2.Id;   // PK overnemen van bestaande record...
                ctx.Entry(a2).CurrentValues.SetValues(a);      // ...en dan kun je wel alle waardes overschrijven.

                ctx.SaveChanges();

                Arrangement foo = ctx.Set<Arrangement>().First();
                Assert.AreEqual(1, a.Versions.Count(), "aantal arrangementversies bij het arrangement in memory moet 1 zijn, want we hebben alles overschreven");

                var versies = from ver in ctx.Set<ArrangementVersion>()  where ver.Arrangement.Id == 1 orderby ver.Version select ver;
                Assert.AreEqual(3, versies.Count(), "aantal arrangementversies in de database moet nog steeds 3 zijn");
                var versieLijst = versies.ToArray();
                Assert.AreEqual(11, versieLijst[0].Version);
                Assert.AreEqual(22, versieLijst[1].Version);
                Assert.AreEqual(33, versieLijst[2].Version);
                // hmm, wat wel vreemd is, is dat de ArrangementVersion van hierboven (9999) niet in de database verschenen is....
            }

            using (DatabaseContext ctx = new DatabaseContext())
            {
                var arrangement = ctx.Set<Arrangement>().Include(a => a.Versions).Single();
                {
                    Assert.AreEqual(arrangement.Versions.Count(), ctx.Set<ArrangementVersion>().Count(), "gekoppelde aantal versies moet nu weer gelijk zijn aan het aantal versies in de database");

                    Assert.AreEqual(arrangement, arrangement.Versions.First().Arrangement, "versie moet aan het enige arrangement gekoppeld zijn");
                    Assert.AreEqual(arrangement, arrangement.Versions.Last().Arrangement, "versie moet aan het enige arrangement gekoppeld zijn");
                }
            }

        }
    }
}
