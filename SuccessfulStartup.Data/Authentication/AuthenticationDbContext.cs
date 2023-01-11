using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using SuccessfulStartup.Data.Entities;

namespace SuccessfulStartup.Data.Authentication
{
    public class AuthenticationDbContext : ApiAuthorizationDbContext<AppUser> // context creates a session where it is possible to interact with database objects, base class is unique to Identity Server
    {
        public DbSet<BusinessPlan> BusinessPlans { get; set; }
        public AuthenticationDbContext(DbContextOptions options) : base(options, new OperationalStoreOptionsMigrations()) // manages persistence of tokens, grants, cache, etc.
        {
            
        }
        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // includes the tables in the base class (Claims, Roles, Users, User Claims, User Logins, User Roles, User Tokens, etc.)
        }
    }
}