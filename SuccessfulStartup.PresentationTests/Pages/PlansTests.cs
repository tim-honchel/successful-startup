using GenFu; // for generating mock data
using Microsoft.EntityFrameworkCore; // for DbContextOptionsBuilder
using Moq; // for Mock, Setup
using Moq.EntityFrameworkCore; // for ReturnsDbSet
using Moq.Protected;
using Shouldly; // for assertion
using SuccessfulStartup.Data.Authentication;
using SuccessfulStartup.Data.Contexts;
using SuccessfulStartup.Data.Entities;
using SuccessfulStartup.Presentation.Pages;
using System;
using System.Net; // for HttpStatusCode
using System.Net.Http; // for HttpMessageHandler
using System.Threading.Tasks; // for Task

namespace SuccessfulStartup.PresentationTests.Pages
{
    internal class PlansTests : Bunit.TestContext // TestContext class allows addition of service configurations
    {
        private ContextHelper _helper = new(); // contains helper methods for TestContext and TestAuthorizationContext
        //private Mock<IHttpClientFactory> _mockHttpClientFactory = new();
        //private Mock<HttpMessageHandler> _mockHandler = new();

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
            //var allUsers = A.ListOf<AppUser>(5);
            //var allPlans = A.ListOf<BusinessPlan>(5);
            //allPlans[0].AuthorId = allUsers[0].Id; // first mock plan was created by first mock user
            //var mockResponse = new HttpResponseMessage
            //{
            //    Content = new StringContent(allPlans[0]),
            //    StatusCode = HttpStatusCode.OK
            //};
            //_mockHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(message => message.RequestUri == new Uri($"https://localhost:7261/Plan/all/{authorId}"))).ReturnsAsync(mockResponse);
            //_mockHttpClientFactory.Setup(factory => factory.CreateClient("Client")).Returns(new HttpClient(_mockHandler.Object));

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
