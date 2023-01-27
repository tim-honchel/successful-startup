using Microsoft.AspNetCore.Mvc; // for OkObjectResult
using Moq; // for Mock, Setup
using Shouldly; // for assertion
using SuccessfulStartup.Api.Controllers;
using SuccessfulStartup.Domain.Repositories.ReadOnly;

namespace SuccessfulStartup.ApiTests.Controllers
{
    [TestFixture]
    internal class UserControllerTests
    {
        private ApiHelper _helper = new ApiHelper();
        private Mock<IUserReadOnlyRepository> _mockRepository = new Mock<IUserReadOnlyRepository>();
        private UserController _controller;

        [Test]
        public async Task GetUserIdByUsername_ReturnsId_GivenExistingUsername()
        {
            _mockRepository.Setup(repository => repository.GetUserIdByUsernameAsync(_helper.standardUser.UserName)).ReturnsAsync(_helper.standardUser.Id);
            _controller = new UserController(_mockRepository.Object);

            var response = await _controller.GetUserIdByUsername(_helper.standardUser.UserName);
            var result = (OkObjectResult)response;

            var returnedId = result.Value;
            returnedId.ShouldBe(_helper.standardUser.Id);
        }
    }
}
