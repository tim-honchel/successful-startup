using SuccessfulStartup.Domain.Entities;

namespace SuccessfulStartup.Domain.Repositories.ReadOnly
{
    public interface IBusinessPlanReadOnlyRepository
    {
        Task<List<BusinessPlanDomain>> GetAllPlansByAuthorIdAsync(string authorId);
        Task<BusinessPlanDomain> GetPlanByIdAsync(int id);
        
    }
}
