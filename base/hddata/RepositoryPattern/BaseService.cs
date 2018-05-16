using hdcontext;
using hddata.DBFactory;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;

namespace hddata.RepositoryPattern
{
    public class BaseService<T, TId> : IBaseService<T, TId> where T : class
    {
        public BaseService()
        {
        }

        private ContextConnection dataContext;
        private DbContextTransaction _tranContext;
        private readonly IDbSet<T> dbSet;

        private IDbFactory DbFactory;

        private ContextConnection DbContext
        {
            get { return dataContext ?? (dataContext = DbFactory.Init()); }
        }

        protected BaseService(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
            dbSet = DbContext.Set<T>();
        }

        public virtual IQueryable<T> Query
        {
            get
            {
                return from a in dbSet select a;
            }
        }

        public virtual T CreateNew(T entity)
        {
            return dbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            dbSet.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(TId id)
        {
            var entity = dbSet.Find(id);
            dbSet.Remove(entity);
        }

        public virtual void DeleteMulti(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbSet.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                dbSet.Remove(obj);
        }

        public virtual T GetbyKey(TId id)
        {
            return dbSet.Find(id);
        }

        public virtual int Count(Expression<Func<T, bool>> where)
        {
            return dbSet.Count(where);
        }

        public virtual IQueryable<T> GetAll(string[] includes = null)
        {
            //HANDLE INCLUDES FOR ASSOCIATED OBJECTS IF APPLICABLE
            if (includes != null && includes.Count() > 0)
            {
                var query = dataContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                return query.AsQueryable();
            }

            return dataContext.Set<T>().AsQueryable();
        }

        public virtual T GetSingleByCondition(Expression<Func<T, bool>> predicate, string[] includes = null)
        {
            return GetAll(includes).FirstOrDefault(predicate);
        }

        public virtual IQueryable<T> GetMulti(Expression<Func<T, bool>> predicate, string[] includes = null)
        {
            //HANDLE INCLUDES FOR ASSOCIATED OBJECTS IF APPLICABLE
            if (includes != null && includes.Count() > 0)
            {
                var query = dataContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                return query.Where<T>(predicate).AsQueryable<T>();
            }

            return dataContext.Set<T>().Where<T>(predicate).AsQueryable<T>();
        }

        public virtual bool CheckContains(Expression<Func<T, bool>> predicate)
        {
            return dataContext.Set<T>().Count<T>(predicate) > 0;
        }

        public void CommitChange()
        {
            DbContext.SaveChanges();
        }

        public void BeginTran()
        {
            _tranContext = DbContext.Database.BeginTransaction();
        }

        public void CommitTran()
        {
            if (_tranContext != null)
            {
                _tranContext.Commit();
                _tranContext.Dispose();
            }
        }

        public void RollbackTran()
        {
            if (_tranContext != null)
            {
                _tranContext.Rollback();
                _tranContext.Dispose();
            }
        }

        public virtual void ClearProxy()
        {
            DbContext.Configuration.ProxyCreationEnabled = false;
        }

        public virtual void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public virtual IList<T> GetbyFilter(int currentPage, int pageSize, out int total)
        {
            var query = Query.OrderBy(n => true);
            total = query.Count();
            return query.Skip(currentPage * pageSize).Take(pageSize).ToList();
        }

        public IList<T> GetbyFilterLazyLoad(int currentPage, int pageSize, out int total)
        {
            var query = Query.OrderBy(n => true);

            var max = currentPage <= 1 ? 4 - currentPage : 2;
            var rows = query.Skip(currentPage * pageSize).Take(pageSize * max + 1).ToList();
            total = currentPage * pageSize + rows.Count;

            var result = rows.Take(pageSize);

            return result.ToList();
        }
    }
}