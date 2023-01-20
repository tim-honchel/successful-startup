using Shouldly;
using SuccessfulStartup.Presentation.Pages;

namespace SuccessfulStartup.PresentationTests.Pages
{
    internal class NewPlanTests
    {
        private ContextHelper _helper = new ContextHelper(); // contains helper methods for TestContext and TestAuthorizationContext

        [Test]
        public void RendersCorrectHeaderText()
        {
            using var testContext = _helper.GetTestContext();
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<NewPlan>(); // render the page 

            var header = component.Find("h1").TextContent;
            header.ShouldBe("Create a New Business Plan");
        }

        [Test]
        public void RendersForm_GivenAuthorization()
        {
            using var testContext = _helper.GetTestContext();
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<NewPlan>();

            var rendersForm = component.FindAll("form").Count > 0;
            rendersForm.ShouldBeTrue();
        }

        [Test]
        public void RendersNoForm_GivenUnauthorization()
        {
            using var testContext = _helper.GetTestContext();
            var authorizationContext = _helper.GetAuthorizationContext(testContext, "email@gmail.com", false);

            var component = testContext.RenderComponent<NewPlan>();

            var rendersForm = component.FindAll("form").Count > 0;
            rendersForm.ShouldBeFalse();
        }

        [Test]
        public void FormSubmit_RendersMessage_GivenValidInput()
        {
            using var testContext = _helper.GetTestContext();
            var authorizationContext = _helper.GetAuthorizationContext(testContext);
            var component = testContext.RenderComponent<NewPlan>();

            component.Find("input[id=\"Name\"]").Change("test name");
            component.Find("input[id=\"Description\"]").Change("test description");
            component.Find("form").Submit();

            var message = component.Find("p[id =\"Message\"]").TextContent;
            message.ShouldNotBeEmpty();
        }

        [TestCase("", "")]
        [TestCase("name only","")]
        [TestCase("", "description only")]
        [TestCase("name is really really really really really really really long", "description")] // name exceeds max length
        public void FormSubmit_RendersNoMessage_GivenInvalidInput(string name, string description) // field messages will appear, but no form message
        {
            using var testContext = _helper.GetTestContext();
            var authorizationContext = _helper.GetAuthorizationContext(testContext);
            var component = testContext.RenderComponent<NewPlan>();

            component.Find("input[id=\"Name\"]").Change(name);
            component.Find("input[id=\"Description\"]").Change(description);
            component.Find("form").Submit(); // EditForm is rendered as "form"

            var message = component.Find("p[id =\"Message\"]").TextContent; // component re-renders automatically, no need to render new component
            message.ShouldBeEmpty();
        }
    }
}
