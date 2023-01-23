using GenFu; // for generating mock data
using Microsoft.EntityFrameworkCore; // for DbContextOptionsBuilder
using Moq; // for Mock, Setup
using Moq.EntityFrameworkCore; // for ReturnsDbSet
using Shouldly; // for assertion
using SuccessfulStartup.Data.Authentication;
using SuccessfulStartup.Data.Contexts;
using SuccessfulStartup.Data.Entities;
using SuccessfulStartup.Presentation.Pages;

namespace SuccessfulStartup.PresentationTests.Pages
{
    internal class PlansTests : Bunit.TestContext // TestContext class allows addition of service configurations
    {
        private ContextHelper _helper = new ContextHelper(); // contains helper methods for TestContext and TestAuthorizationContext

        [Test]
        public void RendersCorrectHeaderText()
        {
            using var testContext = _helper.GetTestContext();
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<Plans>(); // render the page

            var header = component.Find("h1[id=\"header\"]").TextContent; // find the header text
            header.ShouldBe("Your Business Plans");
        }

        [Test]
        public void RendersTable_GivenAuthorization()
        {
            using var testContext = _helper.GetTestContext();
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<Plans>(); // render the page

            var rendersTable = component.FindAll("table[id=\"plans\"]").Count > 0;
            rendersTable.ShouldBeTrue();
        }

        [Test]
        public void RendersNoTable_GivenUnauthorization()
        {
            using var testContext = _helper.GetTestContext();
            var authorizationContext = _helper.GetAuthorizationContext(testContext,"email@gmail.com",false);

            var component = testContext.RenderComponent<Plans>(); // render the page

            var rendersTable = component.FindAll("table[id=\"plans\"]").Count > 0;
            rendersTable.ShouldBeFalse();
        }

        [Test]
        public void RendersRows_GivenAuthorizationAndMatchingRecords()
        {
            var mockContext = new Mock<AuthenticationDbContext>(new DbContextOptionsBuilder<AuthenticationDbContext>().Options, "dummyConnectionString");
            var allUsers = A.ListOf<AppUser>(5);
            var allPlans = A.ListOf<BusinessPlan>(5);
            allPlans[0].AuthorId = allUsers[0].Id; // first mock plan was created by first mock user
            mockContext.Setup(context => context.Users).ReturnsDbSet(allUsers); // will return the 5 generated users
            mockContext.Setup(context => context.BusinessPlans).ReturnsDbSet(allPlans); // return 5 general plans
            using var testContext = _helper.GetTestContextWithMock(mockContext);
            var authorizationContext = _helper.GetAuthorizationContext(testContext, allUsers[0].UserName); // first mock user is logged in

            var component = testContext.RenderComponent<Plans>(); // render the page

            System.Threading.Thread.Sleep(500); // time for component to fully render
            var rendersRows = component.FindAll("tr[id=\"plan\"]").Count > 0;
            rendersRows.ShouldBeTrue();
            
        }
    }
}
