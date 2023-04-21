using ShopBackend.Models;

namespace ShopBackend.Repositories {

    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAll();
        Task<Customer?> Get(string email);
        Task<int> Insert(Customer customer);
        Task<int> Update(Customer customer);
        Task<int> Delete(string email);
    }
}
