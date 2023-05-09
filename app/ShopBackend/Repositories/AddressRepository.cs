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

        public async Task<Address?> Get(Guid addressId)
        {
            return await _dbContext.Addresses.FirstOrDefaultAsync(a => a.Id == addressId);
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

        public async Task<int> Delete(Guid addressId)
        {
            var addresses = _dbContext.Addresses.Find(addressId);
            if (addresses != null)
            {
                _dbContext.Addresses.Remove(addresses);
                return await _dbContext.SaveChangesAsync();
            }
            else
            {
                return 1;
            }
        }
    }
}
