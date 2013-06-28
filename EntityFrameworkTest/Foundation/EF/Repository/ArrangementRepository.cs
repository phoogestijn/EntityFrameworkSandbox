using System.Data.Entity;
using System.Linq;
using EntityFrameworkTest.Model;

namespace EntityFrameworkTest.Foundation.EF.Repository
{
    public class ArrangementRepository : Repository<Arrangement>
    {
        public ArrangementRepository(DbContext context)
            : base(context) {}
    }
}
