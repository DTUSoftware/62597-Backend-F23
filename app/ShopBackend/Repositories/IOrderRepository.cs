using ShopBackend.Models;
namespace ShopBackend.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAll();
        Task<Order?> Get(Guid OrderId);
        Task<int> Insert(Order order);
        Task<int> Update(Order order);
        Task<int> Delete(Guid OrderId);

    }
}
