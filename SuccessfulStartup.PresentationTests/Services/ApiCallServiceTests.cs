using GenFu; // for generating mock data
using Shouldly; // for assertion
using SuccessfulStartup.Api.ViewModels;
using SuccessfulStartup.Presentation.Services;
using System; // for exceptions
using System.Collections.Generic; // for List
using System.Net; // for HttpStatusCode
using System.Net.Http; // for HttpMethod
using System.Text.Json; // for JsonSerializer
using System.Threading.Tasks; // for Task

namespace SuccessfulStartup.PresentationTests.Services
{
    [TestFixture]
    internal class ApiCallServiceTests
    {
        private ApiCallService _service; // new service instance is needed for each test in order to mock behavior
        private ContextHelper _helper = new ContextHelper(); // access frequently used methods, avoid duplication

        [Test]
        public async Task DeletePlanAsync_DoesNotThrowException_GivenExistingPlanIdAndSuccessfulCall()
        {
            var planToDelete = A.New<BusinessPlanViewModel>();
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForPlans(handler, HttpMethod.Delete,HttpStatusCode.OK); // returns OkResult after delete request
            _service = new ApiCallService(handler.Object);

            Should.NotThrow( async () => await _service.DeletePlanAsync(planToDelete.Id));
        }

        [Test]
        public async Task DeletePlanAsync_ThrowsArgumentNullException_GivenInvalidPlanId()
        {
            var invalidPlan = A.New<BusinessPlanViewModel>();
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForPlans(handler, HttpMethod.Delete, HttpStatusCode.BadRequest, "null"); // returns BadRequestResult with "null" response after delete request
            _service = new ApiCallService(handler.Object);

            Should.Throw<ArgumentNullException>(async () => await _service.DeletePlanAsync(invalidPlan.Id));
        }

        [Test]
        public async Task DeletePlanAsync_ThrowsNullReferenceException_GivenNonexistentPlanId()
        {
            var nonexistentPlan = A.New<BusinessPlanViewModel>();
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForPlans(handler, HttpMethod.Delete, HttpStatusCode.NotFound, "not found"); // returns NotFoundResult with "not found" response after delete request
            _service = new ApiCallService(handler.Object);

            Should.Throw<NullReferenceException>(async () => await _service.DeletePlanAsync(nonexistentPlan.Id));
        }

        [Test]
        public async Task DeletePlanAsync_ThrowsInvalidOperationException_GivenDatabaseError()
        {
            var planCausingDatabaseError = A.New<BusinessPlanViewModel>();
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForPlans(handler, HttpMethod.Delete, HttpStatusCode.NoContent); // returns NoContentResult after delete request
            _service = new ApiCallService(handler.Object);

            Should.Throw<InvalidOperationException>(async () => await _service.DeletePlanAsync(planCausingDatabaseError.Id));
        }

        [TestCase(HttpStatusCode.RequestTimeout)]
        [TestCase(HttpStatusCode.InternalServerError)]
        [TestCase(HttpStatusCode.BadGateway)]
        [TestCase(HttpStatusCode.GatewayTimeout)]
        public async Task DeletePlanAsync_ThrowsException_GivenUnsuccessfulStatusCode(HttpStatusCode unsuccessfulCode)
        {
            var planToDelete = A.New<BusinessPlanViewModel>();
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForPlans(handler, HttpMethod.Delete, unsuccessfulCode); // returns one of many unsuccessful result types after delete request
            _service = new ApiCallService(handler.Object);

            Should.Throw<Exception>(async () => await _service.DeletePlanAsync(planToDelete.Id));
        }

        [Test]
        public async Task GetAllPlansByAuthorIdAsync_ReturnsMatchingPlans_GivenAuthorIdWithMatchingPlans()
        {
            A.Configure<BusinessPlanViewModel>().Fill(plan => plan.AuthorId, () => { return _helper.standardUser.Id; }) ;
            var plansMatchingAuthorId = A.ListOf<BusinessPlanViewModel>(5);
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForPlans(handler, HttpMethod.Get, HttpStatusCode.OK, JsonSerializer.Serialize(plansMatchingAuthorId));
            _service = new ApiCallService(handler.Object);

            var plansReturned = await _service.GetAllPlansByAuthorIdAsync(_helper.standardUser.Id);

            plansReturned.ShouldBeEquivalentTo(plansMatchingAuthorId);
        }

        [Test]
        public async Task GetAllPlansByAuthorIdAsync_ReturnsEmptyList_GivenAuthorIdWithoutMatchingPlans()
        {
            List<BusinessPlanViewModel> emptyList = new();
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForPlans(handler, HttpMethod.Get, HttpStatusCode.OK, JsonSerializer.Serialize(emptyList));
            _service = new ApiCallService(handler.Object);

            var plansReturned = await _service.GetAllPlansByAuthorIdAsync(_helper.standardUser.Id);

            plansReturned.ShouldBeEquivalentTo(emptyList);
        }

