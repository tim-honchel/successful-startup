using SuccessfulStartup.Domain.Entities;

namespace SuccessfulStartup.Domain.Repositories.WriteOnly
{
    public interface IBusinessPlanWriteOnlyRepository
    {
        Task UpdatePlanAsync(BusinessPlanDomain planToUpdate);
        Task SaveNewPlanAsync(BusinessPlanDomain planToSave);
    }
}
