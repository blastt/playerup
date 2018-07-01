using Marketplace.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Market.Data.Infrastructure
{
    public abstract class RepositoryBase<T> where T : class
    {
        #region Properties
        private MarketEntities dataContext;
        private readonly IDbSet<T> dbSet;

        protected IDbFactory DbFactory
        {
            get;
            private set;
        }

        protected MarketEntities DbContext
        {
            get { return dataContext ?? (dataContext = DbFactory.Init()); }
        }
        #endregion

        protected RepositoryBase(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
            dbSet = DbContext.Set<T>();
        }

        #region Implementation
        public virtual void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            dbSet.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbSet.Where<T>(where).AsEnumerable();
            
            
            foreach (T obj in objects)
                dbSet.Remove(obj);
        }

        public virtual T GetById(int id)
        {
            return dbSet.Find(id);
            
        }

        public virtual T GetById(int id, params Expression<Func<T, object>>[] includes)
        {
            var set = dbSet;
            foreach (var include in includes)
            {
                set.Include(include);
            }
            return set.Find(id);

        }





        public virtual IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        public virtual IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {

            var set = dbSet.AsQueryable();
            foreach (var include in includes)
            {
                set = set.Include(include);
            }
            return set;
        }

        public virtual IQueryable<T> GetAllAsNoTracking(params Expression<Func<T, object>>[] includes)
        {

            var set = dbSet.AsNoTracking();
            foreach (var include in includes)
            {
                set = set.Include(include);
            }
            return set;
        }


        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).ToList();
        }

        public virtual IEnumerable<T> GetManyAsNoTracking(Expression<Func<T, bool>> where)
        {
            return dbSet.AsNoTracking().Where(where).ToList();
        }

        public virtual IQueryable<T> GetMany(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes)
        {
            var query = dbSet.Where(where);
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query;
        }

        public virtual IQueryable<T> GetManyAsNoTracking(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes)
        {
            var query = dbSet.AsNoTracking().Where(where);
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query;
        }

        public async virtual Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> where)
        {
            return await dbSet.Where(where).ToListAsync<T>();
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).FirstOrDefault<T>();
        }

        public T Get(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes)
        {
            var query = dbSet.Where(where);
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query.FirstOrDefault<T>();
        }
        public Task<T> GetAsync(Expression<Func<T, bool>> where)
        {
            return dbSet.FirstOrDefaultAsync(where);
        }

        #endregion

    }
}
