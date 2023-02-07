

namespace SuccessfulStartup.Domain.Repositories.WriteOnly
{
    public interface IUserWriteOnlyRepository
    {
        Task AddUserAsync(string userId, string securityStamp);
    }
}
