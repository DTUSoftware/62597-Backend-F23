using Microsoft.EntityFrameworkCore;
using ShopBackend.Contexts;
using ShopBackend.Models;

namespace ShopBackend.Repositories
{
    public class OrderDetailsRepository : IOrderDetailRepository
    {
        private readonly DBContext _dbContext;
        public OrderDetailsRepository(DBContext dbContext)
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
            _dbContext.OrdersDetails.Remove(new OrderDetail{ Id= orderDetailId } );
            return await _dbContext.SaveChangesAsync();
        }
    }
}
