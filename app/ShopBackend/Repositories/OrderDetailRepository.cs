using Microsoft.EntityFrameworkCore;
using ShopBackend.Contexts;
using ShopBackend.Models;

namespace ShopBackend.Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly DBContext _dbContext;
        public OrderDetailRepository(DBContext dbContext)
        {  
            _dbContext = dbContext; 
        }

        public async Task<IEnumerable<OrderDetail>> GetAll()
        {
            return await _dbContext.OrdersDetails.ToListAsync();
        }

        public async Task<OrderDetail?> Get(Guid orderDetailId)
        {
            return await _dbContext.OrdersDetails.FirstOrDefaultAsync(od => od.Id == orderDetailId);
        }

        public async Task<int> Insert(OrderDetail orderDetail)
        {
            _dbContext.OrdersDetails.Add(orderDetail);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> Update(OrderDetail orderDetail)
        {
            _dbContext.OrdersDetails.Update(orderDetail);
            return await _dbContext.SaveChangesAsync();

        }

        public async Task<int> Delete(Guid orderDetailId)
        {
            var ordersDetails = _dbContext.OrdersDetails.Find(orderDetailId);
            if (ordersDetails != null)
            {
                _dbContext.OrdersDetails.Remove(ordersDetails);
                return await _dbContext.SaveChangesAsync();
            }
            else
            {
                return 1;
            }
        }
    }
}
