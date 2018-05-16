using hdcontext.IdentityDomain;
using hddata.RepositoryPattern;
using System.Collections.Generic;

namespace hdidentity.Interface
{
    public interface IRoleService : IBaseService<Role, int>
    {
        IList<Role> GetbyFilter(string keyWord, int currentPage, int pageSize, out int total);

        bool Create(Role entity, out string message);

        bool Update(Role entity, out string message);

        bool Delete(int id, out string message);

        bool CheckContainName(int id, string name);

        IList<Role> GetAll();

        IList<Role> GetbyUserRole(int comId);
    }
}