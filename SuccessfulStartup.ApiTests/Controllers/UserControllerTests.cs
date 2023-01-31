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
        private ApiHelper _helper = new();
        private Mock<IUserReadOnlyRepository> _mockRepository = new();
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

        [Test]
        public async Task GetUserIdByUserName_Returns200CodeWithObject_GivenExistingUsername()
        {
            _mockRepository.Setup(repository => repository.GetUserIdByUsernameAsync(_helper.standardUser.UserName)).ReturnsAsync(_helper.standardUser.Id);
            _controller = new UserController(_mockRepository.Object);

            var response = await _controller.GetUserIdByUsername(_helper.standardUser.UserName);
            response.ShouldBeOfType<OkObjectResult>();
        }

        [Test]
        public async Task GetUserIdByUserName_Returns400CodeWithObject_GivenInvalidUsername()
        {
            _mockRepository.Setup(repository => repository.GetUserIdByUsernameAsync(null)).Throws<ArgumentNullException>();
            _controller = new UserController(_mockRepository.Object);

            var response = await _controller.GetUserIdByUsername(null);
            response.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task GetUserIdByUserName_Returns404Code_GivenNonexistentUsername()
        {
            _mockRepository.Setup(repository => repository.GetUserIdByUsernameAsync("nonexistent-user")).Throws<NullReferenceException>();
            _controller = new UserController(_mockRepository.Object);

            var response = await _controller.GetUserIdByUsername("nonexistent-user");
            response.ShouldBeOfType<NotFoundObjectResult>();
        }


    }
}
