using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace SuccessfulStartup.Presentation.Authentication
{
    public class AuhtenticationDbContextFactory : IDesignTimeDbContextFactory<AuthenticationDbContext>
    {
        public AuthenticationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AuthenticationDbContext>();
            optionsBuilder.UseSqlServer("Data Source = ./IdentitySuccessfulStart.db");
            return new AuthenticationDbContext(optionsBuilder.Options);
        }

    }
}
