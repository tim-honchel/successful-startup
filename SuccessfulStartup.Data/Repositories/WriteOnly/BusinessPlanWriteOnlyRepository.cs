using AutoMapper; // for IMapper
using Microsoft.EntityFrameworkCore; // for DbUpdateException
using SuccessfulStartup.Data.Contexts;
using SuccessfulStartup.Data.Entities;
using SuccessfulStartup.Domain.Entities;
using SuccessfulStartup.Domain.Repositories.WriteOnly;


namespace SuccessfulStartup.Data.Repositories.WriteOnly
{
    public class BusinessPlanWriteOnlyRepository : IBusinessPlanWriteOnlyRepository
    {
        private readonly AuthenticationDbContextFactory _factory;
        private readonly IMapper _mapper;
        public BusinessPlanWriteOnlyRepository(AuthenticationDbContextFactory factory, IMapper mapper)
        {
            _factory = factory;
            _mapper = mapper;
        }
        public async Task DeletePlanAsync(BusinessPlanDomain planToDelete)
        {
            using var context = _factory.CreateDbContext();

            try
            {
                context.Remove(_mapper.Map<BusinessPlan>(planToDelete));
                context.SaveChanges();
            }
            catch (DbUpdateException exception) 
            {
                throw new DbUpdateException();
            }
        }
        public async Task UpdatePlanAsync(BusinessPlanDomain planToUpdate)
        {
            using var context = _factory.CreateDbContext();
            
            try
            {
                context.Update(_mapper.Map<BusinessPlan>(planToUpdate));
                context.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                throw new DbUpdateException();
            }

        }
        public async Task SaveNewPlanAsync(BusinessPlanDomain planToSave)
        {
            if (planToSave == null || string.IsNullOrWhiteSpace(planToSave.Name) || string.IsNullOrWhiteSpace(planToSave.Description) || string.IsNullOrWhiteSpace(planToSave.AuthorId) ) { throw new ArgumentNullException(nameof(planToSave)); }

            using var context = _factory.CreateDbContext();

            try
            {
                await context.AddAsync(_mapper.Map<BusinessPlan>(planToSave));
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                throw new DbUpdateException();
            }
        }
    }
}
