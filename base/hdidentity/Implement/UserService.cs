using hdcontext.IdentityDomain;
using hdcore;
using hdcore.Utils;
using hddata.DBFactory;
using hddata.RepositoryPattern;
using hddata.UnitOfWork;
using hdidentity.Interface;
using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace hdidentity.Implement
{
    public class UserService : BaseService<ApplicationUser, string>, IUserService
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UserService));
        public UserService(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public bool ChangePassword(string userId, string currentPass, string newPass, out string message)
        {
            try
            {
                var unitOW = IoC.Resolve<IUnitOfWork>();
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(unitOW.DbContext));
                var success = manager.ChangePassword(userId, currentPass, newPass);
                if (success.Succeeded)
                {
                    message = "Đổi mật khẩu thành công.";
                    return true;
                }

                message = "Đổi mật khẩu thất bại.";
                return false;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                message = ex.Message;
                return false;
            }
        }

        public bool Create(ApplicationUser entity, string password, IList<int> groupIds, out string message)
        {
            var userSrv = IoC.Resolve<IUserService>();
            var unitOW = IoC.Resolve<IUnitOfWork>();
            try
            {

                if (userSrv.Query.Any(n => n.UserName.Equals(entity.UserName)))
                {
                    message = "Tên đăng nhập này đã tồn tại trước đó rồi.";
                    return false;
                }

                if (userSrv.Query.Any(n => n.Email.Equals(entity.Email)))
                {
                    message = "Email này đã tồn tại trước đó rồi.";
                    return false;
                }

                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(unitOW.DbContext));
                unitOW.BeginTran();
                var success = manager.Create(entity, password);
                if (success.Succeeded)
                {
                    var ugSrv = IoC.Resolve<IUserGroupService>();
                    foreach (var gId in groupIds)
                    {
                        ugSrv.CreateNew(new UserGroup { GroupId = gId, UserId = entity.Id });
                        ugSrv.CommitChange();
                    }
                    unitOW.CommitTran();
                    message = TextHelper.CREAT_SUCCESSFULL;
                    return true;
                }
                else
                {
                    unitOW.RollbackTran();
                    message = TextHelper.ERROR_SYSTEM;
                    return false;
                }
            }
            catch (Exception ex)
            {
                unitOW.RollbackTran();
                log.Error(ex);
                message = ex.Message;
                return false;
            }
        }

        public bool DeleteUser(ApplicationUser entity, out string message)
        {
            var unitOW = IoC.Resolve<IUnitOfWork>();
            try
            {
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(unitOW.DbContext));
                unitOW.BeginTran();
                var success = manager.Delete(entity);
                if (success.Succeeded)
                {
                    var ugSrv = IoC.Resolve<IUserGroupService>();
                    ugSrv.DeleteMulti(n => n.UserId == entity.Id);
                    ugSrv.CommitChange();

                    unitOW.CommitTran();
                    message = TextHelper.DELETE_SUCCESSFULL;
                    return true;
                }
                else
                {
                    unitOW.RollbackTran();
                    message = TextHelper.ERROR_SYSTEM;
                    return false;
                }
            }
            catch (Exception ex)
            {
                unitOW.RollbackTran();
                log.Error(ex);
                message = ex.Message;
                return false;
            }
        }

        public IList<ApplicationUser> GetbyFilter(string currentUserId, int comId, int? teamId, string userName, string email, string phoneNumber, int currentPage, int pageSize, out int total)
        {
            var query = Query;
            if (comId != -1)
            {
                query = Query.Where(n => n.ComId == comId || n.Id == currentUserId);
            }
            else
            {
                query = query.Where(n => n.TeamId != 2);
            }
            if (teamId.HasValue)
            {
                query = query.Where(n => n.TeamId == teamId);
            }
            if (!string.IsNullOrEmpty(userName))
            {
                query = query.Where(n => n.UserName.Contains(userName));
            }
            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(n => n.Email.Contains(email));
            }
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                query = query.Where(n => n.PhoneNumber.Contains(phoneNumber));
            }
            query = query.OrderByDescending(n => n.CreateDate);
            total = query.Count();
            return query.Skip(currentPage * pageSize).Take(pageSize).ToList();
        }

        public bool LockUser(string userId, out string message)
        {
            try
            {
                var entity = GetbyKey(userId);
                entity.Status = false;
                Update(entity);
                CommitChange();
                message = "Khóa người dùng thành công.";
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                message = ex.Message;
                return false;
            }
        }

        public bool ResetPassword(string userId, string newPass, out string message)
        {
            var unitOW = IoC.Resolve<IUnitOfWork>();
            try
            {
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(unitOW.DbContext));
                unitOW.BeginTran();
                var success = manager.RemovePassword(userId);
                if (success.Succeeded)
                {
                    var s = manager.AddPassword(userId, newPass);
                    if (s.Succeeded)
                    {
                        unitOW.CommitTran();
                        message = "Đổi mật khẩu thành công.";
                        return true;
                    }
                }
                unitOW.RollbackTran();
                message = "Đổi mật khẩu thất bại.";
                return false;
            }
            catch (Exception ex)
            {
                unitOW.RollbackTran();
                log.Error(ex);
                message = ex.Message;
                return false;
            }
        }

        public bool UnLockUser(string userId, out string message)
        {
            try
            {
                var entity = GetbyKey(userId);
                entity.Status = true;
                Update(entity);
                CommitChange();
                message = "mở khóa người dùng thành công.";
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                message = ex.Message;
                return false;
            }
        }

        public bool Update(ApplicationUser entity, IList<int> groupIds, out string message)
        {
            var unitOW = IoC.Resolve<IUnitOfWork>();
            try
            {
                var userSrv = IoC.Resolve<IUserService>();
                if (userSrv.Query.Any(n => n.Id != entity.Id && n.UserName.Equals(entity.UserName)))
                {
                    message = "Tên đăng nhập này đã tồn tại trước đó rồi.";
                    return false;
                }

                if (userSrv.Query.Any(n => n.Id != entity.Id && n.Email.Equals(entity.Email)))
                {
                    message = "Email này đã tồn tại trước đó rồi.";
                    return false;
                }


                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(unitOW.DbContext));
                unitOW.BeginTran();
                var success = manager.Update(entity);
                if (success.Succeeded)
                {
                    var ugSrv = IoC.Resolve<IUserGroupService>();
                    ugSrv.DeleteMulti(n => n.UserId == entity.Id);
                    ugSrv.CommitChange();

                    foreach (var gId in groupIds)
                    {
                        ugSrv.CreateNew(new UserGroup { GroupId = gId, UserId = entity.Id });
                    }
                    ugSrv.CommitChange();

                    unitOW.CommitTran();
                    message = TextHelper.EDIT_SUCCESSFULL;
                    return true;
                }
                else
                {
                    unitOW.RollbackTran();
                    message = TextHelper.ERROR_SYSTEM;
                    return false;
                }
            }
            catch (Exception ex)
            {
                unitOW.RollbackTran();
                log.Error(ex);
                message = ex.Message;
                return false;
            }
        }
    }
}