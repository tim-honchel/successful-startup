using GenFu; // for generating mock data
using Microsoft.AspNetCore.Mvc; // for OkResult, OkObjectResult
using Moq; // for Mock, Setup
using Microsoft.EntityFrameworkCore; // for DbContextOptionsBuilder, DbUpdateException
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
        private ApiHelper _helper = new();
        private Mock<IBusinessPlanReadOnlyRepository> _mockReadRepository = new();
        private Mock<IBusinessPlanWriteOnlyRepository> _mockWriteRepository = new();
        private ViewModelConverter _viewModelConverter = new(AllViewModelsMappingProfiles.GetMapper());
        private EntityConverter _entityConverter = new(AllMappingProfiles.GetMapper());
        private PlanController _controller; // needs to be initialized for each test in order to mock behavior

        [Test]
        public async Task DeletePlan_Returns200Code_GivenExistingPlanId()
        {
            var planToDelete = A.New<BusinessPlanDomain>();
            _mockReadRepository.Setup(repository => repository.GetPlanByIdAsync(planToDelete.Id)).ReturnsAsync(planToDelete);
            _controller = new PlanController(_mockReadRepository.Object, _mockWriteRepository.Object, _viewModelConverter, _entityConverter);

            var response = await _controller.DeletePlan(planToDelete.Id);
            response.ShouldBeOfType<OkResult>();
        }

        [Test]
        public async Task DeletePlan_Returns204Code_GivenNonexistentPlanId()
        {
            var nonexistentId = 999;
            _mockReadRepository.Setup(repository => repository.GetPlanByIdAsync(nonexistentId)).Throws<NullReferenceException>();
            _controller = new PlanController(_mockReadRepository.Object, _mockWriteRepository.Object, _viewModelConverter, _entityConverter);

            var response = await _controller.DeletePlan(nonexistentId);
            response.ShouldBeOfType<NoContentResult>();
        }

        [Test]
        public async Task DeletePlan_Returns204Code_GivenDatabaseError()
        {
            var previouslyDeletedId = 999;
            _mockReadRepository.Setup(repository => repository.GetPlanByIdAsync(previouslyDeletedId)).Throws<DbUpdateException>();
            _controller = new PlanController(_mockReadRepository.Object, _mockWriteRepository.Object, _viewModelConverter, _entityConverter);

            var response = await _controller.DeletePlan(previouslyDeletedId);
            response.ShouldBeOfType<NoContentResult>();
        }

        [Test]
        public async Task DeletePlan_Returns404CodeWithObject_GivenInvalidPlanId()
        {
            var invalidId = 0;
            _mockReadRepository.Setup(repository => repository.GetPlanByIdAsync(invalidId)).Throws<ArgumentNullException>();
            _controller = new PlanController(_mockReadRepository.Object, _mockWriteRepository.Object, _viewModelConverter, _entityConverter);

            var response = await _controller.DeletePlan(invalidId);
            response.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task GetAllPlansByAuthorId_ReturnsEmptyList_GivenAuthorIdWithNoMatchingPlans()
        {
            List<BusinessPlanDomain> emptyList = new();
            _mockReadRepository.Setup(repository => repository.GetAllPlansByAuthorIdAsync(_helper.standardUser.Id)).ReturnsAsync(emptyList);
            _controller = new PlanController(_mockReadRepository.Object, _mockWriteRepository.Object, _viewModelConverter, _entityConverter);

            var response = await _controller.GetAllPlansByAuthorId(_helper.standardUser.Id);
            var result = (OkObjectResult)response;

            var returnedPlans = result.Value;
            returnedPlans.ShouldBeEquivalentTo(emptyList);
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
        public async Task GetAllPlansByAuthorId_Returns200CodeWithObject_GivenAuthorIdWithMatchingPlans()
        {
            A.Configure<BusinessPlanDomain>().Fill(plan => plan.AuthorId, () => { return _helper.standardUser.Id; });
            var plansByUser = A.ListOf<BusinessPlanDomain>(5);
            _mockReadRepository.Setup(repository => repository.GetAllPlansByAuthorIdAsync(_helper.standardUser.Id)).ReturnsAsync(plansByUser);
            _controller = new PlanController(_mockReadRepository.Object, _mockWriteRepository.Object, _viewModelConverter, _entityConverter);

            var response = await _controller.GetAllPlansByAuthorId(_helper.standardUser.Id);
            response.ShouldBeOfType<OkObjectResult>();
        }

        [Test]
        public async Task GetAllPlansByAuthorId_Returns40CodeWithObject_GivenInvalidAuthorId()
        {
            string invalidId = null;
            _mockReadRepository.Setup(repository => repository.GetAllPlansByAuthorIdAsync(invalidId)).Throws<ArgumentNullException>();
            _controller = new PlanController(_mockReadRepository.Object, _mockWriteRepository.Object, _viewModelConverter, _entityConverter);

            var response = await _controller.GetAllPlansByAuthorId(invalidId);
            response.ShouldBeOfType<BadRequestObjectResult>();
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
        public async Task GetPlanById_Returns200CodeWithObject_GivenIdWithMatchingPlan()
        {
            var planMatchingId = A.New<BusinessPlanDomain>();
            _mockReadRepository.Setup(repository => repository.GetPlanByIdAsync(planMatchingId.Id)).ReturnsAsync(planMatchingId);
            _controller = new PlanController(_mockReadRepository.Object, _mockWriteRepository.Object, _viewModelConverter, _entityConverter);

            var response = await _controller.GetPlanById(planMatchingId.Id);
            response.ShouldBeOfType<OkObjectResult>();
        }

        [Test]
        public async Task GetPlanById_Returns204Code_GivenIdWithNonexistentPlan()
        {
            var nonexistentId = 999;
            _mockReadRepository.Setup(repository => repository.GetPlanByIdAsync(nonexistentId)).Throws<NullReferenceException>();
            _controller = new PlanController(_mockReadRepository.Object, _mockWriteRepository.Object, _viewModelConverter, _entityConverter);

            var response = await _controller.GetPlanById(nonexistentId);
            response.ShouldBeOfType<NoContentResult>();
        }

        [Test]
        public async Task GetPlanById_Returns400CodeWithObject_GivenInvalidPlanId()
        {
            var invalidId = 0;
            _mockReadRepository.Setup(repository => repository.GetPlanByIdAsync(invalidId)).Throws<ArgumentNullException>();
            _controller = new PlanController(_mockReadRepository.Object, _mockWriteRepository.Object, _viewModelConverter, _entityConverter);

            var response = await _controller.GetPlanById(invalidId);
            response.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task UpdatePlan_Returns200Code_GivenValidPlan()
        {
            var planToUpdate = A.New<BusinessPlanViewModel>();
            _controller = new PlanController(_mockReadRepository.Object, _mockWriteRepository.Object, _viewModelConverter, _entityConverter);

            var response = await _controller.UpdatePlan(planToUpdate);
            response.ShouldBeOfType<OkResult>();
        }

        [Test]
        public async Task UpdatePlan_Returns204Code_GivenDatabaseError()
        {
            var planToUpdate = A.New<BusinessPlanViewModel>();
            _mockWriteRepository.Setup(repository => repository.UpdatePlanAsync(It.Is<BusinessPlanDomain>(plan => plan.Id == planToUpdate.Id))).Throws<DbUpdateException>();
            _controller = new PlanController(_mockReadRepository.Object, _mockWriteRepository.Object, _viewModelConverter, _entityConverter);

            var response = await _controller.UpdatePlan(planToUpdate);
            response.ShouldBeOfType<NoContentResult>();
        }

        [Test]
        public async Task SaveNewPlan_Returns201Code_GivenValidPlan()
        {
            var planToSave = A.New<BusinessPlanViewModel>();
            _controller = new PlanController(_mockReadRepository.Object, _mockWriteRepository.Object, _viewModelConverter, _entityConverter);

            var response = await _controller.SaveNewPlan(planToSave);
            response.ShouldBeOfType<CreatedResult>();
        }

        [Test]
        public async Task SaveNewPlan_Returns204Code_GivenDatabaseError()
        {
            var planToSave = A.New<BusinessPlanViewModel>();
            _mockWriteRepository.Setup(repository => repository.SaveNewPlanAsync(It.Is<BusinessPlanDomain>(plan => plan.Id == planToSave.Id))).Throws<DbUpdateException>();
            _controller = new PlanController(_mockReadRepository.Object, _mockWriteRepository.Object, _viewModelConverter, _entityConverter);

            var response = await _controller.SaveNewPlan(planToSave);
            response.ShouldBeOfType<NoContentResult>();
        }

        [Test]
        public async Task SaveNewPlan_Returns400CodeWithObject_GivenInvalidInput()
        {
            var invalidPlan = A.New<BusinessPlanViewModel>();
            invalidPlan.Name = null;
            _mockWriteRepository.Setup(repository => repository.SaveNewPlanAsync(It.Is<BusinessPlanDomain>(plan => plan.Id == invalidPlan.Id))).Throws<ArgumentNullException>();
            _controller = new PlanController(_mockReadRepository.Object, _mockWriteRepository.Object, _viewModelConverter, _entityConverter);

            var response = await _controller.SaveNewPlan(invalidPlan);
            response.ShouldBeOfType<BadRequestObjectResult>();
        }
    }


}
