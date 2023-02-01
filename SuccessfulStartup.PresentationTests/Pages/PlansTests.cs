using GenFu; // for generating mock data
using Moq; // for Setup
using Shouldly; // for assertion
using SuccessfulStartup.Api.ViewModels;
using SuccessfulStartup.Presentation.Pages;

namespace SuccessfulStartup.PresentationTests.Pages
{
    internal class PlansTests : Bunit.TestContext // TestContext class allows addition of service configurations
    {
        private readonly ContextHelper _helper = new(); // contains helper methods for TestContext and TestAuthorizationContext

        [Test]
        public void RendersCorrectHeaderText()
        {
            var service = _helper.GetMockApiService(); // mock API call service
            service.Setup(service => service.GetUserIdByUsernameAsync(_helper.standardUser.UserName)).ReturnsAsync(_helper.standardUser.Id); // returns standard Id
            var plansToReturn = A.ListOf<BusinessPlanViewModel>(5);
            service.Setup(service => service.GetAllPlansByAuthorIdAsync(_helper.standardUser.Id)).ReturnsAsync(plansToReturn); // returns generated plans
            using var testContext = _helper.GetTestContext(service);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<Plans>(); // render the page

            var header = component.Find("h1[id=\"header\"]").TextContent; // find the header text
            header.ShouldBe("Your Business Plans");
        }

        [Test]
        public void RendersNoTable_GivenUnauthorization()
        {
            var service = _helper.GetMockApiService();
            service.Setup(service => service.GetUserIdByUsernameAsync(_helper.standardUser.UserName)).ReturnsAsync(_helper.standardUser.Id);
            using var testContext = _helper.GetTestContext(service);
            var authorizationContext = _helper.GetAuthorizationContext(testContext, false);

            var component = testContext.RenderComponent<Plans>(); // render the page

            var rendersTable = component.FindAll("table[id=\"plans\"]").Count > 0;
            rendersTable.ShouldBeFalse();
        }

        [Test]
        public void RendersRows_GivenAuthorizationAndMatchingRecords()
        {
            var service = _helper.GetMockApiService();
            service.Setup(service => service.GetUserIdByUsernameAsync(_helper.standardUser.UserName)).ReturnsAsync(_helper.standardUser.Id);
            A.Configure<BusinessPlanViewModel>().Fill(plan => plan.AuthorId, () => { return _helper.standardUser.Id; });
            var plansToReturn = A.ListOf<BusinessPlanViewModel>(5);
            service.Setup(service => service.GetAllPlansByAuthorIdAsync(_helper.standardUser.Id)).ReturnsAsync(plansToReturn);
            using var testContext = _helper.GetTestContext(service);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<Plans>(); // render the page

            var rendersRows = component.FindAll("tr[id=\"plan\"]").Count > 0;
            rendersRows.ShouldBeTrue();
        }

        [Test]
        public void RendersTable_GivenAuthorization()
        {
            var service = _helper.GetMockApiService();
            service.Setup(service => service.GetUserIdByUsernameAsync(_helper.standardUser.UserName)).ReturnsAsync(_helper.standardUser.Id);
            var plansToReturn = A.ListOf<BusinessPlanViewModel>(5);
            service.Setup(service => service.GetAllPlansByAuthorIdAsync(_helper.standardUser.Id)).ReturnsAsync(plansToReturn);
            using var testContext = _helper.GetTestContext(service);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<Plans>(); // render the page

            var rendersTable = component.FindAll("table[id=\"plans\"]").Count > 0;
            rendersTable.ShouldBeTrue();
        }
    }
}
