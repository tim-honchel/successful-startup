using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;




namespace SuccessfulStartup.Presentation.Authentication
{
    public class AuthenticationDbContext : ApiAuthorizationDbContext<AppUser>
    {
        public AuthenticationDbContext(DbContextOptions options) : base(options, new OperationalStoreOptionsMigrations())
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}