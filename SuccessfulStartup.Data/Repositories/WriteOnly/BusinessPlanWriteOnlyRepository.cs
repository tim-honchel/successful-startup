using Microsoft.EntityFrameworkCore; // for IDbContextFactory
using SuccessfulStartup.Data.Contexts;
using SuccessfulStartup.Domain.Entities;
using SuccessfulStartup.Domain.Repositories.WriteOnly;


namespace SuccessfulStartup.Data.Repositories.WriteOnly
{
    public class BusinessPlanWriteOnlyRepository : IBusinessPlanWriteOnlyRepository
    {
        public IDbContextFactory<AuthenticationDbContext> _factory;
        public MappingProfile _mapper = new MappingProfile();
        public BusinessPlanWriteOnlyRepository(IDbContextFactory<AuthenticationDbContext> factory) 
        {
            _factory = factory;
        }
        public async Task SaveNewPlanAsync(BusinessPlanDomain planToSave)
        {
            using var context = _factory.CreateDbContext();
            await context.AddAsync(_mapper.BusinessPlanDomainToData(planToSave));
            await context.SaveChangesAsync();
        }
    }
}
