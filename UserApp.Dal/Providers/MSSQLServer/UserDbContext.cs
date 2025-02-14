using Microsoft.EntityFrameworkCore;
using UserApp.Dal.Providers.MSSQLServer.Model;

namespace UserApp.Dal.Providers.MSSQLServer
{
    public sealed class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}