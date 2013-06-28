using System.Data.Entity.ModelConfiguration;
using EntityFrameworkTest.Model;

namespace EntityFrameworkTest.Foundation.EF.Configuration
{
    public class ArrangementVersionConfiguration : EntityTypeConfiguration<ArrangementVersion>
    {
        public ArrangementVersionConfiguration()
        {
            HasKey(e => e.Id);
        }
    }
}
