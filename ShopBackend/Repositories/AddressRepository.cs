using Microsoft.EntityFrameworkCore;
using ShopBackend.Contexts;
using ShopBackend.Models;


namespace ShopBackend.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly DBContext _dbContext;

        public AddressRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Address>> GetAll()
        {
            return await _dbContext.Addresses.ToListAsync();
        }

        public async Task<Address?> Get(string email, string type)
        {
            return await _dbContext.Addresses.FirstOrDefaultAsync(a => a.Email == email && a.Type == type);
        }

        public async Task<int> Insert(Address address)
        {
            _dbContext.Add(address);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> Update(Address address)
        {
            _dbContext.Update(address);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> Delete(string addressEmail, string addressType)
        {
            _dbContext.Addresses.Remove(new Address { Email = addressEmail, Type = addressType });
            return await _dbContext.SaveChangesAsync();
        }
    }
}
