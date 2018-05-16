using hdcontext.IdentityDomain;
using hdcore;
using hdcore.Utils;
using hddata.DBFactory;
using hddata.RepositoryPattern;
using hdidentity.Interface;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace hdidentity.Implement
{
    public class GroupService : BaseService<Group, int>, IGroupService
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GroupService));
        public GroupService(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public bool CheckContainName(int comId, int id, string name)
        {
            return Query.Any(n => n.ComId == comId && n.Id != id && n.Name.Equals(name) && n.Status == true);
        }

        public bool Create(Group entity, IList<int> roleIds, out string message)
        {
            try
            {
                var rgSrv = IoC.Resolve<IRoleGroupService>();
                if (CheckContainName(entity.ComId, entity.Id, entity.Name))
                {
                    message = "Tên này đã tồn tại trước đó, hãy thử lại.";
                    return false;
                }
                BeginTran();
                var g = CreateNew(entity);
                CommitChange();
                foreach (var roleId in roleIds)
                {
                    rgSrv.CreateNew(new RoleGroup
                    {
                        GroupId = g.Id,
                        RoleId = roleId
                    });
                    rgSrv.CommitChange();
                }
                CommitTran();
                message = TextHelper.CREAT_SUCCESSFULL;
                return true;
            }
            catch (Exception ex)
            {
                RollbackTran();
                log.Error(ex);
                message = ex.Message;
                return false;
            }
        }

        public bool Delete(int groupId, out string message)
        {
            var rgSrv = IoC.Resolve<IRoleGroupService>();
            BeginTran();
            try
            {
                rgSrv.DeleteMulti(n => n.GroupId == groupId);
                rgSrv.CommitChange();

                Delete(groupId);
                CommitChange();

                CommitTran();
                message = TextHelper.DELETE_SUCCESSFULL;
                return true;
            }
            catch (Exception ex)
            {
                RollbackTran();
                log.Error(ex);
                message = ex.Message;
                return false;
            }
        }

        public IList<Group> GetbyFilter(int comId, string keyWord, int currentPage, int pageSize, out int total)
        {
            var query = Query.Where(n => n.ComId == comId);
            if (!string.IsNullOrEmpty(keyWord))
            {
                query = query.Where(n => n.Name.Contains(keyWord) || n.Descreption.Contains(keyWord));
            }

            query = query.OrderByDescending(n => n.Id);

            total = query.Count();

            return query.Skip(currentPage * pageSize).ToList();
        }

        public IList<Role> GetRolesOfGroup(int groupId)
        {
            var groupRp = IoC.Resolve<IRoleGroupService>();
            var roleSrv = IoC.Resolve<IRoleService>();

            var lstRoleGroup = groupRp.Query.Where(n => n.GroupId == groupId).Select(n => n.RoleId);
            var result = roleSrv.GetMulti(n => lstRoleGroup.Contains(n.Id)).ToList();

            return result;
        }

        public bool Update(Group entity, IList<int> roleIds, out string message)
        {
            try
            {
                if (CheckContainName(entity.ComId, entity.Id, entity.Name))
                {
                    message = "Tên này đã tồn tại trước đó, hãy thử lại.";
                    return false;
                }
                var rgSrv = IoC.Resolve<IRoleGroupService>();
                BeginTran();
                Update(entity);
                CommitChange();

                rgSrv.DeleteMulti(n => n.GroupId == entity.Id);
                rgSrv.CommitChange();

                foreach (var roleId in roleIds)
                {
                    rgSrv.CreateNew(new RoleGroup
                    {
                        GroupId = entity.Id,
                        RoleId = roleId
                    });
                    rgSrv.CommitChange();
                }

                CommitTran();
                message = TextHelper.EDIT_SUCCESSFULL;
                return true;
            }
            catch (Exception ex)
            {
                RollbackTran();
                log.Error(ex);
                message = ex.Message;
                return false;
            }
        }
    }
}