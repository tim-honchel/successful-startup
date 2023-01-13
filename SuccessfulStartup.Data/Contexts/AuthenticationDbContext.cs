using Microsoft.AspNetCore.ApiAuthorization.IdentityServer; // for ApiAuthorizationDbContext base class
using Microsoft.EntityFrameworkCore; // for DbSet, DbContextOptions, DbContextOptionsBuilder, and ModelBuilder
using SuccessfulStartup.Data.Authentication;
using SuccessfulStartup.Data.Entities;

namespace SuccessfulStartup.Data.Contexts
{
    public class AuthenticationDbContext : ApiAuthorizationDbContext<AppUser> // context creates a session where it is possible to interact with database objects, base class is unique to Identity Server
    {
        public string? _connectionString;
        public DbSet<BusinessPlan> BusinessPlans { get; set; }
        public AuthenticationDbContext(DbContextOptions options, string? connectionString = null) : base(options, new OperationalStoreOptionsMigrations()) // manages persistence of tokens, grants, cache, etc.
        {
            _connectionString= connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //options.UseSqlServer(_connectionString);
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // includes the tables in the base class (Claims, Roles, Users, User Claims, User Logins, User Roles, User Tokens, etc.)
        }
    }
}