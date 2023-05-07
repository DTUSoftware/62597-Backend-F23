using ShopBackend.Models;

namespace ShopBackend.Repositories {

    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<User?> Get(string email);
        Task<int> Insert(User user);
        Task<int> Update(User user);
        Task<int> Delete(string email);
    }
}
