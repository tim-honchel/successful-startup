using Microsoft.AspNetCore.Components.Authorization; // for AuthenticationStateProvider
using Microsoft.AspNetCore.Identity.UI.Services; //for IEmailSender
using Microsoft.EntityFrameworkCore; // for UseSqlServer
using Microsoft.Extensions.Configuration; // for ConfigurationBuilder
using Microsoft.Extensions.DependencyInjection; // for IServiceCollection, AddAutoMapper
using SuccessfulStartup.Data.APIs;
using SuccessfulStartup.Data.Authentication;
using SuccessfulStartup.Data.Contexts;
using SuccessfulStartup.Data.Mapping;
using SuccessfulStartup.Data.Repositories.ReadOnly;
using SuccessfulStartup.Data.Repositories.WriteOnly;
using SuccessfulStartup.Domain.APIs;
using SuccessfulStartup.Domain.Repositories.ReadOnly;
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

            
            services.AddAutoMapper(typeof(AllMappingProfiles).Assembly); // allows injection of IMapper for mapping data and domain entities; requires NuGet package: AutoMapper.Extensions.Microsoft.DependencyInjection 
            services.AddDbContextFactory<AuthenticationDbContext>(options => options.UseSqlServer(connectionString));
            services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AuthenticationDbContext>(); // adds default UI for Identity, eliminating need to create custom register and login pages, also requires account verification prior to first login
            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<AppUser>>(); // periodically checks whether user credentials are still valid
            services.AddScoped<AuthenticationDbContextFactory>();
            //services.AddScoped<IWriteOnlyApi, WriteOnlyApi>(); // requires presentation layer to access domain layer
            //services.AddScoped<IReadOnlyApi, ReadOnlyApi>(); // requires presentation layer to access domain layer
            services.AddScoped<EntityConverter>();
            services.AddScoped<ReadOnlyApi>();
            services.AddScoped<WriteOnlyApi>();
            services.AddTransient<IEmailSender, EmailSender>(); // enables email sends
            //services.AddTransient<IBusinessPlanReadOnlyRepository, BusinessPlanReadOnlyRepository>();
            //services.AddTransient<IBusinessPlanWriteOnlyRepository, BusinessPlanWriteOnlyRepository>();
            services.AddTransient<BusinessPlanReadOnlyRepository>();
            services.AddTransient<BusinessPlanWriteOnlyRepository>();
            services.AddTransient<UserReadOnlyRepository>();
            return services;
        }
    }
}
