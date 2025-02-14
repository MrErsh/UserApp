#if DEBUG
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace UserApp.Dal.Providers.MSSQLServer.Design
{
    public class DesignTimeUserDbFactory : IDesignTimeDbContextFactory<UserDbContext>
    {
        public UserDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
            // modify
            optionsBuilder.UseSqlServer("data source=.\\SQLEXPRESS;initial catalog=userapp_db;trusted_connection=true;TrustServerCertificate=True");
            return new UserDbContext(optionsBuilder.Options);
        }
    }

}
#endif