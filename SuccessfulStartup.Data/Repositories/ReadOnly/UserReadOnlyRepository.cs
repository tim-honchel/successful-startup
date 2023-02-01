using Microsoft.EntityFrameworkCore; // for SingleOrDefaultAsync
using Microsoft.EntityFrameworkCore.Design;
using SuccessfulStartup.Data.Contexts;
using SuccessfulStartup.Domain.Repositories.ReadOnly;

namespace SuccessfulStartup.Data.Repositories.ReadOnly
{
    public class UserReadOnlyRepository : IUserReadOnlyRepository // 
    {
        private readonly IDesignTimeDbContextFactory<AuthenticationDbContext> _factory; // creates context for database connection TODO: is it possible to use interface instead
        public UserReadOnlyRepository(IDesignTimeDbContextFactory<AuthenticationDbContext> factory)
        {
            _factory = factory;
        }

        public async Task<string> GetUserIdByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) { throw new ArgumentNullException(nameof(username)); }

            using var context = _factory.CreateDbContext(new string[] { });

                var authorId = await context.Users.Where(user => user.UserName == username).Select(user => user.Id).SingleOrDefaultAsync();

                if (authorId == null) { throw new NullReferenceException(nameof(username)); } // if no user with that name is found
                return authorId;
        }
    }
}
