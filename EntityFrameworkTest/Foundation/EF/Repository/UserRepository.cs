using System.Data.Entity;
using EntityFrameworkTest.Model;

namespace EntityFrameworkTest.Foundation.EF.Repository
{
    public class UserRepository : Repository<User>
    {
        public UserRepository(DbContext context)
            : base(context)
        {}
    }
}
