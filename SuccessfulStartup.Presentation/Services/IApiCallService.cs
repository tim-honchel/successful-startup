using SuccessfulStartup.Api.ViewModels;

namespace SuccessfulStartup.Presentation.Services
{
    public interface IApiCallService
    {
        Task DeletePlanAsync(int planId);
        Task<List<BusinessPlanViewModel>> GetAllPlansByAuthorIdAsync(string authorId);
        Task<BusinessPlanViewModel> GetPlanByIdAsync(int planId);
        Task<int> SaveNewPlanAsync(BusinessPlanViewModel plan);
        Task UpdatePlanAsync(BusinessPlanViewModel plan);
    }
}
