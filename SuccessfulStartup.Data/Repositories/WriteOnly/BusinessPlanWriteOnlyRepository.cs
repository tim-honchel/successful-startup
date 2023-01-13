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
            planToSave.AuthorId = "4c6afe26-eb99-4d98-9f2f-5bed3a7bdd5b"; // TODO: figure out how to get current user Id
            await context.AddAsync(_mapper.BusinessPlanDomainToData(planToSave));
            await context.SaveChangesAsync();
        }
    }
}
