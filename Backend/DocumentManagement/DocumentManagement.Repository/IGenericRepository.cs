using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DocumentManagement.Repository
{
    public interface IGenericRepository<TEntity> where TEntity: class
    {
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");
        TEntity GetById(object id);
        void Insert(TEntity entity);
        void InsertNoCommit(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entityToDelete);
        void Update(TEntity entityToUpdate);
        void Update(List<TEntity> entitiesToUpdate);
        void UpdateNoCommit(TEntity entityToUpdate);
        void Save();
    }
}
