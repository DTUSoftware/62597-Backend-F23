using Microsoft.EntityFrameworkCore;
using ShopBackend.Models;

namespace ShopBackend.Contexts
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<OrderDetail> OrdersDetails { get; set; }


    }
}
