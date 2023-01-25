using Microsoft.AspNetCore.Mvc; // for ControllerBase, HttpGet, HttpPost, HttpPut, HttpDelete)
// using Newtonsoft.Json; // for JsonConvert, DeserializeObject
using SuccessfulStartup.Api.ViewModels;
using SuccessfulStartup.Data.Mapping;
using SuccessfulStartup.Data.Repositories.ReadOnly;
using SuccessfulStartup.Data.Repositories.WriteOnly;
using SuccessfulStartup.Domain.Entities;

namespace SuccessfulStartup.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlanController : ControllerBase // API endpoints for HTTP requests
    {
        private BusinessPlanReadOnlyRepository _repositoryForReadingBusinessPlans;
        private BusinessPlanWriteOnlyRepository _repositoryForWritingBusinessPlans;
        private EntityConverter _entityConverter;

        public PlanController(BusinessPlanReadOnlyRepository repositoryForReadingBusinessPlans, BusinessPlanWriteOnlyRepository repositoryForWritingBusinessPlans, EntityConverter entityConverter)
        {
            _repositoryForReadingBusinessPlans = repositoryForReadingBusinessPlans;
            _repositoryForWritingBusinessPlans = repositoryForWritingBusinessPlans;
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
            return new OkObjectResult(_entityConverter.Convert(plans));
        }

        [HttpGet("{planId}")]
        public async Task<IActionResult> GetPlanById(int planId)
        {
            var plan = await _repositoryForReadingBusinessPlans.GetPlanByIdAsync(planId);
            return new OkObjectResult(plan);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePlan([FromBody] BusinessPlanViewModel plan)
        {
            await _repositoryForWritingBusinessPlans.UpdatePlanAsync(plan);
            return new OkResult();
        }

        [HttpPost]
        public async Task<IActionResult> SaveNewPlan([FromBody] BusinessPlanViewModel plan)
        {
            //plan = JsonConvert.DeserializeObject<BusinessPlanDomain>(json);
            var planToSave = new BusinessPlanDomain() { Id = plan.Id, Name = plan.Name, Description = plan.Description, AuthorId = plan.AuthorId };
            await _repositoryForWritingBusinessPlans.SaveNewPlanAsync(planToSave);
            return new OkResult();
        }

    }
}