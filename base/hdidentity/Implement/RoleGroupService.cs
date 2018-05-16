using hdcontext.IdentityDomain;
using hddata.DBFactory;
using hddata.RepositoryPattern;
using hdidentity.Interface;

namespace hdidentity.Implement
{
    public class RoleGroupService : BaseService<RoleGroup, int>, IRoleGroupService
    {
        public RoleGroupService(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}