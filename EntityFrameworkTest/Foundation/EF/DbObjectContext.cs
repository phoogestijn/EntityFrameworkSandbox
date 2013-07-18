using System.Data.EntityClient;
using System.Data.Objects;

namespace EntityFrameworkTest.Foundation.EF
{
    public class DbObjectContext : ObjectContext
    {
        public DbObjectContext(EntityConnection connection)
            : base(connection) {}

        public DbObjectContext(string connectionString)
            : base(connectionString) {}

        protected DbObjectContext(string connectionString, string defaultContainerName)
            : base(connectionString, defaultContainerName) {}

        protected DbObjectContext(EntityConnection connection, string defaultContainerName)
            : base(connection, defaultContainerName) {}
               
    }
}
