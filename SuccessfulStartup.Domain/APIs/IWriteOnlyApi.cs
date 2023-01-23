using SuccessfulStartup.Domain.Entities;

namespace SuccessfulStartup.Domain.APIs
{
    public interface IWriteOnlyApi // blueprint for command-based API that serves as intermediary between presentation layer and repositories
    {
        Task DeletePlan(BusinessPlanDomain planToDelete);
        Task UpdatePlan(BusinessPlanDomain planToUpdate);
        Task SaveNewPlan(BusinessPlanDomain planToSave);
    }
}
