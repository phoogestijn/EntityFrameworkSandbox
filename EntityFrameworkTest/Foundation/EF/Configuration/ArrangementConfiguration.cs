using System.Data.Entity.ModelConfiguration;
using EntityFrameworkTest.Model;

namespace EntityFrameworkTest.Foundation.EF.Configuration
{
    public class ArrangementConfiguration : EntityTypeConfiguration<Arrangement>
    {
        public ArrangementConfiguration()
        {
            // Configure primairy key for the arrangement
            HasKey(e => e.Id);

            // Configure relation to ArrangementVersion
            HasMany(e => e.Versions).WithRequired(e => e.Arrangement);

            // Configure relation to Users            
            HasMany(e => e.Users).WithMany(u => u.Arrangements).Map(m => m.MapLeftKey("Arrangement").MapRightKey("User").ToTable("ArrangementUsers"));
        }
    }
}
