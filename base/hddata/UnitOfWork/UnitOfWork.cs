using hdcontext;
using hddata.DBFactory;
using System.Data.Entity;

namespace hddata.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbFactory _dbFactory;
        private ContextConnection _dbContext;
        private DbContextTransaction _tranContext;

        public UnitOfWork(IDbFactory dbFactory)
        {
            this._dbFactory = dbFactory;
        }

        public ContextConnection DbContext
        {
            get { return _dbContext ?? (_dbContext = _dbFactory.Init()); }
        }

        public virtual void CommitChange()
        {
            DbContext.SaveChanges();
        }

        public virtual void BeginTran()
        {
            _tranContext = DbContext.Database.BeginTransaction();
        }

        public virtual void CommitTran()
        {
            if (_tranContext != null)
            {
                _tranContext.Commit();
                _tranContext.Dispose();
            }
        }

        public virtual void RollbackTran()
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
    }
}