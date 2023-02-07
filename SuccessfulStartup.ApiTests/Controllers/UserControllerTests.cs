using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;
using SuccessfulStartup.Api.Controllers;
using SuccessfulStartup.Data.Entities;
using SuccessfulStartup.Domain.Repositories.ReadOnly;
using SuccessfulStartup.Domain.Repositories.WriteOnly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuccessfulStartup.ApiTests.Controllers
{
    [TestFixture]
    internal class UserControllerTests
    {
        private UserController _controller;
        private Mock<IUserWriteOnlyRepository> _mockRepository = new();

        [Test]
        public async Task AddUser_Returns200Code_GivenNewValidInput()
        {
            var user = new Dictionary<string, string>() { { "userId", "valid" }, { "securityStamp", "valid" } };
            _mockRepository.Setup(repository => repository.AddUserAsync(user["userId"], user["securityStamp"]));
            _controller = new(_mockRepository.Object);

            var response = await _controller.AddUser(user);

            response.ShouldBeOfType<OkResult>();
        }


        [Test]
        public async Task AddUser_Returns204Code_GivenDatabaseError()
        {
            var user = new Dictionary<string, string>() { { "userId", "valid" }, { "securityStamp", "valid" } };
            _mockRepository.Setup(repository => repository.AddUserAsync(user["userId"], user["securityStamp"])).Throws<DbUpdateException>();
            _controller = new(_mockRepository.Object);

            var response = await _controller.AddUser(user);

            response.ShouldBeOfType<NoContentResult>();
        }

        [Test]
        public async Task AddUser_Returns400Code_GivenInvalidInput()
        {
            var user = new Dictionary<string, string>() { { "userId", "invalid" }, { "securityStamp", "invalid" } };
            _mockRepository.Setup(repository => repository.AddUserAsync(user["userId"], user["securityStamp"])).Throws<ArgumentNullException>();
            _controller = new(_mockRepository.Object);

            var response = await _controller.AddUser(user);

            response.ShouldBeOfType<BadRequestResult>();
        }

        [Test]
        public async Task AddUser_Returns401Code_GivenExistingId()
        {
            var user = new Dictionary<string, string>() { { "userId", "existing" }, { "securityStamp", "existing" } };
            _mockRepository.Setup(repository => repository.AddUserAsync(user["userId"], user["securityStamp"])).Throws<InvalidOperationException>();
            _controller = new(_mockRepository.Object);

            var response = await _controller.AddUser(user);

            response.ShouldBeOfType<UnauthorizedResult>();
        }
    }
}
