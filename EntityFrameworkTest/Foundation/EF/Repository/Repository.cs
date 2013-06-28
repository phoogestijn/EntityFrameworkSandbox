using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using EntityFrameworkTest.Model;

namespace EntityFrameworkTest.Foundation.EF.Repository
{
    /// <summary>
    /// Respository is the abstract repository class user to simpelify the implementations of 
    /// custom EntityFramework repositories. 
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public abstract class Repository<TEntity> : IRepository<TEntity>
        where TEntity : BusinessObject
    {
        private readonly DbContext ctx;

        // Within this abstract Repository class, only the Set property should be used. Direct
        // access to the context's Set() method will potentially return entity instances that
        // should not be accessed by the user.
        private DbSet<TEntity> Set { get { return Filter(this.ctx.Set<TEntity>()); } }

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="context">The current database context.</param>
        protected Repository(DbContext context)
        {
            this.ctx = context;
        }

        /// <summary>
        /// Gets one single element base on the specified filter
        /// </summary>
        /// <param name="filter">The filter to specify the needed entity.</param>
        /// <returns>If found, the entity instance is return; otherwise <c>null</c> is returned.</returns>
        public TEntity GetSingle(Expression<Func<TEntity, bool>> filter)
        {
            return this.Set.FirstOrDefault(filter);            
        }

        /// <summary>
        /// Gets all entities in the current context. (Set will could be filtered when the Filter() method
        /// is overridden.
        /// </summary>
        /// <returns>Collection of entities inside the current context.</returns>
        public IQueryable<TEntity> GetAll()
        {
            return this.Set;
        }

        /// <summary>
        /// Gets all entities in the current context that meet the specified condition.
        /// </summary>
        /// <param name="filter">Expression to specifies the filter condition.</param>
        /// <returns>Collection of entities inside the current context that meet the specified condition.</returns>
        public IQueryable<TEntity> GetAllWhere(Expression<Func<TEntity, bool>> filter)
        {
            return this.Set.Where(filter);
        }

        /// <summary>
        /// Deletes instance of the entity from the database context.
        /// </summary>
        /// <param name="entity">The entity instance that should be deleted.</param>
        public void Delete(TEntity entity)
        {
            this.Set.Remove(entity);
        }

        /// <summary>
        /// Delete all entity instances from the database context, which meet the specified condition.
        /// </summary>
        /// <param name="filter">The conditions that specifies which entity instances should be deleted.</param>
        /// <returns>The number of entity instances deleted from the context.</returns>
        public int DeleteWhen(Expression<Func<TEntity, bool>> filter)
        {
            var removals = GetAllWhere(filter).ToList();

            foreach (var removal in removals)
                Delete(removal);

            return removals.Count();
        }

        /// <summary>
        /// Adds the specified entity instance to the database context. (The instance will be added on SaveChanges())
        /// </summary>
        /// <param name="entity">The entity instance that should be added to the context.</param>
        /// <returns>The instance added to the context.</returns>
        public TEntity Add(TEntity entity)
        {
            return this.Set.Add(entity);
        }

        /// <summary>
        /// Add a new entity instance to the database context. (The instance will be added on SaveChanges())
        /// </summary>
        /// <returns>The instance added to the context.</returns>
        public TEntity Add()
        {
            var entity = this.Set.Create();
            return Add(entity);            
        }        

        /// <summary>
        /// Filters the repositories underlying set. Overriding this method allows one to narrow the complete set.
        /// used in the 
        /// </summary>
        /// <param name="set">The complete set of entity instances</param>
        /// <returns>When overridden the method could return a narrowed set, normally the complete set is returned.</returns>
        public virtual DbSet<TEntity> Filter(DbSet<TEntity> set)
        {
            return set;
        }
    }
}
