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
    public class RoleService : BaseService<Role, int>, IRoleService
    {
        private static readonly List<string> lstRoleIgnore = new List<string>()
        {
            "ROLE_VIEW", "ROLE_CREATE", "ROLE_DELETE", "EDIT_ROLE"
        };
        private static readonly ILog log = LogManager.GetLogger(typeof(RoleService));

        public RoleService(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public bool CheckContainName(int id, string name)
        {
            return CheckContains(n => n.Id != id && n.Name.Equals(name));
        }

        public bool Create(Role entity, out string message)
        {
            try
            {
                CreateNew(entity);
                CommitChange();
                message = TextHelper.CREAT_SUCCESSFULL;
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                message = TextHelper.ERROR_SYSTEM;
                return false;
            }
        }

        public bool Delete(int id, out string message)
        {
            try
            {
                Delete(id);
                CommitChange();
                message = TextHelper.DELETE_SUCCESSFULL;
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                message = TextHelper.ERROR_SYSTEM;
                return false;
            }
        }

        public IList<Role> GetAll()
        {
            return Query.OrderBy(n => n.Name).ToList();
        }

        public IList<Role> GetbyFilter(string keyWord, int currentPage, int pageSize, out int total)
        {
            var query = Query;
            if (!string.IsNullOrEmpty(keyWord))
            {
                query = query.Where(o => o.Name.Contains(keyWord) || o.Descreption.Contains(keyWord));
            }
            query = query.OrderByDescending(o => o.Id);
            total = query.Count();
            return query.Skip(currentPage * pageSize).Take(pageSize).ToList();
        }

        public IList<Role> GetbyUserRole(int comId)
        {
            var roles = Query.Where(n => n.Status == true);
            if (comId != -1)
            {
                roles = Query.Where(n => !lstRoleIgnore.Contains(n.Name));
            }
            return roles.ToList();
        }

        public bool Update(Role entity, out string message)
        {
            try
            {
                Update(entity);
                CommitChange();
                message = TextHelper.EDIT_SUCCESSFULL;
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                message = TextHelper.ERROR_SYSTEM;
                return false;
            }
        }
    }
}