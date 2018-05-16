using hdcontext.IdentityDomain;
using hddata.RepositoryPattern;
using System.Collections.Generic;

namespace hdidentity.Interface
{
    public interface IUserService : IBaseService<ApplicationUser, string>
    {
        IList<ApplicationUser> GetbyFilter(string currentUserId, int comId, int? teamId, string userName, string email, string phoneNumber, int currentPage, int pageSize, out int total);

        bool Create(ApplicationUser entity, string password, IList<int> groupIds, out string message);

        bool Update(ApplicationUser entity, IList<int> groupIds, out string message);

        bool DeleteUser(ApplicationUser entity, out string message);

        bool LockUser(string userId, out string message);

        bool UnLockUser(string userId, out string message);

        bool ChangePassword(string userId, string currentPass, string newPass, out string message);

        bool ResetPassword(string userId, string newPass, out string message);
    }
}