        [Test]
        public async Task GetAllPlansByAuthorIdAsync_ThrowsArgumentNullException_GivenInvalidAuthorId()
        {
            var invalidAuthorId = "";
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForPlans(handler, HttpMethod.Get, HttpStatusCode.BadRequest, "null");
            _service = new ApiCallService(handler.Object);

            Should.Throw<ArgumentNullException>(async () => await _service.GetAllPlansByAuthorIdAsync(invalidAuthorId));
        }

        [Test]
        public async Task GetAllPlansByAuthorIdAsync_ThrowsInvalidOperationException_GivenDatabaseError()
        {
            var authorIdCausingDatabaseError = "x";
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForPlans(handler, HttpMethod.Get, HttpStatusCode.NoContent);
            _service = new ApiCallService(handler.Object);

            Should.Throw<InvalidOperationException>(async () => await _service.GetAllPlansByAuthorIdAsync(authorIdCausingDatabaseError));
        }

        [TestCase(HttpStatusCode.RequestTimeout)]
        [TestCase(HttpStatusCode.InternalServerError)]
        [TestCase(HttpStatusCode.BadGateway)]
        [TestCase(HttpStatusCode.GatewayTimeout)]
        public async Task GetAllPlansByAuthorIdAsync_ThrowsException_GivenUnsuccessfulStatusCode(HttpStatusCode unsuccessfulCode)
        {
            var authorId = "x";
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForPlans(handler, HttpMethod.Get, unsuccessfulCode);
            _service = new ApiCallService(handler.Object);

            Should.Throw<Exception>(async () => await _service.GetAllPlansByAuthorIdAsync(authorId));
        }

        [Test]
        public async Task GetPlanByIdAsync_ReturnsMatchingPlan_GivenIdWithMatchingPlan()
        {
            A.Configure<BusinessPlanViewModel>().Fill(plan => plan.AuthorId, () => { return _helper.standardUser.Id; });
            var planMatchingId = A.New<BusinessPlanViewModel>();
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForPlans(handler, HttpMethod.Get, HttpStatusCode.OK, JsonSerializer.Serialize(planMatchingId));
            _service = new ApiCallService(handler.Object);

            var planReturned = await _service.GetPlanByIdAsync(planMatchingId.Id);

            planReturned.ShouldBeEquivalentTo(planMatchingId);
        }

        [Test]
        public async Task GetPlanByIdAsync_ThrowsArgumentNullException_GivenInvalidPlanId()
        {
            var invalidPlanId = 0;
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForPlans(handler, HttpMethod.Get, HttpStatusCode.BadRequest, "null");
            _service = new ApiCallService(handler.Object);

            Should.Throw<ArgumentNullException>(async () => await _service.GetPlanByIdAsync(invalidPlanId));
        }

        [Test]
        public async Task GetPlanByIdAsync_ThrowsNullReferenceException_GivenNonexistentPlanId()
        {
            var nonexistentPlanId = 999;
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForPlans(handler, HttpMethod.Get, HttpStatusCode.NotFound, "not found");
            _service = new ApiCallService(handler.Object);

            Should.Throw<NullReferenceException>(async () => await _service.GetPlanByIdAsync(nonexistentPlanId));
        }

        [TestCase(HttpStatusCode.RequestTimeout)]
        [TestCase(HttpStatusCode.InternalServerError)]
        [TestCase(HttpStatusCode.BadGateway)]
        [TestCase(HttpStatusCode.GatewayTimeout)]
        public async Task GetPlanByIdAsync_ThrowsException_GivenUnsuccessfulStatusCode(HttpStatusCode unsuccessfulCode)
        {
            var planId = 999;
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForPlans(handler, HttpMethod.Get, unsuccessfulCode);
            _service = new ApiCallService(handler.Object);

            Should.Throw<Exception>(async () => await _service.GetPlanByIdAsync(planId));
        }

        [Test]
        public async Task GetUserIdbyUsernameAsync_ReturnsMatchingUserId_GivenExistingUsername()
        {
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForUsers(handler, HttpStatusCode.OK);
            _service = new ApiCallService(handler.Object);

            var userIdReturned = await _service.GetUserIdByUsernameAsync(_helper.standardUser.UserName);

            userIdReturned.ShouldBe(_helper.standardUser.Id);
        }

        [Test]
        public async Task GetUserIdByUsernameAsync_ThrowsArgumentNullException_GivenInvalidUsername()
        {
            var invalidUsername = "";
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForUsers(handler, HttpStatusCode.BadRequest, "null");
            _service = new ApiCallService(handler.Object);

            Should.Throw<ArgumentNullException>(async () => await _service.GetUserIdByUsernameAsync(invalidUsername));
        }

        [Test]
        public async Task GetUserIdByUsernameAsync_ThrowsNullReferenceException_GivenNonexistentUsername()
        {
            var nonexistentUsername = "x";
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForUsers(handler, HttpStatusCode.NotFound, "not found");
            _service = new ApiCallService(handler.Object);

            Should.Throw<NullReferenceException>(async () => await _service.GetUserIdByUsernameAsync(nonexistentUsername));
        }

