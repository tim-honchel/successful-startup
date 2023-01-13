using Microsoft.EntityFrameworkCore; // for database context
using SuccessfulStartup.Data.Contexts;
using SuccessfulStartup.Data.Repositories.WriteOnly;
using SuccessfulStartup.Domain.Entities;

namespace SuccessfulStartup.Data.APIs
{
    public class WriteOnlyApi : IWriteOnlyApi // single API manages all repositories; individual repositories perform table-specific CRUD operations
    {
        public IDbContextFactory<AuthenticationDbContext> _factory; // used to create a new context each time a database connection is needed, improving thread safety
        public BusinessPlanWriteOnlyRepository _repositoryForBusinessPlan;

        public WriteOnlyApi(IDbContextFactory<AuthenticationDbContext> factory)
        {
            _factory = factory;
            _repositoryForBusinessPlan = new BusinessPlanWriteOnlyRepository(_factory) ;
        }

        public async Task SaveNewPlan(BusinessPlanDomain planToSave)
        {
            await _repositoryForBusinessPlan.SaveNewPlanAsync(planToSave);
        }
    }
}
