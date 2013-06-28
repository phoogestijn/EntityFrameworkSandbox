using System.Data.Entity;
using EntityFrameworkTest.Model;

namespace EntityFrameworkTest.Foundation.EF.Repository
{
    public class ArrangementVersionRepository : Repository<ArrangementVersion>
    {
        public ArrangementVersionRepository(DbContext context)
            : base(context)
        {}
   }
}
