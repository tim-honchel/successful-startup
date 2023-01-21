

using Microsoft.CodeAnalysis.CSharp.Syntax;
using Shouldly;
using SuccessfulStartup.Data.APIs;
using SuccessfulStartup.Data.Mapping;
using SuccessfulStartup.Presentation.Pages;
using System.Threading.Tasks;

namespace SuccessfulStartup.PresentationTests.Pages
{
    internal class UpdatePlanTests
    {
        private ContextHelper _helper; // contains helper methods for TestContext and TestAuthorizationContext
        private ReadOnlyApi _api;
        private int _validPlanId;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _helper = new ContextHelper();
            _api = new ReadOnlyApi(new Data.Contexts.AuthenticationDbContextFactory(), AllMappingProfiles.GetMapper());
            _validPlanId = 24;
        }

        [Test]
        public void RendersCorrectHeaderText()
        {
            using var testContext = _helper.GetTestContext();
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<UpdatePlan>(parameters => parameters.Add(p => p.planId, _validPlanId)); // render the page 

            var header = component.Find("h1").TextContent;
            header.ShouldBe("Edit Your Plan");
        }

        [Test]
        public void RendersForm_GivenAuthorization()
        {
            using var testContext = _helper.GetTestContext();
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<UpdatePlan>(parameters => parameters.Add(p => p.planId, _validPlanId));

            var rendersForm = component.FindAll("form").Count > 0;
            rendersForm.ShouldBeTrue();
        }

        [Test]
        public void RendersNoForm_GivenUnauthorization()
        {
            using var testContext = _helper.GetTestContext();
            var authorizationContext = _helper.GetAuthorizationContext(testContext, "email@gmail.com", false);

            var component = testContext.RenderComponent<UpdatePlan>(parameters => parameters.Add(p => p.planId, _validPlanId));

            var rendersForm = component.FindAll("form").Count > 0;
            rendersForm.ShouldBeFalse();
        }

        [Test]
        public async Task DetailsContainInfoFromParameter()
        {
            using var testContext = _helper.GetTestContext();
            var authorizationContext = _helper.GetAuthorizationContext(testContext);

            var component = testContext.RenderComponent<UpdatePlan>(parameters => parameters.Add(p => p.planId, _validPlanId));

            var fetchedPlan = await _api.GetPlanById(_validPlanId);
            System.Threading.Thread.Sleep(500); // time for component to fully render
            var input = component.Find("input[id=\"Name\"]").ToMarkup();
            var startPosition = input.IndexOf("value") + 7;
            var endPosition = input.IndexOf("blazor") - 2;
            var name = input.Substring(startPosition, endPosition-startPosition);
            name.ShouldBe(fetchedPlan.Name);
        }

        [Test]
        public void FormSubmit_RendersMessage_GivenValidInput()
        {
            using var testContext = _helper.GetTestContext();
            var authorizationContext = _helper.GetAuthorizationContext(testContext);
            var component = testContext.RenderComponent<UpdatePlan>(parameters => parameters.Add(p => p.planId, _validPlanId));

            component.Find("input[id=\"Name\"]").Change("test name");
            component.Find("input[id=\"Description\"]").Change("test description");
            component.Find("form").Submit();

            var message = component.Find("p[id =\"Message\"]").TextContent;
            message.ShouldNotBeEmpty();
        }

        [TestCase("", "")]
        [TestCase("name only", "")]
        [TestCase("", "description only")]
        [TestCase("name is really really really really really really really long", "description")] // name exceeds max length
        public void FormSubmit_RendersNoMessage_GivenInvalidInput(string name, string description) // field messages will appear, but no form message
        {
            using var testContext = _helper.GetTestContext();
            var authorizationContext = _helper.GetAuthorizationContext(testContext);
            var component = testContext.RenderComponent<UpdatePlan>(parameters => parameters.Add(p => p.planId, _validPlanId));

            component.Find("input[id=\"Name\"]").Change(name);
            component.Find("input[id=\"Description\"]").Change(description);
            component.Find("form").Submit(); // EditForm is rendered as "form"

            var message = component.Find("p[id =\"Message\"]").TextContent; // component re-renders automatically, no need to render new component
            message.ShouldBeEmpty();
        }
    }
}

