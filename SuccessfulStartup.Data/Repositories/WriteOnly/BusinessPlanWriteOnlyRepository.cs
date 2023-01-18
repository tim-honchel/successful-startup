using AutoMapper; // for IMapper
using Microsoft.EntityFrameworkCore; // for IDbContextFactory
using SuccessfulStartup.Data.Contexts;
using SuccessfulStartup.Data.Entities;
using SuccessfulStartup.Domain.Entities;
using SuccessfulStartup.Domain.Repositories.WriteOnly;


namespace SuccessfulStartup.Data.Repositories.WriteOnly
{
    public class BusinessPlanWriteOnlyRepository : IBusinessPlanWriteOnlyRepository
    {
        private AuthenticationDbContextFactory _factory;
        private IMapper _mapper;
        public BusinessPlanWriteOnlyRepository(AuthenticationDbContextFactory factory, IMapper mapper)
        {
            _factory = factory;
            _mapper = mapper;
        }
        public async Task SaveNewPlanAsync(BusinessPlanDomain planToSave)
        {
            using var context = _factory.CreateDbContext();
            await context.AddAsync(_mapper.Map<BusinessPlan>(planToSave));
            await context.SaveChangesAsync();
        }
    }
}
