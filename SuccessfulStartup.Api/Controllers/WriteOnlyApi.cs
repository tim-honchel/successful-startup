// TODO: delete and replace this temporary workaround

using AutoMapper;
using SuccessfulStartup.Data.Contexts;
using SuccessfulStartup.Data.Entities;
using SuccessfulStartup.Data.Mapping;
using SuccessfulStartup.Data.Repositories.WriteOnly;

namespace SuccessfulStartup.Api.Controllers
{
    public class WriteOnlyApi // single API manages all repositories; individual repositories perform table-specific CRUD operations
    {
        private AuthenticationDbContextFactory _factory; // used to create a new context each time a database connection is needed, improving thread safety
        private IMapper _mapper;
        private BusinessPlanWriteOnlyRepository _repositoryForBusinessPlan;
        private EntityConverter _converter;

        public WriteOnlyApi()
        {
            _factory = new AuthenticationDbContextFactory();
            _mapper = AllMappingProfiles.GetMapper();
            _repositoryForBusinessPlan = new BusinessPlanWriteOnlyRepository(_factory, _mapper);
            _converter = new EntityConverter(_mapper);
        }

        public async Task UpdatePlan(BusinessPlan planToUpdate)
        {
            await _repositoryForBusinessPlan.UpdatePlanAsync(_converter.Convert(planToUpdate));
        }

        public async Task SaveNewPlan(BusinessPlan planToSave)
        {
            await _repositoryForBusinessPlan.SaveNewPlanAsync(_converter.Convert(planToSave));
        }
    }
}
