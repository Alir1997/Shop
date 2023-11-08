using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Shop.Domain.Models.Account;
using Shop.Domain.Models.Orders;
using Shop.Domain.Models.ProductEntity;
using Shop.Domain.Models.Site;
using Shop.Domain.Models.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infra.Data.Context
{
    public class ShopDbContext:DbContext
    {
        public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ShopDbContext>
        {
            public ShopDbContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<ShopDbContext>();
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog =ShopDb;Integrated security=True");

                return new ShopDbContext(optionsBuilder.Options);
            }
        }
        #region Constractor
        public ShopDbContext(DbContextOptions<ShopDbContext> options):base(options)
        {
        }


        #endregion
        #region user
        public DbSet<User> users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserFavorite> UserFavorites { get; set; }
        public DbSet<UserCompare> UserCompares { get; set; }
        #endregion

        #region Wallet
        public DbSet<UserWallet> UserWallets { get; set; }
        #endregion

        #region products
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> productCategories { get; set; }
        public DbSet<ProductFeature> productFeatures { get; set; }
        public DbSet<ProductSelectedCategories> productSelectedCategories { get; set; }
        public DbSet<ProductGalleries> productGalleries { get; set; }
        public DbSet<ProductComment> ProductComments { get; set; }
        #endregion

        #region site
        public DbSet<Slider> Sliders { get; set; }
        #endregion

        #region Order
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        #endregion


 
    }
}
