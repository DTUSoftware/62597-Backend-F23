using Microsoft.EntityFrameworkCore;
using ShopBackend.Contexts;
using ShopBackend.Models;

namespace ShopBackend.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DBContext _dbContext;

        public CustomerRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Customer>> GetAll()
        {
            return await _dbContext.Customers.ToListAsync();
        }

        public async Task<Customer?> Get(string email)
        {
            return await _dbContext.Customers.FirstOrDefaultAsync(c => c.Email == email);
        }


        public async Task<int> Insert(Customer customer)
        {
            _dbContext.Add(customer);
            return await _dbContext.SaveChangesAsync();

        }

        public async Task<int> Update(Customer customer)
        {
            _dbContext.Update(customer);
            return await _dbContext.SaveChangesAsync();

        }

        public async Task<int> Delete(string email)
        {
            _dbContext.Customers.Remove(new Customer { Email = email });
            return await _dbContext.SaveChangesAsync();
        }
    }
}
