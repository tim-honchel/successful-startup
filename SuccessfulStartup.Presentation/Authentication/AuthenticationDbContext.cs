using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Duende.IdentityServer.EntityFramework.Options;
using System.Net.NetworkInformation;




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