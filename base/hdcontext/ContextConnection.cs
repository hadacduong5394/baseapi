using hd.context;
using hdcontext.AdminDomain.Domain;
using hdcontext.AdminDomain.IdentityCode;
using hdcontext.IdentityDomain;
using System.Data.Entity;

namespace hdcontext
{
    public abstract class ContextConnection : DbConnection
    {
        public ContextConnection() : base()
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Group> Groups { get; set; }
        public DbSet<Role> ApplicationRoles { get; set; }
        public DbSet<RoleGroup> RoleGroups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Error> Errors { get; set; }

        public DbSet<LeftMenu> LeftMenus { get; set; }
        public DbSet<TopMenu> TopMenus { get; set; }
        public DbSet<CompanyInfo> CompanyInfo { get; set; }

        public DbSet<ProductCode> ProductCode { get; set; }
        public DbSet<MaterialCode> MaterialCode { get; set; }
        public DbSet<OrderCode> OrderCode { get; set; }
        public DbSet<ImportProductCode> ImportProductCode { get; set; }
        public DbSet<CustomerCode> CustomerCode { get; set; }
        public DbSet<SupplierCode> SupplierCode { get; set; }
    }
}