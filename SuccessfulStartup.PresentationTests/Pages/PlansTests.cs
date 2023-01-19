using AutoMapper;
using GenFu;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly; // for assertion
using SuccessfulStartup.Data;
using SuccessfulStartup.Data.APIs;
using SuccessfulStartup.Data.Authentication;
using SuccessfulStartup.Data.Contexts;
using SuccessfulStartup.Data.Entities;
using SuccessfulStartup.Data.Mapping;
using SuccessfulStartup.Domain.Entities;
using SuccessfulStartup.Presentation.Pages;
using System.Threading.Tasks;

namespace SuccessfulStartup.PresentationTests.Pages
{
    internal class PlansTests : Bunit.TestContext // TestContext class allows addition of service configurations
    {
        public Bunit.TestContext GetTestContext() // helper method to avoid duplication
        {
            var testContext = new Bunit.TestContext(); // Bunit and Nunit both have TestContext
            testContext.Services.AddSingleton(new EntityConverter(AllMappingProfiles.GetMapper())); // for injection of EntityConverter
            testContext.Services.AddSingleton(new ReadOnlyApi(new AuthenticationDbContextFactory(), AllMappingProfiles.GetMapper())); // for injection of ReadOnlyApi
            return testContext;
        }

        public TestAuthorizationContext GetAuthorizationContext(Bunit.TestContext testContext, string username = "email@gmail.com", bool authorized = true) // helper method to avoid duplication
        {
            var authorizationContext = testContext.AddTestAuthorization(); // inject fake authentication state provider
            var authorizationState = authorized ? AuthorizationState.Authorized : AuthorizationState.Unauthorized;
            authorizationContext.SetAuthorized(username, authorizationState); // authorize and set the current user
            return authorizationContext;
        }


        [Test]
        public void RendersCorrectHeaderText()
        {
            using var testContext = GetTestContext();
            var authorizationContext = GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<Plans>(); // render the page

            var header = component.Find("h1").TextContent; // find the header text
            header.ShouldBe("Your Business Plans");
        }

        [Test]
        public void RendersTable_GivenAuthorization()
        {
            using var testContext = GetTestContext();
            var authorizationContext = GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<Plans>(); // render the page

            var rendersTable = component.FindAll("table").Count > 0;
            rendersTable.ShouldBeTrue();
        }

        [Test]
        public void RendersNoTable_GivenUnauthorization()
        {
            using var testContext = GetTestContext();
            var authorizationContext = GetAuthorizationContext(testContext,"email@gmail.com",false);

            var component = testContext.RenderComponent<Plans>(); // render the page

            var rendersTable = component.FindAll("table").Count > 0;
            rendersTable.ShouldBeFalse();
        }

        [Test]
        public async Task RendersRows_GivenAuthorizationAndMatchingRecords()
        {
            using var testContext = GetTestContext();
            var authorizationContext = GetAuthorizationContext(testContext, "usernameWithMatchingBusinessPlans");
            var plans = A.ListOf<BusinessPlanDomain>();
            var mockApi = new Mock<ReadOnlyApi>(new AuthenticationDbContextFactory(), AllMappingProfiles.GetMapper()); // TODO: the mock is not being injected or called
            mockApi.Setup(api => api.GetUserIdByUsername(It.IsAny<string>())).ReturnsAsync("userIdWithMatchingBusinessPlans");
            mockApi.Setup(api => api.GetAllPlansByAuthorId(It.IsAny<string>())).ReturnsAsync(plans);

            var component = testContext.RenderComponent<Plans>(); // render the page

            var rendersRows = component.FindAll("tr").Count > 1;
            rendersRows.ShouldBeTrue();
        }
    }
}
