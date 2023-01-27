using GenFu; // for generating mock data
using Moq; // for Mock
using Moq.Protected; // for MockBehavior
using SuccessfulStartup.Api.ViewModels;
using SuccessfulStartup.Data.Authentication;
using SuccessfulStartup.Presentation.Services;
using System.Net; // for HttpStatusCode
using System.Net.Http; // for HttpMessageHandler
using System.Text.Json; // for JsonSerializer
using System.Threading; // for CancellationToken
using System.Threading.Tasks; // for ReturnsAsync

namespace SuccessfulStartup.PresentationTests
{
    internal class ContextHelper : Bunit.TestContext // helper methods to avoid duplication during testing
    {
        public AppUser standardUser = new AppUser() // creates a standard user for all unit tests for predictable results
        {
            AccessFailedCount = 0,
            ConcurrencyStamp = "5f931d32-af66-4bfe-ad75-cc4f72d221e4",
            Email = "email@gmail.com",
            EmailConfirmed = true,
            Id = "00000000-0000-0000-0000-000000000000",
            LockoutEnabled = true,
            LockoutEnd = null,
            NormalizedEmail = "EMAIL@GMAIL.COM",
            NormalizedUserName = "EMAIL@GMAIL.COM",
            PasswordHash = "SDFASFDsdafsadfSFS24533251",
            PhoneNumber = null,
            PhoneNumberConfirmed = false,
            SecurityStamp = "799a234e-7cdd-4616-84c6-b983190a6d29",
            TwoFactorEnabled = false,
            UserName = "email@gmail.com"
        };

        public TestAuthorizationContext GetAuthorizationContext(Bunit.TestContext testContext, bool authorized = true, string username = "email@gmail.com")
        {
            var authorizationContext = testContext.AddTestAuthorization(); // inject fake authentication state provider
            var authorizationState = authorized ? AuthorizationState.Authorized : AuthorizationState.Unauthorized;
            authorizationContext.SetAuthorized(username, authorizationState); // authorize and set the current user
            return authorizationContext;
        }

        public Mock<HttpMessageHandler> GetMockHandler() // setups will be written in each unit test
        {
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict); 
            return mockHandler;
        }

        public Bunit.TestContext GetTestContext(Mock<HttpMessageHandler> handler) 
        {

            var testContext = new Bunit.TestContext(); // Bunit and Nunit both have TestContext, necessary to specify
            testContext.Services.AddSingleton(new ApiCallService(handler.Object)); // injects ApiCallService with mock handler
            return testContext;
        }

        public Mock<HttpMessageHandler> SetupMockHandlerForPlans(Mock<HttpMessageHandler> mockHandler, bool getRequest, bool okResponse, string returnedJson = "success")
        {
            var statusCode = okResponse ? HttpStatusCode.OK : HttpStatusCode.NotFound;
            var content = new StringContent(returnedJson); // converts to HttpContent
            if (getRequest == true) // for get requests
            {
                mockHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(request => request.Method == HttpMethod.Get && request.RequestUri.ToString().Contains("Plan")), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(new HttpResponseMessage() { StatusCode = statusCode, Content = content }).Verifiable();
            }
            else // for post, put, and delete requests
            {
                mockHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(request => request.Method != HttpMethod.Get && request.RequestUri.ToString().Contains("Plan")), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(new HttpResponseMessage() { StatusCode = statusCode }).Verifiable();
            }
            
            return mockHandler;
        }

        public Mock<HttpMessageHandler> SetupMockHandlerForUsers(Mock<HttpMessageHandler> mockHandler, bool okResponse, string returnedJson = "\"00000000-0000-0000-0000-000000000000\"")
        {
            var statusCode = okResponse ? HttpStatusCode.OK : HttpStatusCode.NotFound;
            var content = new StringContent(returnedJson);
            mockHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(request => request.Method == HttpMethod.Get && request.RequestUri.ToString().Contains("User")), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(new HttpResponseMessage() { StatusCode = statusCode, Content = content }).Verifiable();
            return mockHandler;
        }

    }
}