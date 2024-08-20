using BetsApi.Models;
using BetsApi.Repositories;
using System.Linq.Expressions;
namespace BetsApi.Services
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(RegisterRequest request);
        Task UpdateUserAsync(long id, User user);
        Task DeleteUserAsync(long id);
        Task<User> GetUserByIdAsync(long id);
        Task<User> GetUserByUserNameAsync(string name);
        Task<User> LoginUser(LoginRequest loginRequest);
        Task<IEnumerable<User>> GetAllUsersAsync();

    }
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User> CreateUserAsync(RegisterRequest request)
        {
            Expression<Func<User, bool>> predicate = user => user.UserName == request.Username;
            var users = await _userRepository.FindAsync(predicate);
            if(users.Count() == 0)
            {
                User newUser = new User{
                    UserName = request.Username,
                    Password = request.Password,
                    Name = request.Name
                };
                
                await _userRepository.AddAsync(newUser);
                return newUser;
            }
            else
            {
                throw new InvalidDataException("Username already present");
            }
            

        }

        public async Task DeleteUserAsync(long id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new InvalidDataException("User id not found");
            }
            await _userRepository.DeleteAsync(user);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetUserByIdAsync(long id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User> GetUserByUserNameAsync(string name)
        {
            Expression<Func<User, bool>> predicate = user => user.UserName == name;
            var user = (await _userRepository.FindAsync(predicate)).FirstOrDefault();

            if (user == null)
            {
                throw new InvalidDataException("User not found");
            }

            return user;
        }

        public async Task<User> LoginUser(LoginRequest loginRequest)
        {
            Expression<Func<User, bool>> predicate = user => user.UserName == loginRequest.Username;
            var user = (await _userRepository.FindAsync(predicate)).FirstOrDefault();
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }
            if (user.Password == loginRequest.Password)
            {
                return user;
            }
            else
            {
                throw new InvalidOperationException("Password incorrect");
            }
        }

        public async Task UpdateUserAsync(long id, User user)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser != null)
            {
                existingUser.UserName = user.UserName;
                existingUser.Password = user.Password; 
                existingUser.Name = user.Name;
                existingUser.Balance = user.Balance;
                await _userRepository.UpdateAsync(existingUser);
            }
            else
            {
                throw new InvalidOperationException("User does not exist");
            }
            
        }
    }

}
