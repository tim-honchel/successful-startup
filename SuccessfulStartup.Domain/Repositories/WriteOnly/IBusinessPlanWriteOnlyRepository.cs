using SuccessfulStartup.Domain.Entities;

namespace SuccessfulStartup.Domain.Repositories.WriteOnly
{
    public interface IBusinessPlanWriteOnlyRepository
    {
        void SaveNewPlanAsync(BusinessPlanAbstract planToSave);
    }
}
