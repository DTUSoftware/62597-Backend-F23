using ShopBackend.Models;

namespace ShopBackend.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAll(int position);
        Task<Product?> Get(string productId);        
        Task<int> Insert(Product product);
        Task<int> Update(Product product);
        Task<int> Delete(string productId);

    }
}
