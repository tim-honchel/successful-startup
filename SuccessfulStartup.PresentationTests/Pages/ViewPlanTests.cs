using GenFu; // for generating mock data
using Moq; // for Setup
using Shouldly; // for assertion
using SuccessfulStartup.Api.ViewModels;
using SuccessfulStartup.Data.Entities;
using SuccessfulStartup.Presentation.Pages;
using System.Net; // for HttpStatusCode
using System.Net.Http; // for HttpMethod 
using System.Text.Json; // for JsonSerializer
using System.Threading.Tasks; // for Task

namespace SuccessfulStartup.PresentationTests.Pages
{
    internal class ViewPlanTests
    {
        private readonly ContextHelper _helper = new(); // contains helper methods for TestContext and TestAuthorizationContext

        [Test]
        public async Task DetailsContainInfoFromParameter()
        {
            var service = _helper.GetMockApiService(); // mock API call service
            var planToView = A.New<BusinessPlanViewModel>();
            service.Setup(service => service.GetPlanByIdAsync(planToView.Id)).ReturnsAsync(planToView); // returns the generated plan;
            using var testContext = _helper.GetTestContext(service);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<ViewPlan>(parameters => parameters.Add(p => p.planId, planToView.Id));

            var name = component.Find("p[id=\"name\"]").TextContent;
            name.ShouldBe(planToView.Name);
        }

        [Test]
        public void RendersCorrectHeaderText()
        {
            var service = _helper.GetMockApiService(); 
            var planToView = A.New<BusinessPlanViewModel>();
            service.Setup(service => service.GetPlanByIdAsync(planToView.Id)).ReturnsAsync(planToView);;
            using var testContext = _helper.GetTestContext(service);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<ViewPlan>(parameters => parameters.Add(p => p.planId, planToView.Id)); // render the page with parameters passed in

            var header = component.Find("h1[id=\"header\"]").TextContent;
            header.ShouldBe("Your Business Plan");
        }

        [Test]
        public void RendersDetails_GivenAuthorization()
        {
            var service = _helper.GetMockApiService();
            var planToView = A.New<BusinessPlanViewModel>();
            service.Setup(service => service.GetPlanByIdAsync(planToView.Id)).ReturnsAsync(planToView); ;
            using var testContext = _helper.GetTestContext(service);
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<ViewPlan>(parameters => parameters.Add(p => p.planId, planToView.Id));

            var rendersDetails = component.FindAll("h6[id=\"plan\"]").Count > 0;
            rendersDetails.ShouldBeTrue();
        }

        [Test]
        public void RendersNoDetails_GivenUnauthorization()
        {
            var service = _helper.GetMockApiService();
            var planToView = A.New<BusinessPlanViewModel>();
            service.Setup(service => service.GetPlanByIdAsync(planToView.Id)).ReturnsAsync(planToView); ;
            using var testContext = _helper.GetTestContext(service);
            var authorizationContext = _helper.GetAuthorizationContext(testContext, false);

            var component = testContext.RenderComponent<ViewPlan>(parameters => parameters.Add(p => p.planId, planToView.Id));

            var rendersDetails = component.FindAll("h6[id=\"plan\"]").Count > 0;
            rendersDetails.ShouldBeFalse();
        }
    }
}
