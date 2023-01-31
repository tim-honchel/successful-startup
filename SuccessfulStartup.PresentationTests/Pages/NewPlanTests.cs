using Moq; // for Mock, Setup
using Shouldly; // for assertion
using SuccessfulStartup.Api.ViewModels;
using SuccessfulStartup.Presentation.Pages;
using System;
using System.Net; // for HttpStatusCode
using System.Net.Http; // for HttpMethod 

namespace SuccessfulStartup.PresentationTests.Pages
{
    internal class NewPlanTests
    {
        private readonly ContextHelper _helper = new(); // contains helper methods for TestContext and TestAuthorizationContext


        [Test]
        public void FormSubmit_RendersMessage_GivenValidInput()
        {
            var service = _helper.GetMockApiService();
            service.Setup(service => service.GetUserIdByUsernameAsync(_helper.standardUser.UserName)).ReturnsAsync(_helper.standardUser.Id); // returns standard Id, no need to return anything for SaveNewPlanAsync
            using var testContext = _helper.GetTestContext(service);
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
            var service = _helper.GetMockApiService();
            service.Setup(service => service.GetUserIdByUsernameAsync(_helper.standardUser.UserName)).ReturnsAsync(_helper.standardUser.Id); // returns standard Id
            using var testContext = _helper.GetTestContext(service);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);
            var component = testContext.RenderComponent<NewPlan>();

            component.Find("input[id=\"name\"]").Change(name);
            component.Find("input[id=\"description\"]").Change(description);
            component.Find("form").Submit(); // EditForm is rendered as "form"

            var message = component.Find("p[id =\"message\"]").TextContent; // component re-renders automatically, no need to render new component
            message.ShouldBeEmpty();
        }

        [Test]
        public void HidesForm_GivenApiException()
        {
            var service = _helper.GetMockApiService();
            service.Setup(service => service.GetUserIdByUsernameAsync(_helper.standardUser.UserName)).Throws<Exception>();
            using var testContext = _helper.GetTestContext(service);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<NewPlan>();

            var formHidden = component.FindAll("form[hidden]").Count > 0;
            formHidden.ShouldBeTrue();
        }

        [Test]
        public void RendersCorrectHeaderText()
        {
            var service = _helper.GetMockApiService();
            service.Setup(service => service.GetUserIdByUsernameAsync(_helper.standardUser.UserName)).ReturnsAsync(_helper.standardUser.Id);
            using var testContext = _helper.GetTestContext(service);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<NewPlan>(); // renders the page 

            var header = component.Find("h1[id =\"header\"]").TextContent;
            header.ShouldBe("Create a New Business Plan");
        }

        [Test]
        public void RendersErrorMessage_GivenApiException()
        {
            var service = _helper.GetMockApiService();
            service.Setup(service => service.GetUserIdByUsernameAsync(_helper.standardUser.UserName)).Throws<Exception>();
            using var testContext = _helper.GetTestContext(service);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<NewPlan>();

            var message = component.Find("p[id =\"message\"]").TextContent;
            message.ShouldContain("error");
        }

        [Test]
        public void RendersForm_GivenAuthorization()
        {
            var service = _helper.GetMockApiService();
            service.Setup(service => service.GetUserIdByUsernameAsync(_helper.standardUser.UserName)).ReturnsAsync(_helper.standardUser.Id); // returns standard Id
            using var testContext = _helper.GetTestContext(service);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);
             
            var component = testContext.RenderComponent<NewPlan>();

            var rendersForm = component.FindAll("form[id =\"form\"]").Count > 0;
            rendersForm.ShouldBeTrue();
        }

        [Test]
        public void RendersNoForm_GivenUnauthorization()
        {
            var service = _helper.GetMockApiService();
            service.Setup(service => service.GetUserIdByUsernameAsync(_helper.standardUser.UserName)).ReturnsAsync(_helper.standardUser.Id); // returns standard Id
            using var testContext = _helper.GetTestContext(service);
            var authorizationContext = _helper.GetAuthorizationContext(testContext, false);

            var component = testContext.RenderComponent<NewPlan>();

            var rendersForm = component.FindAll("form[id =\"form\"]").Count > 0;
            rendersForm.ShouldBeFalse();
        }

    }
}
