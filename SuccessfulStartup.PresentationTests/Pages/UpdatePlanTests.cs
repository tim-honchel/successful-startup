using GenFu; // for generating mock objects
using Microsoft.EntityFrameworkCore; // for DbContextOptionsBuilder
using Moq; // for Mockc, Setup
using Moq.EntityFrameworkCore; // for ReturnsDbSet
using Shouldly; // for assertion
using SuccessfulStartup.Data.Contexts;
using SuccessfulStartup.Data.Entities;
using SuccessfulStartup.Presentation.Pages;
using System.Threading.Tasks; // for Sleep

namespace SuccessfulStartup.PresentationTests.Pages
{
    internal class UpdatePlanTests
    {
        private ContextHelper _helper; // contains helper methods for TestContext and TestAuthorizationContext
        private Mock<AuthenticationDbContext> _mockContext; // to simulate database calls
        private BusinessPlan _firstPlanInMockSet; // record to be updated or deleted

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _helper = new ContextHelper(); // contains helper methods to et TestContext and AuthenticationTestContext
        }

        [SetUp]
        public void SetUp() // resets before each test for clean data set
        {
            _mockContext = new Mock<AuthenticationDbContext>(new DbContextOptionsBuilder<AuthenticationDbContext>().Options, "dummyConnectionString");
            var allPlans = A.ListOf<BusinessPlan>(5);
            _firstPlanInMockSet = allPlans[0];
            _mockContext.Setup(context => context.BusinessPlans).ReturnsDbSet(allPlans); // when database is called, will return the 5 generated business plans
        }

        [Test]
        public async Task CancelButton_RendersDeleteButton_OnClick()
        {
            var planToDelete = _firstPlanInMockSet;
            using var testContext = _helper.GetTestContextWithMock(_mockContext);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<UpdatePlan>(parameters => parameters.Add(p => p.planId, planToDelete.Id));
            component.Find("btn[id=\"delete\"]").Click();
            System.Threading.Thread.Sleep(300); // time for component to fully render
            component.Find("btn[id=\"cancelDelete\"]").Click();
            System.Threading.Thread.Sleep(300); // time for component to fully render

            var rendersDeleteButton = component.FindAll("btn[id=\"delete\"]").Count > 0;
            rendersDeleteButton.ShouldBeTrue();
        }

        [Test]
        public async Task ConfirmDeleteButton_RendersSuccessMessage_OnClick()
        {
            var planToDelete = _firstPlanInMockSet;
            using var testContext = _helper.GetTestContextWithMock(_mockContext);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<UpdatePlan>(parameters => parameters.Add(p => p.planId, planToDelete.Id)); // URL passes in plan Id
            component.Find("btn[id=\"delete\"]").Click(); // clicks the delete button
            System.Threading.Thread.Sleep(300); // time for component to fully render
            component.Find("btn[id=\"confirmDelete\"]").Click(); // clicks the confirm delete button
            System.Threading.Thread.Sleep(300); // time for component to fully render

            var rendersSuccessMessage = component.Find("p[id =\"message\"]").TextContent == "Business plan deleted successfully!"; // finds whether the message is shown
            rendersSuccessMessage.ShouldBeTrue();
        }

        [Test]
        public async Task DeleteButton_RendersPrompt_OnClick()
        {
            var planToDelete = _firstPlanInMockSet;
            using var testContext = _helper.GetTestContextWithMock(_mockContext);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<UpdatePlan>(parameters => parameters.Add(p => p.planId, planToDelete.Id));
            component.Find("btn[id=\"delete\"]").Click();
            System.Threading.Thread.Sleep(300); // time for component to fully render

            var rendersPrompt = component.FindAll("btn[id=\"confirmDelete\"]").Count > 0;
            rendersPrompt.ShouldBeTrue();
        }


        [Test]
        public async Task DetailsContainInfoFromParameter()
        {
            var planToUpdate = _firstPlanInMockSet;
            using var testContext = _helper.GetTestContextWithMock(_mockContext);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<UpdatePlan>(parameters => parameters.Add(p => p.planId, planToUpdate.Id));

            System.Threading.Thread.Sleep(300); // time for component to fully render
            var input = component.Find("input[id=\"name\"]").ToMarkup();
            var startPosition = input.IndexOf("value") + 7;
            var endPosition = input.IndexOf("blazor") - 2;
            var name = input.Substring(startPosition, endPosition - startPosition);
            name.ShouldBe(planToUpdate.Name);
        }

        [Test]
        public void FormSubmit_RendersMessage_GivenValidInput()
        {
            var planToUpdate = _firstPlanInMockSet;
            using var testContext = _helper.GetTestContextWithMock(_mockContext);
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
            var planToUpdate = _firstPlanInMockSet;
            using var testContext = _helper.GetTestContextWithMock(_mockContext);
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
            var planToUpdate = _firstPlanInMockSet;
            using var testContext = _helper.GetTestContextWithMock(_mockContext);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<UpdatePlan>(parameters => parameters.Add(p => p.planId, planToUpdate.Id)); // render the page 

            var header = component.Find("h1[id =\"header\"]").TextContent;
            header.ShouldBe("Edit Your Plan");
        }

        [Test]
        public void RendersForm_GivenAuthorization()
        {
            var planToUpdate = _firstPlanInMockSet;
            using var testContext = _helper.GetTestContextWithMock(_mockContext);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<UpdatePlan>(parameters => parameters.Add(p => p.planId, planToUpdate.Id));

            var rendersForm = component.FindAll("form[id=\"form\"]").Count > 0;
            rendersForm.ShouldBeTrue();
        }

        [Test]
        public void RendersNoForm_GivenUnauthorization()
        {
            var planToUpdate = _firstPlanInMockSet;
            using var testContext = _helper.GetTestContextWithMock(_mockContext);
            var authorizationContext = _helper.GetAuthorizationContext(testContext, "email@gmail.com", false);

            var component = testContext.RenderComponent<UpdatePlan>(parameters => parameters.Add(p => p.planId, planToUpdate.Id));

            var rendersForm = component.FindAll("form[id=\"form\"]").Count > 0;
            rendersForm.ShouldBeFalse();
        }

        
    }
}

