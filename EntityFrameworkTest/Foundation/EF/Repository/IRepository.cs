using System;
using System.Linq;
using System.Linq.Expressions;
using EntityFrameworkTest.Model;

namespace EntityFrameworkTest.Foundation.EF.Repository
{
    public interface IRepositoryBase
    {}

    public interface IRepository<TEntity> : IRepositoryBase
        where TEntity : BusinessObject
    {
        TEntity GetSingle(Expression<Func<TEntity, bool>> filter);

        
        IQueryable<TEntity> GetAllWhere(Expression<Func<TEntity, bool>> filter);

        void Delete(TEntity entity);
        int DeleteWhen(Expression<Func<TEntity, bool>> filter);

        TEntity Add(TEntity entity);
        TEntity Add();
    }
}
