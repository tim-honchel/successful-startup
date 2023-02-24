using Microsoft.Extensions.Configuration; // for ConfigurationBuilder
using Microsoft.Extensions.DependencyInjection; // for IServiceCollection, AddAutoMapper
using SuccessfulStartup.Domain.Repositories.ReadOnly;
using SuccessfulStartup.Domain.Repositories.WriteOnly;

namespace SuccessfulStartup.Data.Configuration
{
    public static class DataLayerConfiguration // configure additional services and dependency injections needed in the data layer; called in program.cs
    {
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
