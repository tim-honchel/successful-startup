using Microsoft.AspNetCore.Mvc; // for ControllerBase, HttpGet, HttpPost, HttpPut, HttpDelete)
using Microsoft.EntityFrameworkCore; // for DbUpdateException
using SuccessfulStartup.Api.Mapping;
using SuccessfulStartup.Api.ViewModels;
using SuccessfulStartup.Data.Mapping;
using SuccessfulStartup.Domain.Repositories.ReadOnly;
using SuccessfulStartup.Domain.Repositories.WriteOnly;
using System.Net.Http.Headers;

namespace SuccessfulStartup.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlanController : ControllerBase // API controller with endpoints for HTTP requests
    {
        private readonly IBusinessPlanReadOnlyRepository _repositoryForReadingBusinessPlans;
        private readonly IBusinessPlanWriteOnlyRepository _repositoryForWritingBusinessPlans;
        private readonly IUserReadOnlyRepository _repositoryForReadingUsers;
        private readonly ViewModelConverter _viewModelConverter;
        private readonly  EntityConverter _entityConverter;

        public PlanController(IBusinessPlanReadOnlyRepository repositoryForReadingBusinessPlans, IBusinessPlanWriteOnlyRepository repositoryForWritingBusinessPlans, IUserReadOnlyRepository repositoryForReadingUsers, ViewModelConverter viewModelConverter, EntityConverter entityConverter)
        {
            _repositoryForReadingBusinessPlans = repositoryForReadingBusinessPlans;
            _repositoryForWritingBusinessPlans = repositoryForWritingBusinessPlans;
            _repositoryForReadingUsers = repositoryForReadingUsers;
            _viewModelConverter = viewModelConverter;
            _entityConverter = entityConverter;
        }

        [HttpDelete("{planId}")]
        public async Task<IActionResult> DeletePlan(int planId) // needs to be public in order to be accessible to Swagger
        {
            string securityStamp;
            try
            {
                securityStamp = Request.Headers["key"];
            }
            catch (Exception)
            {
                return new ForbidResult();
            }

            var verified = await _repositoryForReadingUsers.VerifyUserAsync(planId, securityStamp);
            if (!verified)
            {
                return new UnauthorizedResult();
            }

            try
            {
                var planToDelete = await _repositoryForReadingBusinessPlans.GetPlanByIdAsync(planId);
                await _repositoryForWritingBusinessPlans.DeletePlanAsync(planToDelete);
                return new OkResult();
            }
            catch (ArgumentNullException)
            {
                return new BadRequestObjectResult("null");
            }
            catch (NullReferenceException)
            {
                return new NotFoundObjectResult("not found");
            }
            catch (DbUpdateException)
            {
                return new NoContentResult();
            }

        }

        [HttpGet("all/{authorId}")]
        public async Task<IActionResult> GetAllPlansByAuthorId(string authorId)
        {
            string securityStamp;
            try
            {
                securityStamp = Request.Headers["key"];
            }
            catch (Exception)
            {
                return new ForbidResult();
            }

            var verified = await _repositoryForReadingUsers.VerifyUserAsync(authorId, securityStamp);
            if (!verified)
            {
                return new UnauthorizedResult();
            }

            try
            {
                var plans = await _repositoryForReadingBusinessPlans.GetAllPlansByAuthorIdAsync(authorId);
                return new OkObjectResult(plans); // not necessary to convert to ViewModel because JSON format is the same
            }
            catch (ArgumentNullException exception)
            {
                return new BadRequestObjectResult(exception.ParamName);
            }
        }

        [HttpGet("{planId}")]
        public async Task<IActionResult> GetPlanById(int planId)
        {
            string securityStamp;
            try
            {
                securityStamp = Request.Headers["key"];
            }
            catch (Exception)
            {
                return new ForbidResult();
            }

            var verified = await _repositoryForReadingUsers.VerifyUserAsync(planId, securityStamp);
            if (!verified)
            {
                return new UnauthorizedResult();
            }

            try
            {
                var plan = await _repositoryForReadingBusinessPlans.GetPlanByIdAsync(planId);
                return new OkObjectResult(plan); // not necessary to convert to ViewModel because JSON format is the same
            }
            catch (ArgumentNullException exception)
            {
                return new BadRequestObjectResult(exception.ParamName);
            }
            catch (NullReferenceException)
            {
                return new NoContentResult();
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveNewPlan(BusinessPlanViewModel plan)
        {
            string securityStamp;
            try
            {
                securityStamp = Request.Headers["key"];
            }
            catch (Exception)
            {
                return new ForbidResult();
            }

            var verified = await _repositoryForReadingUsers.VerifyUserAsync(plan.AuthorId, securityStamp);
            if (!verified)
            {
                return new UnauthorizedResult();
            }

            try
            {
                var planToSave = _entityConverter.Convert(_viewModelConverter.Convert(plan));
                var newId = await _repositoryForWritingBusinessPlans.SaveNewPlanAsync(planToSave);
                return new CreatedResult(new Uri($"/plans/{newId}", UriKind.Relative), newId); // shows new record was created, points to URL where it can be found
            }
            catch (ArgumentNullException exception)
            {
                return new BadRequestObjectResult(exception.ParamName);
            }
            catch (DbUpdateException)
            {
                return new NoContentResult();
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePlan(BusinessPlanViewModel plan)
        {
            string securityStamp;
            try
            {
                securityStamp = Request.Headers["key"];
            }
            catch (Exception)
            {
                return new ForbidResult();
            }

            var verified = await _repositoryForReadingUsers.VerifyUserAsync(plan.AuthorId, securityStamp);
            if (!verified)
            {
                return new UnauthorizedResult();
            }

            try
            {
                var planToUpdate = _entityConverter.Convert(_viewModelConverter.Convert(plan));
                await _repositoryForWritingBusinessPlans.UpdatePlanAsync(planToUpdate);
                return new OkResult();
            }
            catch (DbUpdateException)
            {
                return new NoContentResult();
            }
        }



    }
}