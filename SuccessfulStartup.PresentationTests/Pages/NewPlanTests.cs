using GenFu; // for generating mock data
using Microsoft.EntityFrameworkCore; // for DbContextOptionsBuilder
using Moq; // for Mock, Setup
using Moq.EntityFrameworkCore; // for ReturnsDbSet
using Shouldly; // for assertion
using SuccessfulStartup.Data.Authentication;
using SuccessfulStartup.Data.Contexts;
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

            var header = component.Find("h1[id =\"header\"]").TextContent;
            header.ShouldBe("Create a New Business Plan");
        }

        [Test]
        public void RendersForm_GivenAuthorization()
        {
            using var testContext = _helper.GetTestContext();
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<NewPlan>();

            var rendersForm = component.FindAll("form[id =\"form\"]").Count > 0;
            rendersForm.ShouldBeTrue();
        }

        [Test]
        public void RendersNoForm_GivenUnauthorization()
        {
            using var testContext = _helper.GetTestContext();
            var authorizationContext = _helper.GetAuthorizationContext(testContext, "email@gmail.com", false);

            var component = testContext.RenderComponent<NewPlan>();

            var rendersForm = component.FindAll("form[id =\"form\"]").Count > 0;
            rendersForm.ShouldBeFalse();
        }

        [Test]
        public void FormSubmit_RendersMessage_GivenValidInput()
        {
            var mockContext = new Mock<AuthenticationDbContext>(new DbContextOptionsBuilder<AuthenticationDbContext>().Options, "dummyConnectionString");
            var allUsers = A.ListOf<AppUser>(5);
            mockContext.Setup(context => context.Users).ReturnsDbSet(allUsers); // will return the 5 generated users
            using var testContext = _helper.GetTestContextWithMock(mockContext);
            var authorizationContext = _helper.GetAuthorizationContext(testContext, allUsers[0].UserName); // first mock user is logged in
            var component = testContext.RenderComponent<NewPlan>();

            component.Find("input[id=\"name\"]").Change("test name");
            component.Find("input[id=\"description\"]").Change("test description");
            component.Find("form").Submit();

            var message = component.Find("p[id =\"message\"]").TextContent;
            message.ShouldNotBeEmpty();
        }

        [TestCase("", "")]
        [TestCase("name only","")]
        [TestCase("", "description only")]
        [TestCase("name is really really really really really really really long", "description")] // name exceeds max length
        public void FormSubmit_RendersNoMessage_GivenInvalidInput(string name, string description) // field messages will appear, but no form message
        {
            var mockContext = new Mock<AuthenticationDbContext>(new DbContextOptionsBuilder<AuthenticationDbContext>().Options, "dummyConnectionString");
            var allUsers = A.ListOf<AppUser>(5);
            mockContext.Setup(context => context.Users).ReturnsDbSet(allUsers); // will return the 5 generated users
            using var testContext = _helper.GetTestContextWithMock(mockContext);
            var authorizationContext = _helper.GetAuthorizationContext(testContext, allUsers[0].UserName); // first mock user is logged in
            var component = testContext.RenderComponent<NewPlan>();

            component.Find("input[id=\"name\"]").Change(name);
            component.Find("input[id=\"description\"]").Change(description);
            component.Find("form").Submit(); // EditForm is rendered as "form"

            var message = component.Find("p[id =\"message\"]").TextContent; // component re-renders automatically, no need to render new component
            message.ShouldBeEmpty();
        }
    }
}
