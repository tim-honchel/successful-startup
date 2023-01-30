using Shouldly; // for assertion
using SuccessfulStartup.Presentation.Pages;

namespace SuccessfulStartup.PresentationTests.Pages
{
    internal class NewPlanTests
    {
        private readonly ContextHelper _helper = new(); // contains helper methods for TestContext and TestAuthorizationContext

        [Test]
        public void RendersCorrectHeaderText()
        {
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForUsers(handler, true); // returns userId
            using var testContext = _helper.GetTestContext(handler);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<NewPlan>(); // renders the page 

            var header = component.Find("h1[id =\"header\"]").TextContent;
            header.ShouldBe("Create a New Business Plan");
        }

        [Test]
        public void RendersForm_GivenAuthorization()
        {
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForUsers(handler, true); 
            using var testContext = _helper.GetTestContext(handler);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);
             
            var component = testContext.RenderComponent<NewPlan>();

            var rendersForm = component.FindAll("form[id =\"form\"]").Count > 0;
            rendersForm.ShouldBeTrue();
        }

        [Test]
        public void RendersNoForm_GivenUnauthorization()
        {
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForUsers(handler, true); 
            using var testContext = _helper.GetTestContext(handler);
            var authorizationContext = _helper.GetAuthorizationContext(testContext, false);

            var component = testContext.RenderComponent<NewPlan>();

            var rendersForm = component.FindAll("form[id =\"form\"]").Count > 0;
            rendersForm.ShouldBeFalse();
        }

        [Test]
        public void FormSubmit_RendersMessage_GivenValidInput()
        {
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForUsers(handler, true);
            _helper.SetupMockHandlerForPlans(handler, false, true); // returns OkResult from post request
            using var testContext = _helper.GetTestContext(handler);
            var authorizationContext = _helper.GetAuthorizationContext(testContext); 
            var component = testContext.RenderComponent<NewPlan>();

            component.Find("input[id=\"name\"]").Change("test name");
            component.Find("input[id=\"description\"]").Change("test description");
            component.Find("form").Submit();

            var message = component.Find("p[id =\"message\"]").TextContent;
            message.ShouldNotBeEmpty();
        }

        [TestCase("", "")]
        [TestCase("name only", "")]
        [TestCase("", "description only")]
        [TestCase("name is really really really really really really really long", "description")] // name exceeds max length
        public void FormSubmit_RendersNoMessage_GivenInvalidInput(string name, string description) // field messages will appear, but no form message
        {
            var handler = _helper.GetMockHandler();
            _helper.SetupMockHandlerForUsers(handler, true);
            using var testContext = _helper.GetTestContext(handler);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);
            var component = testContext.RenderComponent<NewPlan>();

            component.Find("input[id=\"name\"]").Change(name);
            component.Find("input[id=\"description\"]").Change(description);
            component.Find("form").Submit(); // EditForm is rendered as "form"

            var message = component.Find("p[id =\"message\"]").TextContent; // component re-renders automatically, no need to render new component
            message.ShouldBeEmpty();
        }
    }
}
