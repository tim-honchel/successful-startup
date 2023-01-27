using Shouldly; // for assertion
using SuccessfulStartup.Presentation.Pages;

namespace SuccessfulStartup.PresentationTests.Pages
{
    [TestFixture]
    internal class IndexTests
    {

        [Test]
        public void RendersCorrectHeaderText()
        {
            var testContext = new Bunit.TestContext();

            var component = testContext.RenderComponent<Index>(); // render the page

            var header = component.Find("h1").TextContent;
            header.ShouldBe("7 Steps to a Profitable Startup");
        }

    }
}
