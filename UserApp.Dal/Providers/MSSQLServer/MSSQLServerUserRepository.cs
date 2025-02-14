using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserApp.Dal.Model;
using UserEntity = UserApp.Dal.Providers.MSSQLServer.Model.User;

namespace UserApp.Dal.Providers.MSSQLServer
{
    /// <summary>
    /// Репозиторий пользователей, использующий в качестве хранилища MS SQL Server.
    /// </summary>
    public class MSSQLServerUserRepository : IUserRepository
    {
        private readonly IDbContextFactory<UserDbContext> _contextFactory;

        public MSSQLServerUserRepository(IDbContextFactory<UserDbContext> dbContextFactory)
        {
            _contextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        #region Implementation of IUserRepository

        /// <inheritdoc/>
        public async Task<ICollection<User>> GetAllAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);
            var users = await context.Users.ToListAsync().ConfigureAwait(false);
            return users.Cast<User>().ToList();
        }

        /// <inheritdoc/>
        public async Task AddAsync(User user)
        {
            var entity = ConvertUserToEntity(user);
            using var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);
            await context.Users.AddAsync(entity).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(Guid id)
        {
            using var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);
            var entity = await context.Users.FirstOrDefaultAsync(u => u.Id == id).ConfigureAwait(false);
            if (entity == null)
                return false;

            context.Remove(entity);
            await context.SaveChangesAsync().ConfigureAwait(false);
            return true;           
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(User user)
        {
            using var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);
            var entity = await context.Users.FirstAsync(u => u.Id == user.Id).ConfigureAwait(false);

            entity.Login = user.Login;
            entity.FirstName = user.FirstName;
            entity.LastName = user.LastName;

            context.Update(entity);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        #endregion

        #region Private methods

        private UserEntity ConvertUserToEntity(User user)
        {
            return new UserEntity
            {
                Id = user.Id,
                Login = user.Login,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        #endregion
    }
}
