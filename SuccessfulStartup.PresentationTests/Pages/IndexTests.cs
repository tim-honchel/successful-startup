using Shouldly; // for assertion
using SuccessfulStartup.Presentation.Pages;

namespace SuccessfulStartup.PresentationTests.Pages
{
    [TestFixture]
    internal class IndexTests
    {


        [OneTimeSetUp]
        public void OneTimeSetUp() // testContext and renderComponent do not persist, must be recreated in each test
        {

        }

        [Test]
        public void RendersCorrectHeaderText()
        {
            using var testContext = new Bunit.TestContext(); // Bunit and Nunit both have TestContext
            var component = testContext.RenderComponent<Index>();
            var header = component.Find("h1").TextContent;
            header.ShouldBe("7 Steps to a Profitable Startup");
        }

    }
}
