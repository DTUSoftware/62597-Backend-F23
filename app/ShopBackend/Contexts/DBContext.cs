using Microsoft.EntityFrameworkCore;
using ShopBackend.Models;

namespace ShopBackend.Contexts
{
    public class DBContext : DbContext
    {
        public DBContext() { }
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
    
        }

        public virtual DbSet<Address> Addresses { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<OrderDetail> OrdersDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().Property(x => x.OrderStatus)
            .HasConversion<string>();

            modelBuilder.Entity<Customer>().Property(x => x.Role)
            .HasConversion<string>();
        }
    }
}
