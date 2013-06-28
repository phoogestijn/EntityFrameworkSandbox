using System.Data.Entity.ModelConfiguration;
using EntityFrameworkTest.Model;

namespace EntityFrameworkTest.Foundation.EF.Configuration
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            HasKey(u => u.Id);

            HasMany(u => u.Privileges).WithMany(p => p.Users).Map(m => m.MapLeftKey("User").MapRightKey("Privilege").ToTable("UserPrivileges"));
        }
    }
}
