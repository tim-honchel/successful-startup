using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection; // for IServiceCollection, AddAutoMapper
using SuccessfulStartup.Data.Contexts;
using SuccessfulStartup.Data.Mapping;
using SuccessfulStartup.Data.Repositories.ReadOnly;
using SuccessfulStartup.Data.Repositories.WriteOnly;
using SuccessfulStartup.Domain.Repositories.ReadOnly;
using SuccessfulStartup.Domain.Repositories.WriteOnly;

namespace SuccessfulStartup.Data.Configuration
{
    public static class DataLayerConfiguration // configure additional services and dependency injections needed in the data layer; called in program.cs
    {

        private const string _connectionKey = "ConnectionStrings:BusinessPlanConnectionString";
        private const string _fileLocation = "appsettings.json";
        private static string _connectionString = new ConfigurationBuilder().AddJsonFile(_fileLocation).Build()[_connectionKey]; // gets database connection string from appsettings.json

        public static IServiceCollection AddDataScope(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AllMappingProfiles).Assembly); // allows injection of IMapper for mapping data and domain entities; requires NuGet package: AutoMapper.Extensions.Microsoft.DependencyInjection 
            services.AddDbContextFactory<PlanDbContext>();
            services.AddScoped<PlanDbContextFactory>();
            services.AddScoped<EntityConverter>();
            services.AddTransient<IBusinessPlanReadOnlyRepository, BusinessPlanReadOnlyRepository>();
            services.AddTransient<IBusinessPlanWriteOnlyRepository, BusinessPlanWriteOnlyRepository>();
            services.AddTransient<IUserReadOnlyRepository, UserReadOnlyRepository>();
            services.AddTransient<IUserWriteOnlyRepository, UserWriteOnlyRepository>();
            return services;
        }
    }
}
