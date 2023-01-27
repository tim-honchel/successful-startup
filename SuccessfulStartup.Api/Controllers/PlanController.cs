using Microsoft.AspNetCore.Mvc; // for ControllerBase, HttpGet, HttpPost, HttpPut, HttpDelete)
using SuccessfulStartup.Api.Mapping;
using SuccessfulStartup.Api.ViewModels;
using SuccessfulStartup.Data.Mapping;
using SuccessfulStartup.Domain.Repositories.ReadOnly;
using SuccessfulStartup.Domain.Repositories.WriteOnly;

namespace SuccessfulStartup.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlanController : ControllerBase // API endpoints for HTTP requests
    {
        private IBusinessPlanReadOnlyRepository _repositoryForReadingBusinessPlans;
        private IBusinessPlanWriteOnlyRepository _repositoryForWritingBusinessPlans;
        private ViewModelConverter _viewModelConverter;
        private EntityConverter _entityConverter;

        public PlanController(IBusinessPlanReadOnlyRepository repositoryForReadingBusinessPlans, IBusinessPlanWriteOnlyRepository repositoryForWritingBusinessPlans, ViewModelConverter viewModelConverter, EntityConverter entityConverter)
        {
            _repositoryForReadingBusinessPlans = repositoryForReadingBusinessPlans;
            _repositoryForWritingBusinessPlans = repositoryForWritingBusinessPlans;
            _viewModelConverter = viewModelConverter;
            _entityConverter= entityConverter;
        }

        [HttpDelete("{planId}")]
        public async Task<IActionResult> DeletePlan(int planId)
        {
            var planToDelete = await _repositoryForReadingBusinessPlans.GetPlanByIdAsync(planId);
            await _repositoryForWritingBusinessPlans.DeletePlanAsync(planToDelete);
            return new OkResult();
        }

        [HttpGet("all/{authorId}")]
        public async Task<IActionResult> GetAllPlansByAuthorId(string authorId)
        {
            var plans = await _repositoryForReadingBusinessPlans.GetAllPlansByAuthorIdAsync(authorId);
            return new OkObjectResult(plans); // not necessary to convert to ViewModel because JSON format is the same
        }

        [HttpGet("{planId}")]
        public async Task<IActionResult> GetPlanById(int planId)
        {
            var plan = await _repositoryForReadingBusinessPlans.GetPlanByIdAsync(planId);
            return new OkObjectResult(plan); // not necessary to convert to ViewModel because JSON format is the same
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePlan([FromBody] BusinessPlanViewModel plan)
        {
            var planToUpdate = _entityConverter.Convert(_viewModelConverter.Convert(plan));
            await _repositoryForWritingBusinessPlans.UpdatePlanAsync(planToUpdate);
            return new OkResult();
        }

        [HttpPost]
        public async Task<IActionResult> SaveNewPlan(BusinessPlanViewModel plan)
        {
            var planToSave = _entityConverter.Convert(_viewModelConverter.Convert(plan));
            await _repositoryForWritingBusinessPlans.SaveNewPlanAsync(planToSave);
            return new OkResult();
        }

    }
}