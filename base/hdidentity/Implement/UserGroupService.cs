using hdcontext.IdentityDomain;
using hddata.DBFactory;
using hddata.RepositoryPattern;
using hdidentity.Interface;

namespace hdidentity.Implement
{
    public class UserGroupService : BaseService<UserGroup, string>, IUserGroupService
    {
        public UserGroupService(IDbFactory dbFactory): base(dbFactory)
        {

        }
    }
}