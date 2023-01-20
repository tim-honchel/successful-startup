using Shouldly; // for assertion
using SuccessfulStartup.Presentation.Pages;

namespace SuccessfulStartup.PresentationTests.Pages
{
    [TestFixture]
    internal class IndexTests
    {
        private ContextHelper _helper = new ContextHelper(); // contains helper methods for TestContext and TestAuthorizationContext

        [OneTimeSetUp]
        public void OneTimeSetUp() // testContext and renderComponent do not persist, must be recreated in each test
        {

        }

        [Test]
        public void RendersCorrectHeaderText()
        {
            using var testContext = _helper.GetTestContext();

            var component = testContext.RenderComponent<Index>(); // render the page

            var header = component.Find("h1").TextContent;
            header.ShouldBe("7 Steps to a Profitable Startup");
        }

    }
}
