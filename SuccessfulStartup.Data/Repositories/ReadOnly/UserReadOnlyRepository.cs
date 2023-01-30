using Microsoft.EntityFrameworkCore; // for SingleOrDefaultAsync
using SuccessfulStartup.Data.Contexts;
using SuccessfulStartup.Domain.Repositories.ReadOnly;

namespace SuccessfulStartup.Data.Repositories.ReadOnly
{
    public class UserReadOnlyRepository : IUserReadOnlyRepository // 
    {
        private readonly AuthenticationDbContextFactory _factory; // creates context for database connection TODO: is it possible to use interface instead
        public UserReadOnlyRepository(AuthenticationDbContextFactory factory)
        {
            _factory = factory;
        }

        public async Task<string> GetUserIdByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) { throw new ArgumentNullException(nameof(username)); }

            using var context = _factory.CreateDbContext();

                var authorId = await context.Users.Where(user => user.UserName == username).Select(user => user.Id).SingleOrDefaultAsync();

                if (authorId == null) { throw new NullReferenceException(nameof(username)); } // if no user with that name is found
                return authorId;
        }
    }
}
