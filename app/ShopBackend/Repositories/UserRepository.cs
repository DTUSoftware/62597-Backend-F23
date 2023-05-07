using Microsoft.EntityFrameworkCore;
using ShopBackend.Contexts;
using ShopBackend.Models;

namespace ShopBackend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DBContext _dbContext;

        public UserRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User?> Get(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }


        public async Task<int> Insert(User user)
        {
            _dbContext.Add(user);
            return await _dbContext.SaveChangesAsync();

        }

        public async Task<int> Update(User user)
        {
            _dbContext.Update(user);
            return await _dbContext.SaveChangesAsync();

        }

        public async Task<int> Delete(string email)
        {
            _dbContext.Users.Remove(new User { Email = email });
            return await _dbContext.SaveChangesAsync();
        }
    }
}
