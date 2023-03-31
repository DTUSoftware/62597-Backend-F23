using ShopBackend.Models;

namespace ShopBackend.Repositories
{
    public interface IAddressRepository
    {
        Task<IEnumerable<Address>> GetAll();
        Task<Address?> Get(Guid AddressId);
        Task<int> Insert(Address address);
        Task<int> Update(Address address);
        Task<int> Delete(Guid AddressId);
        
    }
}
