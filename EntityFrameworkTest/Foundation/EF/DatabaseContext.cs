using System;
using System.Configuration;
using System.Data.Entity;
using EntityFrameworkTest.Foundation.EF.Configuration;
using EntityFrameworkTest.Foundation.EF.Repository;

namespace EntityFrameworkTest.Foundation.EF
{
    public class DatabaseContext : DbContext
    {       
        public DatabaseContext() 
            : this("EntityFramework") {            
        }

        public DatabaseContext(string connectionStringConfigName) 
            : base(GetConnectionString(connectionStringConfigName))
        {            
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
            
            base.OnModelCreating(modelBuilder);
        }


        private static string GetConnectionString(string connectionStringName)
        {
            var connectionStringElement = ConfigurationManager.ConnectionStrings[connectionStringName];
        
            if (connectionStringElement == null)
                throw new ArgumentException(string.Format("ConnectionString '{0}' could not be found", connectionStringName));
                
            return connectionStringElement.ConnectionString;                     
        }
    }
}
