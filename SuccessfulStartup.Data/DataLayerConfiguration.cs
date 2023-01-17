using Microsoft.AspNetCore.Components.Authorization; // for AuthenticationStateProvider
using Microsoft.AspNetCore.Identity.UI.Services; //for IEmailSender
using Microsoft.EntityFrameworkCore; // for UseSqlServer
using Microsoft.Extensions.Configuration; // for ConfigurationBuilder
using Microsoft.Extensions.DependencyInjection; // for IServiceCollection
using SuccessfulStartup.Data.APIs;
using SuccessfulStartup.Data.Authentication;
using SuccessfulStartup.Data.Contexts;
using SuccessfulStartup.Data.Repositories.WriteOnly;
using SuccessfulStartup.Domain.Repositories.WriteOnly;

namespace SuccessfulStartup.Data
{
    public static class DataLayerConfiguration
    {
        private const string connectionKey = "ConnectionStrings:IdentityConnectionString";
        private const string fileLocation = "appsettings.json";
        private static string connectionString = new ConfigurationBuilder().AddJsonFile(fileLocation).Build()[connectionKey]; // gets database connection string from appsettings.json

        public static IServiceCollection AddDataScope(this IServiceCollection services)
        {
            services.AddDbContextFactory<AuthenticationDbContext>(options => options.UseSqlServer(connectionString));
            services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AuthenticationDbContext>(); // adds default UI for Identity, eliminating need to create custom register and login pages, also requires account verification prior to first login
            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<AppUser>>(); // periodically checks whether user credentials are still valid
            services.AddScoped<IWriteOnlyApi, WriteOnlyApi>();
            services.AddScoped<IReadOnlyApi, ReadOnlyApi>();
            services.AddScoped<MappingProfile>(); 
            services.AddTransient<IEmailSender, EmailSender>(); // enables email sends
            services.AddTransient<IBusinessPlanWriteOnlyRepository, BusinessPlanWriteOnlyRepository>(); 
            return services;
        }
    }
}
