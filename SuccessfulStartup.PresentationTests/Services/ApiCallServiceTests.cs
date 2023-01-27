using GenFu; // for generating mock data
using Shouldly; // for assertion
using SuccessfulStartup.Api.ViewModels;
using SuccessfulStartup.Presentation.Services;
using System.Text.Json; // for JsonSerializer
using System.Threading.Tasks; // for Task

namespace SuccessfulStartup.PresentationTests.Services
{
    [TestFixture]
    internal class ApiCallServiceTests
    {
        private ApiCallService _service;
        private ContextHelper _helper = new ContextHelper();

        [Test]
        public async Task DeletePlanAsync_DoesNotThrowException_GivenExistingPlanId()
        {
            var planToDelete = A.New<BusinessPlanViewModel>();
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForPlans(handler, false, true); // returns Ok after deletion request
            _service = new ApiCallService(handler.Object);

            Should.NotThrow( async () => await _service.DeletePlanAsync(planToDelete.Id));
        }

        [Test]
        public async Task GetAllPlansByAuthorIdAsync_ReturnsMatchingPlans_GivenAuthorIdWithMatchingPlans ()
        {
            A.Configure<BusinessPlanViewModel>().Fill(plan => plan.AuthorId, () => { return _helper.standardUser.Id; }) ;
            var plansMatchingAuthorId = A.ListOf<BusinessPlanViewModel>(5);
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForUsers(handler, true);
            _helper.SetupMockHandlerForPlans(handler, true, true, JsonSerializer.Serialize(plansMatchingAuthorId));
            _service = new ApiCallService(handler.Object);

            var plansReturned = await _service.GetAllPlansByAuthorIdAsync(_helper.standardUser.Id);

            plansReturned.ShouldBeEquivalentTo(plansMatchingAuthorId);
        }

        [Test]
        public async Task GetPlanByIdAsync_ReturnsMatchingPlan_GivenIdWithMatchingPlan()
        {
            A.Configure<BusinessPlanViewModel>().Fill(plan => plan.AuthorId, () => { return _helper.standardUser.Id; });
            var planMatchingId = A.New<BusinessPlanViewModel>();
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForPlans(handler, true, true, JsonSerializer.Serialize(planMatchingId));
            _service = new ApiCallService(handler.Object);

            var planReturned = await _service.GetPlanByIdAsync(planMatchingId.Id);

            planReturned.ShouldBeEquivalentTo(planMatchingId);
        }

        [Test]
        public async Task GetUserIdbyUsernameAsync_ReturnsMatchingUserId_GivenExistingUsername()
        {
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForUsers(handler, true);
            _service = new ApiCallService(handler.Object);

            var userIdReturned = await _service.GetUserIdByUsernameAsync(_helper.standardUser.UserName);

            userIdReturned.ShouldBe(_helper.standardUser.Id);
        }

        [Test]
        public async Task UpdatePlanAsync_DoesNotThrowException_GivenExistingPlan()
        {
            var planToUpdate = A.New<BusinessPlanViewModel>();
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForPlans(handler, false, true); // returns Ok after deletion request
            _service = new ApiCallService(handler.Object);

            Should.NotThrow(async () => await _service.UpdatePlanAsync(planToUpdate));
        }
    }
}