        [TestCase(HttpStatusCode.RequestTimeout)]
        [TestCase(HttpStatusCode.InternalServerError)]
        [TestCase(HttpStatusCode.BadGateway)]
        [TestCase(HttpStatusCode.GatewayTimeout)]
        public async Task GetUserIdByUsernameAsync_ThrowsException_GivenUnsuccessfulStatusCode(HttpStatusCode unsuccessfulCode)
        {
            var username = "x";
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForUsers(handler, unsuccessfulCode);
            _service = new ApiCallService(handler.Object);

            Should.Throw<Exception>(async () => await _service.GetUserIdByUsernameAsync(username));
        }

        [Test]
        public async Task SaveNewPlanAsync_DoesNotThrowException_GivenSuccessfulCreation()
        {
            var planToSave = A.New<BusinessPlanViewModel>();
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForPlans(handler, HttpMethod.Post, HttpStatusCode.Created, JsonSerializer.Serialize(planToSave.Id));
            _service = new ApiCallService(handler.Object);

            Should.NotThrow(async () => await _service.SaveNewPlanAsync(planToSave));
        }

        [Test]
        public async Task SaveNewPlanAsync_ThrowsArgumentNullException_GivenInvalidPlan()
        {
            var invalidPlan = A.New<BusinessPlanViewModel>();
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForPlans(handler, HttpMethod.Post, HttpStatusCode.BadRequest,"null");
            _service = new ApiCallService(handler.Object);

            Should.Throw<ArgumentNullException>(async () => await _service.SaveNewPlanAsync(invalidPlan));
        }

        [Test]
        public async Task SaveNewPlanAsync_ThrowsInvalidOperationException_GivenDatabaseError()
        {
            var planCausingDatabaseError = A.New<BusinessPlanViewModel>();
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForPlans(handler, HttpMethod.Post, HttpStatusCode.NoContent);
            _service = new ApiCallService(handler.Object);

            Should.Throw<InvalidOperationException>(async () => await _service.SaveNewPlanAsync(planCausingDatabaseError));
        }

        [TestCase(HttpStatusCode.RequestTimeout)]
        [TestCase(HttpStatusCode.InternalServerError)]
        [TestCase(HttpStatusCode.BadGateway)]
        [TestCase(HttpStatusCode.GatewayTimeout)]
        public async Task SaveNewPlanAsync_ThrowsException_GivenUnsuccessfulStatusCode(HttpStatusCode unsuccessfulCode)
        {
            var planToSave = A.New<BusinessPlanViewModel>();
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForPlans(handler, HttpMethod.Post, unsuccessfulCode);
            _service = new ApiCallService(handler.Object);

            Should.Throw<Exception>(async () => await _service.SaveNewPlanAsync(planToSave));
        }

        [Test]
        public async Task UpdatePlanAsync_DoesNotThrowException_GivenExistingPlan()
        {
            var planToUpdate = A.New<BusinessPlanViewModel>();
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForPlans(handler, HttpMethod.Put, HttpStatusCode.OK); 
            _service = new ApiCallService(handler.Object);

            Should.NotThrow(async () => await _service.UpdatePlanAsync(planToUpdate));
        }

        [Test]
        public async Task UpdatePlanAsync_ThrowsArgumentNullException_GivenInvalidPlan()
        {
            var invalidPlan = A.New<BusinessPlanViewModel>();
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForPlans(handler, HttpMethod.Put, HttpStatusCode.BadRequest, "null"); 
            _service = new ApiCallService(handler.Object);

            Should.Throw<ArgumentNullException>(async () => await _service.UpdatePlanAsync(invalidPlan));
        }

        [Test]
        public async Task UpdatePlanAsync_ThrowsInvalidOperationException_GivenDatabaseError()
        {
            var planCausingDatabaseError = A.New<BusinessPlanViewModel>();
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForPlans(handler, HttpMethod.Put, HttpStatusCode.NoContent, "not found");
            _service = new ApiCallService(handler.Object);

            Should.Throw<InvalidOperationException>(async () => await _service.UpdatePlanAsync(planCausingDatabaseError));
        }

        [TestCase(HttpStatusCode.RequestTimeout)]
        [TestCase(HttpStatusCode.InternalServerError)]
        [TestCase(HttpStatusCode.BadGateway)]
        [TestCase(HttpStatusCode.GatewayTimeout)]
        public async Task UpdatePlanAsync_ThrowsException_GivenUnsuccessfulStatusCode(HttpStatusCode unsuccessfulCode)
        {
            var plan = A.New<BusinessPlanViewModel>();
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForPlans(handler, HttpMethod.Put, unsuccessfulCode);
            _service = new ApiCallService(handler.Object);

            Should.Throw<Exception>(async () => await _service.UpdatePlanAsync(plan));
        }
    }
}
