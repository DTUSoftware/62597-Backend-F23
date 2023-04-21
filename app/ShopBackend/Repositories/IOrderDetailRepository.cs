using ShopBackend.Models;

namespace ShopBackend.Repositories
{
    public interface IOrderDetailRepository
    {
        Task<IEnumerable<OrderDetail>>  GetAll();
        Task<OrderDetail?> Get(Guid orderDetailId);
        Task<int> Insert(OrderDetail orderDetail);
        Task<int> Update(OrderDetail orderDetail);
        Task<int> Delete(Guid orderDetailId);

    }
}
