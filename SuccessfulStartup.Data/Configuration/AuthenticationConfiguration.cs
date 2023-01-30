using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SuccessfulStartup.Data.Authentication;
using SuccessfulStartup.Data.Contexts;

namespace SuccessfulStartup.Data.Configuration
{
    public static class AuthenticationConfiguration // configure services for Identity
    {

        private const string connectionKey = "ConnectionStrings:IdentityConnectionString";
        private const string fileLocation = "appsettings.json";
        private static string connectionString = new ConfigurationBuilder().AddJsonFile(fileLocation).Build()[connectionKey]; // gets database connection string from appsettings.json

        public static IServiceCollection AddDataScope(this IServiceCollection services)
        {
            services.AddDbContextFactory<AuthenticationDbContext>(options => options.UseSqlServer(connectionString));
            services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AuthenticationDbContext>(); // adds default UI for Identity, eliminating need to create custom register and login pages, also requires account verification prior to first login
            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<AppUser>>(); // periodically checks whether user credentials are still valid
            services.AddScoped<AuthenticationDbContextFactory>();
            services.AddTransient<IEmailSender, EmailSender>(); // enables email sends
            return services;
        }
    }
}
