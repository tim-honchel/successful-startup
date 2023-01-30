using GenFu; // for generating mock objects
using Shouldly; // for assertion
using SuccessfulStartup.Data.Entities;
using SuccessfulStartup.Presentation.Pages;
using System.Text.Json; // for JsonSerializer
using System.Threading.Tasks; // for Sleep

namespace SuccessfulStartup.PresentationTests.Pages
{
    internal class UpdatePlanTests
    {
        private readonly ContextHelper _helper = new(); // contains helper methods for TestContext and TestAuthorizationContext

        [Test]
        public async Task CancelButton_RendersDeleteButton_OnClick()
        {

            var handler = _helper.GetMockHandler();
            var planToDelete = A.New<BusinessPlan>();
            _helper.SetupMockHandlerForPlans(handler, true, true, JsonSerializer.Serialize(planToDelete));
            using var testContext = _helper.GetTestContext(handler);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<UpdatePlan>(parameters => parameters.Add(p => p.planId, planToDelete.Id));
            component.Find("btn[id=\"delete\"]").Click();
            component.Find("btn[id=\"cancelDelete\"]").Click();

            var rendersDeleteButton = component.FindAll("btn[id=\"delete\"]").Count > 0;
            rendersDeleteButton.ShouldBeTrue();
        }

        [Test]
        public async Task ConfirmDeleteButton_RendersSuccessMessage_OnClick()
        {
            var handler = _helper.GetMockHandler();
            var planToDelete = A.New<BusinessPlan>();
            _helper.SetupMockHandlerForPlans(handler, true, true, JsonSerializer.Serialize(planToDelete)); // returns the plan entered as a paramter
            _helper.SetupMockHandlerForPlans(handler, false, true); // returns Ok after deletion request
            using var testContext = _helper.GetTestContext(handler);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<UpdatePlan>(parameters => parameters.Add(p => p.planId, planToDelete.Id)); // URL passes in plan Id
            component.Find("btn[id=\"delete\"]").Click(); // clicks the delete button
            component.Find("btn[id=\"confirmDelete\"]").Click(); // clicks the confirm delete button

            var rendersSuccessMessage = component.Find("p[id =\"message\"]").TextContent == "Business plan deleted successfully!"; // finds whether the message is shown
            rendersSuccessMessage.ShouldBeTrue();
        }

        [Test]
        public async Task DeleteButton_RendersPrompt_OnClick()
        {
            var handler = _helper.GetMockHandler();
            var planToDelete = A.New<BusinessPlan>();
            _helper.SetupMockHandlerForPlans(handler, true, true, JsonSerializer.Serialize(planToDelete));
            using var testContext = _helper.GetTestContext(handler);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<UpdatePlan>(parameters => parameters.Add(p => p.planId, planToDelete.Id));
            component.Find("btn[id=\"delete\"]").Click();

            var rendersPrompt = component.FindAll("btn[id=\"confirmDelete\"]").Count > 0;
            rendersPrompt.ShouldBeTrue();
        }


        [Test]
        public async Task DetailsContainInfoFromParameter()
        {
            var handler = _helper.GetMockHandler();
            var planToUpdate = A.New<BusinessPlan>();
            _helper.SetupMockHandlerForPlans(handler, true, true, JsonSerializer.Serialize(planToUpdate));
            using var testContext = _helper.GetTestContext(handler);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<UpdatePlan>(parameters => parameters.Add(p => p.planId, planToUpdate.Id));

            System.Threading.Thread.Sleep(300); // time for component to fully render
            var input = component.Find("input[id=\"name\"]").ToMarkup();
            input.ShouldContain(planToUpdate.Name);
        }

        [Test]
        public void FormSubmit_RendersMessage_GivenValidInput()
        {
            var handler = _helper.GetMockHandler();
            var planToUpdate = A.New<BusinessPlan>();
            _helper.SetupMockHandlerForPlans(handler, true, true, JsonSerializer.Serialize(planToUpdate));
            using var testContext = _helper.GetTestContext(handler);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);
            var component = testContext.RenderComponent<UpdatePlan>(parameters => parameters.Add(p => p.planId, planToUpdate.Id));

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
            var planToUpdate = A.New<BusinessPlan>();
            _helper.SetupMockHandlerForPlans(handler, true, true, JsonSerializer.Serialize(planToUpdate));
            using var testContext = _helper.GetTestContext(handler);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);
            var component = testContext.RenderComponent<UpdatePlan>(parameters => parameters.Add(p => p.planId, planToUpdate.Id));

            component.Find("input[id=\"name\"]").Change(name);
            component.Find("input[id=\"description\"]").Change(description);
            component.Find("form").Submit(); // EditForm is rendered as "form"

            var message = component.Find("p[id =\"message\"]").TextContent; // component re-renders automatically, no need to render new component
            message.ShouldBeEmpty();
        }

        [Test]
        public void RendersCorrectHeaderText()
        {
            var handler = _helper.GetMockHandler();
            var planToUpdate = A.New<BusinessPlan>();
            _helper.SetupMockHandlerForPlans(handler, true, true, JsonSerializer.Serialize(planToUpdate));
            using var testContext = _helper.GetTestContext(handler);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<UpdatePlan>(parameters => parameters.Add(p => p.planId, planToUpdate.Id)); // render the page 

            var header = component.Find("h1[id =\"header\"]").TextContent;
            header.ShouldBe("Edit Your Plan");
        }

        [Test]
        public void RendersForm_GivenAuthorization()
        {
            var handler = _helper.GetMockHandler();
            var planToUpdate = A.New<BusinessPlan>();
            _helper.SetupMockHandlerForPlans(handler, true, true, JsonSerializer.Serialize(planToUpdate));
            using var testContext = _helper.GetTestContext(handler);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<UpdatePlan>(parameters => parameters.Add(p => p.planId, planToUpdate.Id));

            var rendersForm = component.FindAll("form[id=\"form\"]").Count > 0;
            rendersForm.ShouldBeTrue();
        }

        [Test]
        public void RendersNoForm_GivenUnauthorization()
        {
            var handler = _helper.GetMockHandler();
            var planToUpdate = A.New<BusinessPlan>();
            _helper.SetupMockHandlerForPlans(handler, true, true, JsonSerializer.Serialize(planToUpdate));
            using var testContext = _helper.GetTestContext(handler);
            var authorizationContext = _helper.GetAuthorizationContext(testContext, false);

            var component = testContext.RenderComponent<UpdatePlan>(parameters => parameters.Add(p => p.planId, planToUpdate.Id));

            var rendersForm = component.FindAll("form[id=\"form\"]").Count > 0;
            rendersForm.ShouldBeFalse();
        }


    }
}

