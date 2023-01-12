using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SuccessfulStartup.Data.Contexts;
using Microsoft.AspNetCore.Identity;
using SuccessfulStartup.Domain.Entities;
using SuccessfulStartup.Domain.Repositories.WriteOnly;
using SuccessfulStartup.Data.Authentication;

namespace SuccessfulStartup.Data.Repositories.WriteOnly
{
    public class BusinessPlanWriteOnlyRepository : IBusinessPlanWriteOnlyRepository
    {
        public IDbContextFactory<AuthenticationDbContext> _factory;
        public BusinessPlanWriteOnlyRepository(IDbContextFactory<AuthenticationDbContext> factory) 
        {
            _factory = factory;
        }
        public async void SaveNewPlanAsync(BusinessPlanAbstract planToSave)
        {
            using var context = _factory.CreateDbContext();
            planToSave.AuthorId = "4c6afe26-eb99-4d98-9f2f-5bed3a7bdd5b"; // TODO: figure out how to get current user Id
            await context.AddAsync(planToSave);
            await context.SaveChangesAsync();
        }
    }
}
