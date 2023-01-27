using GenFu; // for generating mock data
using Shouldly; // for assertion
using SuccessfulStartup.Api.ViewModels;
using SuccessfulStartup.Data.Authentication;
using SuccessfulStartup.Presentation.Pages;
using System.Text.Json;

namespace SuccessfulStartup.PresentationTests.Pages
{
    internal class PlansTests : Bunit.TestContext // TestContext class allows addition of service configurations
    {
        private ContextHelper _helper = new(); // contains helper methods for TestContext and TestAuthorizationContext

        [Test]
        public void RendersCorrectHeaderText()
        {
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForUsers(handler, true);
            var plansToReturn = A.ListOf<BusinessPlanViewModel>(5);
            _helper.SetupMockHandlerForPlans(handler, true, true, JsonSerializer.Serialize(plansToReturn));
            using var testContext = _helper.GetTestContext(handler);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<Plans>(); // render the page

            var header = component.Find("h1[id=\"header\"]").TextContent; // find the header text
            header.ShouldBe("Your Business Plans");
        }

        [Test]
        public void RendersNoTable_GivenUnauthorization()
        {
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForUsers(handler, true);
            using var testContext = _helper.GetTestContext(handler);
            var authorizationContext = _helper.GetAuthorizationContext(testContext, false);

            var component = testContext.RenderComponent<Plans>(); // render the page

            var rendersTable = component.FindAll("table[id=\"plans\"]").Count > 0;
            rendersTable.ShouldBeFalse();
        }

        [Test]
        public void RendersRows_GivenAuthorizationAndMatchingRecords()
        {
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForUsers(handler, true);
            A.Configure<BusinessPlanViewModel>().Fill(plan => plan.AuthorId, () => { return _helper.standardUser.Id; });
            var plansToReturn = A.ListOf<BusinessPlanViewModel>(5);
            _helper.SetupMockHandlerForPlans(handler, true, true, JsonSerializer.Serialize(plansToReturn));
            using var testContext = _helper.GetTestContext(handler);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<Plans>(); // render the page

            var rendersRows = component.FindAll("tr[id=\"plan\"]").Count > 0;
            rendersRows.ShouldBeTrue();
        }

        [Test]
        public void RendersTable_GivenAuthorization()
        {
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForUsers(handler, true);
            var plansToReturn = A.ListOf<BusinessPlanViewModel>(5);
            _helper.SetupMockHandlerForPlans(handler, true, true, JsonSerializer.Serialize(plansToReturn));
            using var testContext = _helper.GetTestContext(handler);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<Plans>(); // render the page

            var rendersTable = component.FindAll("table[id=\"plans\"]").Count > 0;
            rendersTable.ShouldBeTrue();
        }
    }
}
