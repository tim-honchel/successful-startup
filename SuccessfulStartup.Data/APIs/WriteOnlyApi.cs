using Microsoft.EntityFrameworkCore;
using SuccessfulStartup.Data.Contexts;
using SuccessfulStartup.Data.Repositories.WriteOnly;
using SuccessfulStartup.Domain.Entities;

namespace SuccessfulStartup.Data.APIs
{
    public class WriteOnlyApi : IWriteOnlyApi
    {
        public IDbContextFactory<AuthenticationDbContext> _factory;
        public BusinessPlanWriteOnlyRepository _repositoryForBusinessPlan;

        public WriteOnlyApi(IDbContextFactory<AuthenticationDbContext> factory)
        {
            _factory = factory;
            _repositoryForBusinessPlan = new BusinessPlanWriteOnlyRepository(_factory) ;
        }

        public void SaveNewPlan(BusinessPlanAbstract planToSave)
        {
            _repositoryForBusinessPlan.SaveNewPlanAsync(planToSave);
        }
    }
}
