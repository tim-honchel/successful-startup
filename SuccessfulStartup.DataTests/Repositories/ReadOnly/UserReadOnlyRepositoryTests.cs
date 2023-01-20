using GenFu; // for generating mock data
using Microsoft.EntityFrameworkCore; // for DbContextOptionsBuilder
using Moq; // for Mock, Setup
using Moq.EntityFrameworkCore; // for ReturnsDbSet
using Shouldly; // for assertion
using SuccessfulStartup.Data.Authentication;
using SuccessfulStartup.Data.Contexts;
using SuccessfulStartup.Data.Repositories.ReadOnly;
using SuccessfulStartup.Domain.Repositories.ReadOnly;

namespace SuccessfulStartup.DataTests.Repositories.ReadOnly
{
    internal class UserReadOnlyRepositoryTests
    {
        private Mock<AuthenticationDbContextFactory> _mockFactory;
        public IUserReadOnlyRepository _repository;
        private Mock<AuthenticationDbContext> _mockContext;

        [OneTimeSetUp] // runs one time, prior to all the tests
        public void OneTimeSetup()
        {
            _mockFactory = new Mock<AuthenticationDbContextFactory>(); // mock factory so the real database is not used
            _repository = new UserReadOnlyRepository(_mockFactory.Object);
            _mockContext = new Mock<AuthenticationDbContext>(new DbContextOptionsBuilder<AuthenticationDbContext>().Options, "dummyConnectionString"); //  fulfills required parameters
            _mockFactory.Setup(mockedFactory => mockedFactory.CreateDbContext()).Returns(_mockContext.Object); // factory will return the mock context instead of the real one
        }

        [Test]
        public async Task GetUserIdByUsernameAsync_ReturnsMatchingId_GivenExistingUsername()
        {
            var savedUsers = A.ListOf<AppUser>(5); // generates list of 5 Identity users
            savedUsers[0].UserName = "existingUsername";
            savedUsers[0].Id = "idThatMatchesUsername";
            _mockContext.Setup(context => context.Users).ReturnsDbSet(savedUsers);

            var id = await _repository.GetUserIdByUsernameAsync("existingUsername");

            id.ShouldBe("idThatMatchesUsername");
        }

        [Test]
        public async Task GetUserIdByUsernameAsync_ThrowsNullReferenceException_GivenNonexistentUsername()
        {
            var savedUsers = A.ListOf<AppUser>(5); // generates list of 25 Identity users
            _mockContext.Setup(context => context.Users).ReturnsDbSet(savedUsers);

            Should.Throw<NullReferenceException>(async () => await _repository.GetUserIdByUsernameAsync("nonexistentUsername"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("     ")]
        public async Task GetUserIdByUsernameAsync_ThrowsArgumentNullException_GivenInvalidUsername(string? username)
        {
            Should.Throw<ArgumentNullException>(async () => await _repository.GetUserIdByUsernameAsync(username));
        }
    }
}
