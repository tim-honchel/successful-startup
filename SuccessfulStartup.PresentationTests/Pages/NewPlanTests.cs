using GenFu; // for generating data
using Microsoft.AspNetCore.Components; // for NavManager
using Moq; // for Mock, Setup
using Shouldly; // for assertion
using SuccessfulStartup.Api.ViewModels;
using SuccessfulStartup.Presentation.Pages;
using System;

namespace SuccessfulStartup.PresentationTests.Pages
{
    internal class NewPlanTests
    {
        private readonly ContextHelper _helper = new(); // contains helper methods for TestContext and TestAuthorizationContext


        [Test]
        public void FormSubmit_RendersMessage_GivenValidInput()
        {
            var service = _helper.GetMockApiService();
            using var testContext = _helper.GetTestContext(service);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);
            var component = testContext.RenderComponent<NewPlan>();

            component.Find("input[id=\"name\"]").Change("test name");
            component.Find("input[id=\"description\"]").Change("test description");
            component.Find("form").Submit();

            var message = component.Find("p[id =\"message\"]").TextContent;
            message.ShouldNotBeEmpty();
        }

        [Test]
        public void ButtonClick_NavigatesToNewPage() // field messages will appear, but no form message
        {
            var planToSave = A.New<BusinessPlanViewModel>();
            var service = _helper.GetMockApiService();
            service.Setup(service => service.SaveNewPlanAsync(It.Is<BusinessPlanViewModel>(plan => plan.Name == planToSave.Name))).ReturnsAsync(planToSave.Id);
            var testContext = _helper.GetTestContext(service);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);
            var navigation = testContext.Services.GetRequiredService<NavigationManager>(); // for testing the URL of the link, needs to come after all other services are registered
            var component = testContext.RenderComponent<NewPlan>();

            component.Find("input[id=\"name\"]").Change(planToSave.Name);
            component.Find("input[id=\"description\"]").Change(planToSave.Description);
            component.Find("form").Submit(); // EditForm is rendered as "form"
            component.Find("button[id =\"view-link\"]").Click();

            navigation.Uri.ShouldContain($"plans/{planToSave.Id}");
        }

        [Test]
        public void FormSubmit_RendersButtonWithLinkToNewPlan_GivenSuccessfulSave() // field messages will appear, but no form message
        {
            var service = _helper.GetMockApiService();
            using var testContext = _helper.GetTestContext(service);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);
            var component = testContext.RenderComponent<NewPlan>();

            component.Find("input[id=\"name\"]").Change("test name");
            component.Find("input[id=\"description\"]").Change("test description");
            component.Find("form").Submit(); // EditForm is rendered as "form"

            var buttonRenders = component.FindAll("button[id =\"view-link\"]").Count>0; // component re-renders automatically, no need to render new component
            buttonRenders.ShouldBeTrue();
        }

        [TestCase("", "")]
        [TestCase("name only", "")]
        [TestCase("", "description only")]
        [TestCase("name is really really really really really really really long", "description")] // name exceeds max length
        public void FormSubmit_RendersNoMessage_GivenInvalidInput(string name, string description) // field messages will appear, but no form message
        {
            var service = _helper.GetMockApiService();
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
        public void RendersCorrectHeaderText()
        {
            var service = _helper.GetMockApiService();
            using var testContext = _helper.GetTestContext(service);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<NewPlan>(); // renders the page 

            var header = component.Find("h1[id =\"header\"]").TextContent;
            header.ShouldBe("Create a New Business Plan");
        }

        [Test]
        public void RendersForm_GivenAuthorization()
        {
            var service = _helper.GetMockApiService();
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
            using var testContext = _helper.GetTestContext(service);
            var authorizationContext = _helper.GetAuthorizationContext(testContext, false);

            var component = testContext.RenderComponent<NewPlan>();

            var rendersForm = component.FindAll("form[id =\"form\"]").Count > 0;
            rendersForm.ShouldBeFalse();
        }

    }
}
