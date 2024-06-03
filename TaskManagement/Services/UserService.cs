using TaskManagement.Data;
using TaskManagement.Models;

namespace TaskManagement.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return _userRepository.GetAllAsync();
        }

        public Task<User?> GetUserByIdAsync(int id)
        {
            return _userRepository.GetByIdAsync(id);
        }

        public Task<int> AddUserAsync(User user)
        {
            return _userRepository.AddAsync(user);
        }

        public Task<int> UpdateUserAsync(User user)
        {
            return _userRepository.UpdateAsync(user);
        }

        public Task<int> DeleteUserAsync(int id)
        {
            return _userRepository.DeleteAsync(id);
        }

        public async Task<User?> AuthenticateUserAsync(string username, string password)
        {
            var users = await _userRepository.GetAllAsync();
            return users.SingleOrDefault(u => u.Username == username && u.Password == password);
        }
    }

}
