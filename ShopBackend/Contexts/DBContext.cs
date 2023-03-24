using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ShopBackend.Models;
using ShopBackend.Utils;
using System.Reflection.Metadata;

namespace ShopBackend.Contexts
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
    
        }

        public DbSet<Address> Address { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrdersDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasMany(x => x.Orders).WithOne(x => x.Customer)
            .HasForeignKey(x => x.CustomerEmail)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>().HasMany(x => x.Addresses).WithOne(x => x.Order)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>().Property(x => x.OrderStatus)
            .HasConversion<string>();

            modelBuilder.Entity<Order>().HasMany(x => x.OrderDetails).WithOne(x => x.Order)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderDetail>().HasOne(x => x.Product).WithMany(x => x.OrderDetails)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
