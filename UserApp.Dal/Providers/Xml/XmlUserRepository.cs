using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UserApp.Dal.Model;
using UserApp.Dal.Providers.InMemory;

namespace UserApp.Dal.Providers.Xml
{
    /// <summary>
    /// Репозиторий пользователей, использующий в качестве хранилища XML файл.
    /// </summary>
    public class XmlUserRepository : IUserRepository
    {
        private readonly InMemoryUserRepository _repository;

        private readonly XmlSerializer _serializer;
        private readonly string _path;

        public XmlUserRepository(string path, InMemoryUserRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(_repository));

            _path = path;
            _serializer = new XmlSerializer(typeof(User[]));

            Initialize();
        }

        #region Implementation of IUserRepository

        /// <inheritdoc/>
        public Task<ICollection<User>> GetAllAsync()
        {
            using var fs = File.Open(_path, FileMode.OpenOrCreate);
            var users = _serializer.Deserialize(fs) as ICollection<User>;
            return Task.FromResult(users);
        }

        /// <inheritdoc/>
        public async Task AddAsync(User user)
        {
            if (user.Id == default)
                user.Id = Guid.NewGuid();

            await _repository.AddAsync(user);

            var users = await _repository.GetAllAsync();
            using (var fs = File.Open(_path, FileMode.Truncate))
            {
                _serializer.Serialize(fs, users.ToArray());
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var result = await _repository.DeleteAsync(id);
            if (result == true)
                await UpdateXmlDbAsync();

            return result;
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(User user)
        {
            await _repository.UpdateAsync(user);
            await UpdateXmlDbAsync();
        }

        #endregion

        #region Private methods

        private async Task Initialize()
        {
            if (File.Exists(_path))
            {
                var users = await GetAllAsync();
                _repository.Initialize(users);
            }
            else
            {
                using (var fs = File.Open(_path, FileMode.OpenOrCreate))
                {
                    _serializer.Serialize(fs, new User[0]);
                }
            }
        }

        private async Task UpdateXmlDbAsync()
        {
            var users = await _repository.GetAllAsync();
            using (var fs = File.Open(_path, FileMode.Truncate))
            _serializer.Serialize(fs, users.ToArray());
        }

        #endregion
    }
}
