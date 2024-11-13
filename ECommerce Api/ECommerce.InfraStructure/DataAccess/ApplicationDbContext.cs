using ECommerce.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.InfraStructure.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        //public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Shipping> Shippings { get; set; }
        public DbSet<CartItems> CartItems { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Cart> Carts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(entity => entity.ToTable("Users"));
            builder.Entity<IdentityRole<int>>(entity => entity.ToTable("Roles"));
            builder.Entity<IdentityUserRole<int>>(entity => entity.ToTable("UserRoles"));
            builder.Entity<IdentityUserClaim<int>>(entity => entity.ToTable("UserClaims"));
            builder.Entity<IdentityUserLogin<int>>(entity => entity.ToTable("UserLogins"));
            builder.Entity<IdentityRoleClaim<int>>(entity => entity.ToTable("RoleClaims"));
            builder.Entity<IdentityUserToken<int>>(entity => entity.ToTable("UserTokens"));

            builder.Entity<Order>()
                .HasOne(o => o.Payment)
                .WithOne(p => p.Order)
                .HasForeignKey<Payment>(p => p.OrderId);

            builder.Entity<Order>()
                .HasOne(o => o.Shipping)
                .WithOne(p => p.Order)
                .HasForeignKey<Shipping>(p => p.OrderId);

            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Cart)
                .WithOne(c => c.ApplicationUser)
                .HasForeignKey<Cart>(c => c.UserId);

        }
    }
}
