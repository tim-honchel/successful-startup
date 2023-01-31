using SuccessfulStartup.Domain.Entities;

namespace SuccessfulStartup.Domain.Repositories.WriteOnly
{
    public interface IBusinessPlanWriteOnlyRepository
    {
        Task DeletePlanAsync(BusinessPlanDomain planToDelete);
        Task UpdatePlanAsync(BusinessPlanDomain planToUpdate);
        Task<int> SaveNewPlanAsync(BusinessPlanDomain planToSave);
    }
}
