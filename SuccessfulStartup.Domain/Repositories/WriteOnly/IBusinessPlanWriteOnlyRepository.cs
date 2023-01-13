using SuccessfulStartup.Domain.Entities;

namespace SuccessfulStartup.Domain.Repositories.WriteOnly
{
    public interface IBusinessPlanWriteOnlyRepository
    {
        Task SaveNewPlanAsync(BusinessPlanDomain planToSave);
    }
}
