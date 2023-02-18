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

    }
}
