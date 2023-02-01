using AutoMapper; // for IMapper
using Microsoft.EntityFrameworkCore; // for DbUpdateException
using Microsoft.EntityFrameworkCore.Design; // for IDesignTimeDbContextFactory
using SuccessfulStartup.Data.Contexts;
using SuccessfulStartup.Data.Entities;
using SuccessfulStartup.Domain.Entities;
using SuccessfulStartup.Domain.Repositories.WriteOnly;


namespace SuccessfulStartup.Data.Repositories.WriteOnly
{
    public class BusinessPlanWriteOnlyRepository : IBusinessPlanWriteOnlyRepository
    {
        private readonly IDesignTimeDbContextFactory<AuthenticationDbContext> _factory;
        private readonly IMapper _mapper;
        public BusinessPlanWriteOnlyRepository(IDesignTimeDbContextFactory<AuthenticationDbContext> factory, IMapper mapper)
        {
            _factory = factory;
            _mapper = mapper;
        }
        public async Task DeletePlanAsync(BusinessPlanDomain planToDelete)
        {
            using var context = _factory.CreateDbContext(new string[] { });

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
            using var context = _factory.CreateDbContext(new string[] { });
            
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
        public async Task<int> SaveNewPlanAsync(BusinessPlanDomain planToSave)
        {
            if (planToSave == null || string.IsNullOrWhiteSpace(planToSave.Name) || string.IsNullOrWhiteSpace(planToSave.Description) || string.IsNullOrWhiteSpace(planToSave.AuthorId) ) { throw new ArgumentNullException(nameof(planToSave)); }
            var plan = _mapper.Map<BusinessPlan>(planToSave);

            using var context = _factory.CreateDbContext(new string[] { });

            try
            {
                await context.AddAsync(plan);
                await context.SaveChangesAsync();
                return plan.Id; // gets the auto-generated Id, has to be the data entity, not the domain entity
            }
            catch (DbUpdateException exception)
            {
                throw new DbUpdateException();
            }
        }
    }
}
