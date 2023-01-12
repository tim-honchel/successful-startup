

using SuccessfulStartup.Domain.Entities;

namespace SuccessfulStartup.Data.APIs
{
    public interface IWriteOnlyApi
    {
        void SaveNewPlan(BusinessPlanAbstract planToSave);
    }
}
