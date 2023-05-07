using Microsoft.EntityFrameworkCore;
using ShopBackend.Contexts;
using ShopBackend.Models;

namespace ShopBackend.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DBContext _dbContext;

        public ProductRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Product>> GetAll(int page)
        {
            var pageSize = 20;
            return await _dbContext.Products.Skip(page * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<Product?> Get(string productId)
        {
           return await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);

        }

        public async Task<int> Insert(Product product)
        {
            _dbContext.Products.Add(product);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> Update(Product product)
        {
            _dbContext.Update(product);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> Delete(string productId)
        {
            _dbContext.Products.Remove( new Product { Id = productId} );
            return await _dbContext.SaveChangesAsync();
        }
    }
}
