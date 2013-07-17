using System.Data.Entity;
using System.Linq;
using EntityFrameworkTest.Model;

namespace EntityFrameworkTest.Foundation.EF.Repository
{
    public class ArrangementVersionRepository : Repository<ArrangementVersion>
    {
        private readonly DbSet<ArrangementVersion> Versions;

        public ArrangementVersionRepository(DbContext context)
            : base(context)
        {
            this.Versions = context.Set<ArrangementVersion>();
        }

        public void Remove(ArrangementVersion version)
        {
            this.Versions.Remove(version);
        }
   }
}
