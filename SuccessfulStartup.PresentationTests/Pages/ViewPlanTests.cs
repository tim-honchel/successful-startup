using Shouldly; // for assertion
using SuccessfulStartup.Data.APIs;
using SuccessfulStartup.Data.Mapping;
using SuccessfulStartup.Presentation.Pages;
using System.Threading.Tasks; // for Sleep

namespace SuccessfulStartup.PresentationTests.Pages
{
    internal class ViewPlanTests
    {
        private ContextHelper _helper; // contains helper methods for TestContext and TestAuthorizationContext
        private ReadOnlyApi _api;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _helper = new ContextHelper();
            _api = new ReadOnlyApi(new Data.Contexts.AuthenticationDbContextFactory(), AllMappingProfiles.GetMapper());
        }

        [Test]
        public void RendersCorrectHeaderText()
        {
            using var testContext = _helper.GetTestContext();
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<ViewPlan>(parameters => parameters.Add(p => p.planId, 24)); // render the page with parameters passed in

            var header = component.Find("h1").TextContent;
            header.ShouldBe("Your Business Plan");
        }

        [Test]
        public void RendersDetails_GivenAuthorization()
        {
            using var testContext = _helper.GetTestContext();
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<ViewPlan>(parameters => parameters.Add(p => p.planId, 24));

            var rendersDetails = component.FindAll("h6").Count > 0;
            rendersDetails.ShouldBeTrue();
        }

        [Test]
        public void RendersNoDetails_GivenUnauthorization()
        {
            using var testContext = _helper.GetTestContext();
            var authorizationContext = _helper.GetAuthorizationContext(testContext, "email@gmail.com", false);

            var component = testContext.RenderComponent<ViewPlan>(parameters => parameters.Add(p => p.planId, 24));

            var rendersDetails = component.FindAll("h6").Count > 0;
            rendersDetails.ShouldBeFalse();
        }

        [Test]
        public async Task DetailsContainInfoFromParameter()
        {
            using var testContext = _helper.GetTestContext();
            var authorizationContext = _helper.GetAuthorizationContext(testContext);
            var existingPlanId = 24;

            var component = testContext.RenderComponent<ViewPlan>(parameters => parameters.Add(p => p.planId, existingPlanId));

            var fetchedPlan = await _api.GetPlanById(existingPlanId);
            System.Threading.Thread.Sleep(50); // time for component to fully render
            var name = component.Find("p[id=\"Name\"]").TextContent;
            name.ShouldBe(fetchedPlan.Name);
        }
    }
}
