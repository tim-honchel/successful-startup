using Microsoft.EntityFrameworkCore; // for DbContextOptionsBuilder
using Microsoft.EntityFrameworkCore.Design; // for IDesignTimeDbContextFactory interface
using Microsoft.Extensions.Configuration; // for accessing connection strings

namespace SuccessfulStartup.Data.Contexts
{
    internal class AuthenticationDbContextFactory : IDesignTimeDbContextFactory<AuthenticationDbContext>
    {
        private const string connectionKey = "ConnectionStrings:IdentityConnectionString";
        private const string fileLocation = "appsettings.json";
        private static string connectionString = new ConfigurationBuilder().AddJsonFile(fileLocation).Build()[connectionKey];
        public AuthenticationDbContext CreateDbContext(string[] args)
        {            
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder<AuthenticationDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new AuthenticationDbContext(optionsBuilder.Options);
        }
    }
}
