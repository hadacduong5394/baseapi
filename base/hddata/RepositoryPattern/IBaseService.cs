using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace hddata.RepositoryPattern
{
    public interface IBaseService<T, TId> where T : class
    {
        IQueryable<T> Query { get; }

        T CreateNew(T entity);

        void Update(T entity);

        void Delete(TId id);

        void Delete(T entity);

        void DeleteMulti(Expression<Func<T, bool>> where);

        T GetbyKey(TId id);

        IList<T> GetbyFilter(int currentPage, int pageSize, out int total);

        IList<T> GetbyFilterLazyLoad(int currentPage, int pageSize, out int total);

        T GetSingleByCondition(Expression<Func<T, bool>> predicate, string[] includes = null);

        IQueryable<T> GetAll(string[] includes = null);

        IQueryable<T> GetMulti(Expression<Func<T, bool>> predicate, string[] includes = null);

        int Count(Expression<Func<T, bool>> where);

        bool CheckContains(Expression<Func<T, bool>> predicate);

        void CommitChange();

        void BeginTran();

        void CommitTran();

        void RollbackTran();

        void ClearProxy();
    }
}