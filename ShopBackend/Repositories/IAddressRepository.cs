using ShopBackend.Models;

namespace ShopBackend.Repositories
{
    public interface IAddressRepository
    {
        Task<IEnumerable<Address>> GetAll();
        Task<Address?> Get(string email,string type);
        Task<int> Insert(Address address);
        Task<int> Update(Address address);
        Task<int> Delete(string email, string addressType);
        
    }
}
