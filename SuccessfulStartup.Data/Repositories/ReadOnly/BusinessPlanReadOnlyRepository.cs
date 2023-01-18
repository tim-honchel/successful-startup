using AutoMapper; // for AutoMapper
using Microsoft.EntityFrameworkCore; // for database queries
using SuccessfulStartup.Data.Contexts;
using SuccessfulStartup.Domain.Entities;
using SuccessfulStartup.Domain.Repositories.ReadOnly;

namespace SuccessfulStartup.Data.Repositories.ReadOnly
{
    public class BusinessPlanReadOnlyRepository : IBusinessPlanReadOnlyRepository // performs CRUD operations on the BusinessPlan table
    {
        private AuthenticationDbContextFactory _factory; // creates context for database connection
        private IMapper _mapper; // converts data and domain entities
        public BusinessPlanReadOnlyRepository(AuthenticationDbContextFactory factory, IMapper mapper)
        {
            _factory = factory;
            _mapper = mapper;
        }
        public async Task<List<BusinessPlanDomain>> GetAllPlansByAuthorIdAsync(string authorId)
        {
            using var context = _factory.CreateDbContext(new string[] { });
            var plansByUser = await context.BusinessPlans.Where(plan => plan.AuthorId == authorId).ToListAsync();
            return _mapper.Map<List<BusinessPlanDomain>>(plansByUser);
        }
        public async Task<string> GetUserIdByUsernameAsync(string username)
        {
            using var context = _factory.CreateDbContext(new string[] { });
            var authorId = await context.Users.Where(user => user.UserName== username).Select(user => user.Id).SingleOrDefaultAsync();
            return authorId;
        }
    }
}
