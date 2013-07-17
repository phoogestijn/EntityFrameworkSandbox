using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Runtime.Serialization;
using EntityFrameworkTest.Foundation.EF.Configuration;
using EntityFrameworkTest.Foundation.EF.Repository;
using EntityFrameworkTest.Model;

namespace EntityFrameworkTest.Foundation.EF
{
    public class DatabaseContext : DbContext
    {
        public ObjectContext ObjectContext { get; private set; }

        public DbSet<Arrangement> Arrangements { get; set; }
        public DbSet<ArrangementVersion> ArrangementVersions { get; set; }

        public DatabaseContext() 
            : this("EntityFramework") {            
        }

        public DatabaseContext(string connectionStringConfigName) 
            : base(GetConnectionString(connectionStringConfigName))
        {
            this.Configuration.AutoDetectChangesEnabled = false;            

            this.ObjectContext = (this as IObjectContextAdapter).ObjectContext;
        }

        public TRepository GetRepository<TRepository>()
            where TRepository : IRepositoryBase
        {
            var repository = Activator.CreateInstance(typeof (TRepository), this);
            return (TRepository) repository;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ArrangementConfiguration());
            modelBuilder.Configurations.Add(new ArrangementVersionConfiguration());
            
            
            base.OnModelCreating(modelBuilder);
        }


        private static string GetConnectionString(string connectionStringName)
        {
            var connectionStringElement = ConfigurationManager.ConnectionStrings[connectionStringName];
        
            if (connectionStringElement == null)
                throw new ArgumentException(string.Format("ConnectionString '{0}' could not be found", connectionStringName));
                
            return connectionStringElement.ConnectionString;                     
        }

        public static explicit operator ObjectContext(DatabaseContext context)
        {
            return context.ObjectContext;
        }
    }
}
