using Microsoft.AspNetCore.Mvc; // for ControllerBase, HttpGet, HttpPost, HttpPut, HttpDelete)
using SuccessfulStartup.Api.Mapping;
// using Newtonsoft.Json; // for JsonConvert, DeserializeObject
using SuccessfulStartup.Api.ViewModels;
using SuccessfulStartup.Data.Mapping;
using SuccessfulStartup.Data.Repositories.ReadOnly;
using SuccessfulStartup.Data.Repositories.WriteOnly;

namespace SuccessfulStartup.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlanController : ControllerBase // API endpoints for HTTP requests
    {
        private BusinessPlanReadOnlyRepository _repositoryForReadingBusinessPlans;
        private BusinessPlanWriteOnlyRepository _repositoryForWritingBusinessPlans;
        private ViewModelConverter _viewModelConverter;
        private EntityConverter _entityConverter;

        public PlanController(BusinessPlanReadOnlyRepository repositoryForReadingBusinessPlans, BusinessPlanWriteOnlyRepository repositoryForWritingBusinessPlans, ViewModelConverter viewModelConverter, EntityConverter entityConverter)
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
            await _repositoryForWritingBusinessPlans.UpdatePlanAsync(plan); // may need to convert to domain
            return new OkResult();
        }

        [HttpPost]
        public async Task<IActionResult> SaveNewPlan([FromBody] BusinessPlanViewModel plan)
        {
            //plan = JsonConvert.DeserializeObject<BusinessPlanDomain>(json);
            await _repositoryForWritingBusinessPlans.SaveNewPlanAsync(plan); // may need to convert to domain
            return new OkResult();
        }

    }
}