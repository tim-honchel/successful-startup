using Shouldly; // for assertion
using SuccessfulStartup.Presentation.Pages;
using System.Threading.Tasks;

namespace SuccessfulStartup.PresentationTests.Pages
{
    internal class PlansTests : Bunit.TestContext // TestContext class allows addition of service configurations
    {
        private ContextHelper _helper = new ContextHelper(); // contains helper methods for TestContext and TestAuthorizationContext

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
           
        }

        [Test]
        public void RendersCorrectHeaderText()
        {
            using var testContext = _helper.GetTestContext();
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<Plans>(); // render the page

            var header = component.Find("h1").TextContent; // find the header text
            header.ShouldBe("Your Business Plans");
        }

        [Test]
        public void RendersTable_GivenAuthorization()
        {
            using var testContext = _helper.GetTestContext();
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<Plans>(); // render the page

            var rendersTable = component.FindAll("table").Count > 0;
            rendersTable.ShouldBeTrue();
        }

        [Test]
        public void RendersNoTable_GivenUnauthorization()
        {
            using var testContext = _helper.GetTestContext();
            var authorizationContext = _helper.GetAuthorizationContext(testContext,"email@gmail.com",false);

            var component = testContext.RenderComponent<Plans>(); // render the page

            var rendersTable = component.FindAll("table").Count > 0;
            rendersTable.ShouldBeFalse();
        }

        [Test]
        public void RendersRows_GivenAuthorizationAndMatchingRecords()
        {
            var usernameWithMatchingBusinessPlans = "tim.honchel@gmail.com"; // TODO: find a way to mock API or context
            using var testContext = _helper.GetTestContext();
            var authorizationContext = _helper.GetAuthorizationContext(testContext, usernameWithMatchingBusinessPlans);

            var component = testContext.RenderComponent<Plans>(); // render the page

            System.Threading.Thread.Sleep(500); // time for component to fully render
            var rendersRows = component.FindAll("tr").Count > 1;
            rendersRows.ShouldBeTrue();
            
        }
    }
}
