using System.Data.Entity.Infrastructure;
using System.Linq;
using EntityFrameworkTest.Foundation.EF;
using EntityFrameworkTest.Foundation.EF.Repository;
using EntityFrameworkTest.Model;
using NUnit.Framework;

namespace EntityFrameworkTest.Tests.Create
{
    public class ArrangementVersionTests : EFTestBase
    {
        [Test, ExpectedException(typeof(DbUpdateException))]
        public void CreateVersionWithoutArrangement()
        {
            using (DatabaseContext ctx = new DatabaseContext())
            {
                var dao = ctx.GetRepository<ArrangementVersionRepository>();

                ArrangementVersion version = dao.Add();
                version.Version = 1;
                // Oeps, no arrangement version is set !!

                ctx.SaveChanges();
            }
        }

        [Test]
        public void CreateVersionWithArrangement()
        {
            int arrangementId;
            using (DatabaseContext ctx = new DatabaseContext())
            {
                var arrangementDao = ctx.GetRepository<ArrangementRepository>();
                
                Arrangement arrangement = arrangementDao.Add();
                arrangement.BpNumber = "123456789";
                arrangement.Name = "Koops";

                ctx.SaveChanges();
                arrangementId = arrangement.Id;
            }

            using (DatabaseContext ctx = new DatabaseContext())
            {
                var arrangementDao = ctx.GetRepository<ArrangementRepository>();
                var versionDao = ctx.GetRepository<ArrangementVersionRepository>();

                var arrangement = arrangementDao.GetSingle(a => a.Id == arrangementId);

                ArrangementVersion version = versionDao.Add();
                version.Version = 1;
                arrangement.Versions.Add(version);

                ctx.SaveChanges();
            }

            using (DatabaseContext ctx = new DatabaseContext())
            {
                var arrangementDao = ctx.GetRepository<ArrangementRepository>();
                var arrangement = arrangementDao.GetSingle(a => a.Id == arrangementId);

                Assert.IsNotEmpty(arrangement.Versions);
                Assert.AreEqual(1, arrangement.Versions.Single().Id);

                ctx.SaveChanges();
            }
        }
    }
}
