using Microsoft.EntityFrameworkCore; // for context executions
using SuccessfulStartup.Domain.Repositories.WriteOnly;

namespace SuccessfulStartup.Data.Repositories.WriteOnly
{
    public class UserWriteOnlyRepository : IUserWriteOnlyRepository
    {
        private readonly PlanDbContextFactory _factory; // creates context for database connection

        public UserWriteOnlyRepository(PlanDbContextFactory factory)
        {
            _factory = factory;
        }

        public async Task AddUserAsync(string userId, string securityStamp)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(securityStamp)) { throw new ArgumentNullException(); }

            User newUser = new()
            {
                AuthorId = userId,
                SecurityStamp = securityStamp
            };

            using var context = _factory.CreateDbContext();

            var alreadyExists = context.Users.Any(user => user.AuthorId== newUser.AuthorId);
            if (alreadyExists) {throw new InvalidOperationException();}

            try
            {
                context.AddAsync(newUser);
                context.SaveChangesAsync();
            }
            catch (DbUpdateException exception) { throw new DbUpdateException(); }


        }
    }
}
