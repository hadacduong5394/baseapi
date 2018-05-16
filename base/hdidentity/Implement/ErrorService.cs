using hdcontext.IdentityDomain;
using hddata.DBFactory;
using hddata.RepositoryPattern;
using hdidentity.Interface;
using System;

namespace hdidentity.Implement
{
    public class ErrorService : BaseService<Error, int>, IErrorService
    {
        public ErrorService(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public void Log(Exception ex)
        {
            try
            {
                CreateNew(new Error
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    CreateDate = DateTime.Now,
                    Status = false
                });
                CommitChange();
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
        }
    }
}