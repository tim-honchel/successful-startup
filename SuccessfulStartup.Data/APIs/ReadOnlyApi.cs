using AutoMapper; // for IMapper
using SuccessfulStartup.Data.Contexts;
using SuccessfulStartup.Data.Repositories.ReadOnly;
using SuccessfulStartup.Domain.APIs;
using SuccessfulStartup.Domain.Entities;

namespace SuccessfulStartup.Data.APIs
{
    public class ReadOnlyApi : IReadOnlyApi // single API manages all repositories; individual repositories perform table-specific CRUD operations
    {
        private AuthenticationDbContextFactory _factory; // used to create a new context each time a database connection is needed, improving thread safety
        private IMapper _mapper;
        private BusinessPlanReadOnlyRepository _repositoryForBusinessPlan;

        public ReadOnlyApi(AuthenticationDbContextFactory factory, IMapper mapper) // factory and mapper are injected from DataLayerConfiguration
        {
            _factory = factory;
            _mapper = mapper;
            _repositoryForBusinessPlan = new BusinessPlanReadOnlyRepository(_factory, _mapper);
        }
        public virtual async Task<List<BusinessPlanDomain>> GetAllPlansByAuthorId(string authorId)
        {
            return await _repositoryForBusinessPlan.GetAllPlansByAuthorIdAsync(authorId);
        }
        public virtual async Task<string> GetUserIdByUsername(string username)
        {
            return await _repositoryForBusinessPlan.GetUserIdByUsernameAsync(username);
        }
    }
}
