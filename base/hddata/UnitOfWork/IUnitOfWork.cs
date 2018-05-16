using hdcontext;

namespace hddata.UnitOfWork
{
    public interface IUnitOfWork
    {
        ContextConnection DbContext { get; }

        void CommitChange();

        void BeginTran();

        void CommitTran();

        void RollbackTran();

        void ClearProxy();
    }
}