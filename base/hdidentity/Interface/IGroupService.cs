using hdcontext.IdentityDomain;
using hddata.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hdidentity.Interface
{
    public interface IGroupService: IBaseService<Group, int>
    {
        bool CheckContainName(int comId, int id, string name);

        IList<Group> GetbyFilter(int comId, string keyWord, int currentPage, int pageSize, out int total);

        bool Create(Group entity, IList<int> roleIds, out string message);

        bool Update(Group entity, IList<int> roleIds, out string message);

        bool Delete(int groupId, out string message);

        IList<Role> GetRolesOfGroup(int groupId);
    }
}
