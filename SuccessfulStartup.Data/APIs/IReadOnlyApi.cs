using SuccessfulStartup.Domain.Entities;

namespace SuccessfulStartup.Data.APIs
{
    public interface IReadOnlyApi // blueprint for query-based API thatserves as intermediary between presentation layer and repositories
    {
        Task<List<BusinessPlanDomain>> GetAllPlansByAuthorId(string authorId);
        Task<string> GetUserIdByUsername(string username);
    }
}
