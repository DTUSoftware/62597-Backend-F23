using Microsoft.EntityFrameworkCore;
using ShopBackend.Models;
using System.Security.Cryptography.X509Certificates;

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
