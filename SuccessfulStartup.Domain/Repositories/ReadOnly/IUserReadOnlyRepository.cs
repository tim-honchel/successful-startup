namespace SuccessfulStartup.Domain.Repositories.ReadOnly
{
    public interface IUserReadOnlyRepository
    {
        Task<string> GetUserIdByUsernameAsync(string username);
    }
}
