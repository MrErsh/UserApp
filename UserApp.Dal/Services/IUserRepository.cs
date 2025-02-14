using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserApp.Dal.Model;

namespace UserApp.Dal
{
    /// <summary>
    /// Репозиторий пользователей.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Получить все записи.
        /// </summary>
        /// <returns>Коллекция пользователей.</returns>
        Task<ICollection<User>> GetAllAsync();

        /// <summary>
        /// Добавить пользователя.
        /// </summary>
        /// <param name="user">Модель пользователя.</param>
        Task AddAsync(User user);

        /// <summary>
        /// Удалить пользователя.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Обновляет информацию о пользователе.
        /// </summary>
        /// <param name="user">Модель пользователя.</param>
        Task UpdateAsync(User user);
    }
}

