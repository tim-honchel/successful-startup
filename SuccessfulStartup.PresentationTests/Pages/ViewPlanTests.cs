using Microsoft.CodeAnalysis.Differencing;
using Shouldly; // for assertion
using SuccessfulStartup.Presentation.Pages;

namespace SuccessfulStartup.PresentationTests.Pages
{
    internal class ViewPlanTests
    {
        private ContextHelper _helper = new ContextHelper(); // contains helper methods for TestContext and TestAuthorizationContext

        [Test]
        public void RendersCorrectHeaderText()
        {
            using var testContext = _helper.GetTestContext();
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<ViewPlan>(parameters => parameters.Add(p => p.planId, 24)); // render the page 

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
        public void DetailsContainInfoFromParameter()
        {
            using var testContext = _helper.GetTestContext();
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<ViewPlan>(parameters => parameters.Add(p => p.planId, 24));
            System.Threading.Thread.Sleep(500); // time for component to fully render

            var name = component.Find("p[id=\"Name\"]").TextContent;
            name.ShouldBe("nineteenth plan"); // change to API call
        }
    }
}
