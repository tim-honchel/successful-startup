using Microsoft.EntityFrameworkCore; // for database context
using SuccessfulStartup.Data.Contexts;
using SuccessfulStartup.Data.Repositories.ReadOnly;
using SuccessfulStartup.Domain.APIs;
using SuccessfulStartup.Domain.Entities;

namespace SuccessfulStartup.Data.APIs
{
    public class ReadOnlyApi : IReadOnlyApi // single API manages all repositories; individual repositories perform table-specific CRUD operations
    {
        public IDbContextFactory<AuthenticationDbContext> _factory; // used to create a new context each time a database connection is needed, improving thread safety
        public BusinessPlanReadOnlyRepository _repositoryForBusinessPlan;

        public ReadOnlyApi(IDbContextFactory<AuthenticationDbContext> factory)
        {
            _factory = factory;
            _repositoryForBusinessPlan = new BusinessPlanReadOnlyRepository(_factory);
        }
        public async Task<List<BusinessPlanDomain>> GetAllPlansByAuthorId(string authorId)
        {
            return await _repositoryForBusinessPlan.GetAllPlansByAuthorIdAsync(authorId);
        }
        public async Task<string> GetUserIdByUsername(string username)
        {
            return await _repositoryForBusinessPlan.GetUserIdByUsernameAsync(username);
        }
    }
}
