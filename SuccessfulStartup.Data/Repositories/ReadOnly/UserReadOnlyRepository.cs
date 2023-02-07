using Microsoft.EntityFrameworkCore;
using SuccessfulStartup.Data.Contexts;
using SuccessfulStartup.Domain.Repositories.ReadOnly;
using System.Numerics;


namespace SuccessfulStartup.Data.Repositories.ReadOnly
{
    public class UserReadOnlyRepository : IUserReadOnlyRepository
    {
        private readonly PlanDbContextFactory _factory; // creates context for database connection

        public UserReadOnlyRepository(PlanDbContextFactory factory)
        {
            _factory = factory;
        }

        public async Task<bool> VerifyUserAsync(string userId, string securityStamp)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(securityStamp)) { throw new ArgumentNullException(); }

            using var context = _factory.CreateDbContext();

            var verifiable = await context.Users.AnyAsync(user => user.AuthorId == userId && user.SecurityStamp == securityStamp);

            return verifiable;
        }

        public async Task<bool> VerifyUserAsync(int planId, string securityStamp)
        {
            if (planId<=0 || string.IsNullOrWhiteSpace(securityStamp)) { throw new ArgumentNullException(); }
            
            using var context = _factory.CreateDbContext();

            var userId = await context.BusinessPlans.Where(plan => plan.Id == planId).Select(plan => plan.AuthorId).FirstOrDefaultAsync();
            if (userId == null) { throw new NullReferenceException(nameof(planId)); }

            var verifiable = await context.Users.AnyAsync(user => user.AuthorId == userId && user.SecurityStamp == securityStamp);

            return verifiable;

            
            


        }
    }
}
