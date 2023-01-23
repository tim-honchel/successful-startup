using AutoMapper; // for IMapper
using Moq; // for Mock
using SuccessfulStartup.Data.APIs;
using SuccessfulStartup.Data.Contexts;
using SuccessfulStartup.Data.Mapping;

namespace SuccessfulStartup.PresentationTests.Pages
{
    internal class ContextHelper : Bunit.TestContext
    {
        private IMapper _mapper;

        public ContextHelper() {
            _mapper = AllMappingProfiles.GetMapper();
        }
        public Bunit.TestContext GetTestContext() // helper method to avoid duplication
        {
            var testContext = new Bunit.TestContext(); // Bunit and Nunit both have TestContext
            testContext.Services.AddSingleton(new EntityConverter(_mapper)); // for injection of EntityConverter
            testContext.Services.AddSingleton(new ReadOnlyApi(new AuthenticationDbContextFactory(), AllMappingProfiles.GetMapper())); // for injection of ReadOnlyApi
            testContext.Services.AddSingleton(new WriteOnlyApi(new AuthenticationDbContextFactory(), AllMappingProfiles.GetMapper())); // for injection of WriteOnlyApi
            return testContext;
        }

        public Bunit.TestContext GetTestContextWithMock(Mock<AuthenticationDbContext>? mockContext) // helper method to avoid duplication
        {
            var mockFactory = new Mock<AuthenticationDbContextFactory>(); // mock factory so the real database is not used
            mockFactory.Setup(mockedFactory => mockedFactory.CreateDbContext()).Returns(mockContext.Object); // factory will return the mock context instead of the real one

            var testContext = new Bunit.TestContext(); // Bunit and Nunit both have TestContext
            testContext.Services.AddSingleton(new EntityConverter(_mapper)); // for injection of EntityConverter
            testContext.Services.AddSingleton(new ReadOnlyApi(mockFactory.Object, _mapper)); // for injection of ReadOnlyApi
            testContext.Services.AddSingleton(new WriteOnlyApi(mockFactory.Object, _mapper)); // for injection of WriteOnlyApi

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
