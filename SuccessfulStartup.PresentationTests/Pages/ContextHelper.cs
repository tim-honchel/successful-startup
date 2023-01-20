

using SuccessfulStartup.Data.APIs;
using SuccessfulStartup.Data.Contexts;
using SuccessfulStartup.Data.Mapping;

namespace SuccessfulStartup.PresentationTests.Pages
{
    internal class ContextHelper : Bunit.TestContext
    {
        public Bunit.TestContext GetTestContext() // helper method to avoid duplication
        {
            var testContext = new Bunit.TestContext(); // Bunit and Nunit both have TestContext
            testContext.Services.AddSingleton(new EntityConverter(AllMappingProfiles.GetMapper())); // for injection of EntityConverter
            testContext.Services.AddSingleton(new ReadOnlyApi(new AuthenticationDbContextFactory(), AllMappingProfiles.GetMapper())); // for injection of ReadOnlyApi
            testContext.Services.AddSingleton(new WriteOnlyApi(new AuthenticationDbContextFactory(), AllMappingProfiles.GetMapper())); // for injection of ReadOnlyApi
            return testContext;
        }

        public TestAuthorizationContext GetAuthorizationContext(Bunit.TestContext testContext, string username = "email@gmail.com", bool authorized = true) // helper method to avoid duplication
        {
            var authorizationContext = testContext.AddTestAuthorization(); // inject fake authentication state provider
            var authorizationState = authorized ? AuthorizationState.Authorized : AuthorizationState.Unauthorized;
            authorizationContext.SetAuthorized(username, authorizationState); // authorize and set the current user
            return authorizationContext;
        }
    }
}
