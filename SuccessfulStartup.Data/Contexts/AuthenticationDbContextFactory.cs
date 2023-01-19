using Microsoft.EntityFrameworkCore; // for DbContextOptionsBuilder
using Microsoft.EntityFrameworkCore.Design; // for IDesignTimeDbContextFactory interface
using Microsoft.Extensions.Configuration; // for accessing connection strings
using System.Runtime.CompilerServices; // for InternalsVisibleTo

[assembly: InternalsVisibleTo("SuccessfulStartup.DataTests")] // allows tests to access internal members

namespace SuccessfulStartup.Data.Contexts
{
    public class AuthenticationDbContextFactory : IDesignTimeDbContextFactory<AuthenticationDbContext>
    {
        private const string _connectionKey = "ConnectionStrings:IdentityConnectionString";
        private const string _fileLocation = "appsettings.json";
        internal static string _connectionString = new ConfigurationBuilder().AddJsonFile(_fileLocation).Build()[_connectionKey]; // internal for testing

        public virtual AuthenticationDbContext CreateDbContext(string[] args) // required by interface
        {            
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder<AuthenticationDbContext>();
            optionsBuilder.UseSqlServer(_connectionString);
            return new AuthenticationDbContext(optionsBuilder.Options);
        }

        public virtual AuthenticationDbContext CreateDbContext() // overload is preferable to avoid having to pass "new string[] {}" every time context is created
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder<AuthenticationDbContext>();
            optionsBuilder.UseSqlServer(_connectionString);
            return new AuthenticationDbContext(optionsBuilder.Options);
        }
    }
}
