using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Market.Data.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        // Marks an entity as new
        void Add(T entity);
        // Marks an entity as modified
        void Update(T entity);
        // Marks an entity to be removed
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> where);
        // Get an entity by int id
        T GetById(int id);
        T GetById(int id, params Expression<Func<T, object>>[] includes);
        // Get an entity using delegate
        T Get(Expression<Func<T, bool>> where);
        T Get(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes);
        Task<T> GetAsync(Expression<Func<T, bool>> where);
        // Gets all entities of type T
        IEnumerable<T> GetAll();
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);
        IQueryable<T> GetAllAsNoTracking(params Expression<Func<T, object>>[] includes);
        // Gets entities using delegate
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
        IQueryable<T> GetMany(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes);
        IQueryable<T> GetManyAsNoTracking(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes);
        //Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> where);
    }
}
