using AutoMapper; // for AutoMapper
using Microsoft.EntityFrameworkCore; // for database queries
using SuccessfulStartup.Data.Contexts;
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

            var plansByUser = await context.BusinessPlans.Where(plan => plan.AuthorId == authorId).ToListAsync();
            return _mapper.Map<List<BusinessPlanDomain>>(plansByUser); // returns empty list if no plans are found

        }

        public async Task<BusinessPlanDomain> GetPlanByIdAsync(int id)
        {
            if (id == 0) { throw new ArgumentNullException(nameof(id)); }

            using var context = _factory.CreateDbContext();

            var plan = await context.BusinessPlans.Where(plan => plan.Id == id).SingleOrDefaultAsync(); // returns null if no plan is found

            if (plan == null)
            {
                throw new NullReferenceException(nameof(id)); // if no user with that name is found
            }
            return _mapper.Map<BusinessPlanDomain>(plan);
            
        }

        
    }
}
