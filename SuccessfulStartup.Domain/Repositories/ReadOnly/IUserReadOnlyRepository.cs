namespace SuccessfulStartup.Domain.Repositories.ReadOnly
{
    public interface IUserReadOnlyRepository
    {
        public Task<bool> VerifyUserAsync(string userId, string securityStamp);

        public Task<bool> VerifyUserAsync(int planId, string securityStamp);
    }
}
