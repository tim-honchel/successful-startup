using SuccessfulStartup.Domain.Entities;

namespace SuccessfulStartup.Data.APIs
{
    public interface IWriteOnlyApi // blueprint for command-based API that serves as intermediary between presentation layer and repositories
    {
        Task SaveNewPlan(BusinessPlanDomain planToSave);
    }
}
