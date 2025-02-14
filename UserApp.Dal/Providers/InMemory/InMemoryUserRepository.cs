using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserApp.Dal.Model;

namespace UserApp.Dal.Providers.InMemory
{
    /// <summary>
    /// Репозиторий пользователей, хранящийся в памяти.
    /// </summary>
    public class InMemoryUserRepository : IUserRepository
    {
        private const string USER_NOT_FOUND_ERROR = "User not found.";
        private const string ALREADY_EXISTS_ERROR = "A user with the same ID already exists.";

        #region Fields

        private IList<User> _users = new List<User>();
        
        #endregion

//#if DEBUG
//        public InMemoryUserRepository()
//        {
//            var random = new Random();
//            for (var i = 0; i < 25; i++)
//            {
//                var num = random.Next();
//                _users.Add(new User
//                {
//                    Id = Guid.NewGuid(),
//                    Login = $"Login {num}",
//                    FirstName = $"First name {num}",
//                    LastName = $"Last name {num}"
//                });
//            }
//        }
//#endif

        #region Implementation of IUserRepository

        /// <inheritdoc/>
        public Task<ICollection<User>> GetAllAsync()
        {
            ICollection<User> r = _users;
            return Task.FromResult(r);
        }

        /// <inheritdoc/>
        /// <exception cref="Exception"></exception>
        public Task AddAsync(User user)
        {
            var existing = _users.FirstOrDefault(u => u.Id == user.Id);
            if (existing != null)
            {
                throw new Exception(ALREADY_EXISTS_ERROR);
            }

            if (user.Id == default)
                user.Id = Guid.NewGuid();

            _users.Add(user);

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task<bool> DeleteAsync(Guid id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _users.Remove(user);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        ///<inheritdoc/>
        /// <exception cref="Exception"></exception>
        public Task UpdateAsync(User user)
        {
            var existing = _users.FirstOrDefault(u => user.Id == user.Id);
            if (existing == null)
            {
                throw new Exception(USER_NOT_FOUND_ERROR);
            }

            existing.Login = user.Login;
            existing.LastName = user.LastName;
            existing.FirstName = user.FirstName;

            return Task.CompletedTask;
        }
       
        #endregion

        public void Initialize(IEnumerable<User> users) => _users = users.ToList();
    }
}
