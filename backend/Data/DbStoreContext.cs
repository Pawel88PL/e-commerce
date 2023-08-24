using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiodOdStaniula.Models;
using System.Reflection.Emit;

namespace MiodOdStaniula
{
    public class DbStoreContext : IdentityDbContext<UserModel>
    {
        public DbStoreContext(DbContextOptions<DbStoreContext> options) : base(options) { }

        
        public DbSet<Product>? Products { get; set; }
        public DbSet<ProductImage>? ProductImages { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<ShopingCart>? ShopingCarts { get; set; }
        public DbSet<Customer>? Customers { get; set; }
        public DbSet<CartItem>? CartItem { get; set; }
        public DbSet<Order>? Orders { get; set; }
        public DbSet<OrderDetail>? OrderDetails{ get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Relacja między Order a Customer
            builder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId);

            // Relacja między OrderDetail a Order
            builder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId);

            // Relacja między OrderDetail a Product
            builder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductId);
        }
    }
}
