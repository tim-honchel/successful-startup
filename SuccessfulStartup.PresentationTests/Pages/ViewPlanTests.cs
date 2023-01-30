using GenFu; // for generating mock data
using Shouldly; // for assertion
using SuccessfulStartup.Data.Entities;
using SuccessfulStartup.Presentation.Pages;
using System.Text.Json; // for JsonSerializer
using System.Threading.Tasks; // for Sleep

namespace SuccessfulStartup.PresentationTests.Pages
{
    internal class ViewPlanTests
    {
        private readonly ContextHelper _helper = new(); // contains helper methods for TestContext and TestAuthorizationContext

        [Test]
        public async Task DetailsContainInfoFromParameter()
        {
            var handler = _helper.GetMockHandler();
            var planToView = A.New<BusinessPlan>();
            _helper.SetupMockHandlerForPlans(handler, true, true, JsonSerializer.Serialize(planToView));
            using var testContext = _helper.GetTestContext(handler);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<ViewPlan>(parameters => parameters.Add(p => p.planId, planToView.Id));

            var name = component.Find("p[id=\"name\"]").TextContent;
            name.ShouldBe(planToView.Name);
        }

        [Test]
        public void RendersCorrectHeaderText()
        {
            var handler = _helper.GetMockHandler();
            var planToView = A.New<BusinessPlan>();
            _helper.SetupMockHandlerForPlans(handler, true, true, JsonSerializer.Serialize(planToView));
            using var testContext = _helper.GetTestContext(handler);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<ViewPlan>(parameters => parameters.Add(p => p.planId, planToView.Id)); // render the page with parameters passed in

            var header = component.Find("h1[id=\"header\"]").TextContent;
            header.ShouldBe("Your Business Plan");
        }

        [Test]
        public void RendersDetails_GivenAuthorization()
        {
            var handler = _helper.GetMockHandler();
            var planToView = A.New<BusinessPlan>();
            _helper.SetupMockHandlerForPlans(handler, true, true, JsonSerializer.Serialize(planToView));
            using var testContext = _helper.GetTestContext(handler);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<ViewPlan>(parameters => parameters.Add(p => p.planId, planToView.Id));

            var rendersDetails = component.FindAll("h6[id=\"plan\"]").Count > 0;
            rendersDetails.ShouldBeTrue();
        }

        [Test]
        public void RendersNoDetails_GivenUnauthorization()
        {
            var handler = _helper.GetMockHandler();
            var planToView = A.New<BusinessPlan>();
            _helper.SetupMockHandlerForPlans(handler, true, true, JsonSerializer.Serialize(planToView));
            using var testContext = _helper.GetTestContext(handler);
            var authorizationContext = _helper.GetAuthorizationContext(testContext, false);

            var component = testContext.RenderComponent<ViewPlan>(parameters => parameters.Add(p => p.planId, planToView.Id));

            var rendersDetails = component.FindAll("h6[id=\"plan\"]").Count > 0;
            rendersDetails.ShouldBeFalse();
        }
    }
}
