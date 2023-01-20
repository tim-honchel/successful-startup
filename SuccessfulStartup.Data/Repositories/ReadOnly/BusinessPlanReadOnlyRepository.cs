using AutoMapper; // for AutoMapper
using Duende.IdentityServer.Extensions;
using Microsoft.EntityFrameworkCore; // for database queries
using SuccessfulStartup.Data.Contexts;
using SuccessfulStartup.Data.Entities;
using SuccessfulStartup.Domain.Entities;
using SuccessfulStartup.Domain.Repositories.ReadOnly;

namespace SuccessfulStartup.Data.Repositories.ReadOnly
{
    public class BusinessPlanReadOnlyRepository : IBusinessPlanReadOnlyRepository // performs CRUD operations on the BusinessPlan table
    {
        private AuthenticationDbContextFactory _factory; // creates context for database connection TODO: is it possible to use interface instead?
        private IMapper _mapper; // converts data and domain entities
        public BusinessPlanReadOnlyRepository(AuthenticationDbContextFactory factory, IMapper mapper)
        {
            _factory = factory;
            _mapper = mapper;
        }
        public async Task<List<BusinessPlanDomain>> GetAllPlansByAuthorIdAsync(string authorId)
        {
            if (string.IsNullOrWhiteSpace(authorId)) {throw new ArgumentNullException(nameof(authorId));}

            using var context = _factory.CreateDbContext();

            try
            {
                var plansByUser = await context.BusinessPlans.Where(plan => plan.AuthorId == authorId).ToListAsync();
                return _mapper.Map<List<BusinessPlanDomain>>(plansByUser);
            }
            catch (ArgumentNullException)
            {
                return new List<BusinessPlanDomain>() { }; // returns empty list if no results are found
            }

        }

        public async Task<BusinessPlanDomain> GetPlanByIdAsync(int id)
        {
            if (id == 0) { throw new ArgumentNullException(nameof(id)); }

            using var context = _factory.CreateDbContext();

            try
            {
                var plan = await context.BusinessPlans.Where(plan => plan.Id == id).SingleOrDefaultAsync();
                return _mapper.Map<BusinessPlanDomain>(plan);
            }
            catch (ArgumentNullException)
            {
                throw new NullReferenceException(nameof(id)); // if no user with that name is found
            }
        }

        public async Task<string> GetUserIdByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) {throw new ArgumentNullException(nameof(username));}

            using var context = _factory.CreateDbContext();

            try
            {
                var authorId = await context.Users.Where(user => user.UserName == username).Select(user => user.Id).SingleOrDefaultAsync();
                return authorId;
            }
            catch (ArgumentNullException)
            {
                throw new NullReferenceException(nameof(username)); // if no user with that name is found
            }
        }
    }
}
