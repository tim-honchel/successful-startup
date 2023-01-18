using AutoMapper;
using Microsoft.EntityFrameworkCore; // for database context
using SuccessfulStartup.Data.Contexts;
using SuccessfulStartup.Data.Repositories.WriteOnly;
using SuccessfulStartup.Domain.APIs;
using SuccessfulStartup.Domain.Entities;

namespace SuccessfulStartup.Data.APIs
{
    public class WriteOnlyApi : IWriteOnlyApi // single API manages all repositories; individual repositories perform table-specific CRUD operations
    {
        private AuthenticationDbContextFactory _factory; // used to create a new context each time a database connection is needed, improving thread safety
        private IMapper _mapper;
        private BusinessPlanWriteOnlyRepository _repositoryForBusinessPlan;

        public WriteOnlyApi(AuthenticationDbContextFactory factory, IMapper mapper) // factory and mapper are injected from DataLayerConfiguration
        {
            _factory = factory;
            _mapper = mapper;
            _repositoryForBusinessPlan = new BusinessPlanWriteOnlyRepository(_factory, _mapper) ;
        }

        public async Task SaveNewPlan(BusinessPlanDomain planToSave)
        {
            await _repositoryForBusinessPlan.SaveNewPlanAsync(planToSave);
        }
    }
}
