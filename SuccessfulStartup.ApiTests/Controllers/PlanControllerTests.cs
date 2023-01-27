using GenFu; // for generating mock data
using Microsoft.AspNetCore.Mvc; // for OkResult, OkObjectResult
using Moq; // for Mock, Setup
using Shouldly; // for assertion
using SuccessfulStartup.Api.Controllers;
using SuccessfulStartup.Api.Mapping;
using SuccessfulStartup.Api.ViewModels;
using SuccessfulStartup.Data.Mapping;
using SuccessfulStartup.Domain.Entities;
using SuccessfulStartup.Domain.Repositories.ReadOnly;
using SuccessfulStartup.Domain.Repositories.WriteOnly;

namespace SuccessfulStartup.ApiTests.Controllers
{
    [TestFixture]
    internal class PlanControllerTests
    {
        private ApiHelper _helper = new ApiHelper();
        private Mock<IBusinessPlanReadOnlyRepository> _mockReadRepository = new Mock<IBusinessPlanReadOnlyRepository>();
        private Mock<IBusinessPlanWriteOnlyRepository> _mockWriteRepository = new Mock<IBusinessPlanWriteOnlyRepository>();
        private ViewModelConverter _viewModelConverter = new ViewModelConverter(AllViewModelsMappingProfiles.GetMapper());
        private EntityConverter _entityConverter = new EntityConverter(AllMappingProfiles.GetMapper());
        private PlanController _controller;

        [Test]
        public async Task DeletePlan_ReturnsOkResult_GivenExistingPlanId()
        {
            var planToDelete = A.New<BusinessPlanDomain>();
            _mockReadRepository.Setup(repository => repository.GetPlanByIdAsync(planToDelete.Id)).ReturnsAsync(planToDelete);
            _controller = new PlanController(_mockReadRepository.Object, _mockWriteRepository.Object, _viewModelConverter, _entityConverter);

            var response = await _controller.DeletePlan(planToDelete.Id);
            var result = (OkResult)response;

            var statusCode = result.StatusCode;
            statusCode.ShouldBe(200);
        }

        [Test]
        public async Task GetAllPlansByAuthorId_ReturnsPlans_GivenAuthorIdWithMatchingPlans()
        {
            A.Configure<BusinessPlanDomain>().Fill(plan => plan.AuthorId, () => { return _helper.standardUser.Id; });
            var plansByUser = A.ListOf<BusinessPlanDomain>(5);
            _mockReadRepository.Setup(repository => repository.GetAllPlansByAuthorIdAsync(_helper.standardUser.Id)).ReturnsAsync(plansByUser);
            _controller = new PlanController(_mockReadRepository.Object, _mockWriteRepository.Object, _viewModelConverter, _entityConverter);

            var response = await _controller.GetAllPlansByAuthorId(_helper.standardUser.Id);
            var result = (OkObjectResult)response;

            var returnedPlans = result.Value;
            returnedPlans.ShouldBeEquivalentTo(plansByUser);
        }

        [Test]
        public async Task GetPlanById_ReturnsPlan_GivenIdWithMatchingPlan()
        {
            var planMatchingId = A.New<BusinessPlanDomain>();
            _mockReadRepository.Setup(repository => repository.GetPlanByIdAsync(planMatchingId.Id)).ReturnsAsync(planMatchingId);
            _controller = new PlanController(_mockReadRepository.Object, _mockWriteRepository.Object, _viewModelConverter, _entityConverter);

            var response = await _controller.GetPlanById(planMatchingId.Id);
            var result = (OkObjectResult)response;

            var returnedPlan = result.Value;
            returnedPlan.ShouldBeEquivalentTo(planMatchingId);
        }

        [Test]
        public async Task UpdatePlan_ReturnsOkResult_GivenValidPlan()
        {
            var planToUpdate = A.New<BusinessPlanViewModel>();
            _controller = new PlanController(_mockReadRepository.Object, _mockWriteRepository.Object, _viewModelConverter, _entityConverter);

            var response = await _controller.UpdatePlan(planToUpdate);
            var result = (OkResult)response;

            var statusCode = result.StatusCode;
            statusCode.ShouldBe(200);
        }

        [Test]
        public async Task SaveNewPlan()
        {
            var planToSave = A.New<BusinessPlanViewModel>();
            _controller = new PlanController(_mockReadRepository.Object, _mockWriteRepository.Object, _viewModelConverter, _entityConverter);

            var response = await _controller.SaveNewPlan(planToSave);
            var result = (OkResult)response;

            var statusCode = result.StatusCode;
            statusCode.ShouldBe(200);
        }
    }


}
