using GenFu;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using Shouldly; // for assertion
using SuccessfulStartup.Data.Contexts;
using SuccessfulStartup.Data.Entities;
using SuccessfulStartup.Presentation.Pages;
using System.Threading.Tasks; // for Sleep

namespace SuccessfulStartup.PresentationTests.Pages
{
    internal class ViewPlanTests
    {
        private ContextHelper _helper; // contains helper methods for TestContext and TestAuthorizationContext
        private Mock<AuthenticationDbContext> _mockContext; // to simulate database calls
        private BusinessPlan _firstPlanInMockSet; // record to be updated or deleted

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _helper = new ContextHelper();
            //_api = new ReadOnlyApi(new Data.Contexts.AuthenticationDbContextFactory(), AllMappingProfiles.GetMapper());
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
        public void RendersCorrectHeaderText()
        {
            var planToView = _firstPlanInMockSet;
            using var testContext = _helper.GetTestContextWithMock(_mockContext);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<ViewPlan>(parameters => parameters.Add(p => p.planId, planToView.Id)); // render the page with parameters passed in

            var header = component.Find("h1[id=\"header\"]").TextContent;
            header.ShouldBe("Your Business Plan");
        }

        [Test]
        public void RendersDetails_GivenAuthorization()
        {
            var planToView = _firstPlanInMockSet;
            using var testContext = _helper.GetTestContextWithMock(_mockContext);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<ViewPlan>(parameters => parameters.Add(p => p.planId, planToView.Id));

            var rendersDetails = component.FindAll("h6[id=\"plan\"]").Count > 0;
            rendersDetails.ShouldBeTrue();
        }

        [Test]
        public void RendersNoDetails_GivenUnauthorization()
        {
            var planToView = _firstPlanInMockSet;
            using var testContext = _helper.GetTestContextWithMock(_mockContext);
            var authorizationContext = _helper.GetAuthorizationContext(testContext, "email@gmail.com", false);

            var component = testContext.RenderComponent<ViewPlan>(parameters => parameters.Add(p => p.planId, planToView.Id));

            var rendersDetails = component.FindAll("h6[id=\"plan\"]").Count > 0;
            rendersDetails.ShouldBeFalse();
        }

        [Test]
        public async Task DetailsContainInfoFromParameter()
        {
            var planToView = _firstPlanInMockSet;
            using var testContext = _helper.GetTestContextWithMock(_mockContext);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);
            var existingPlanId = 24;

            var component = testContext.RenderComponent<ViewPlan>(parameters => parameters.Add(p => p.planId, planToView.Id));

            System.Threading.Thread.Sleep(50); // time for component to fully render
            var name = component.Find("p[id=\"name\"]").TextContent;
            name.ShouldBe(planToView.Name);
        }
    }
}
