using GenFu;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using Shouldly;
using SuccessfulStartup.Data.Contexts;
using SuccessfulStartup.Data.Entities;
using SuccessfulStartup.Data.Repositories.WriteOnly;
using SuccessfulStartup.Domain.Entities;
using SuccessfulStartup.Domain.Repositories.WriteOnly;

namespace SuccessfulStartup.DataTests.Repositories.WriteOnly
{
    [TestFixture]
    internal class UserWriteOnlyRepositoryTests
    {

        private Mock<PlanDbContextFactory> _mockFactory;
        private Mock<PlanDbContext> _mockContext;
        private IUserWriteOnlyRepository _repository;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mockFactory = new Mock<PlanDbContextFactory>(); // mock factory so the real database is not used
            _mockContext = new Mock<PlanDbContext>(true); //  fulfills required parameters
            _mockFactory.Setup(mockedFactory => mockedFactory.CreateDbContext()).Returns(_mockContext.Object); // factory will return the mock context instead of the real one
            _repository = new UserWriteOnlyRepository(_mockFactory.Object);
        }

        [Test]
        public async Task AddUserAsync_SavesToDatabase_GivenValidInput()
        {
            var users = A.ListOf<User>(5);
            _mockContext.Setup(context => context.Users).ReturnsDbSet(users);
            _mockContext.Setup(context => context.SaveChangesAsync(default)); // clears previous setups, returns void

            await _repository.AddUserAsync("userId","security-stamp");

            _mockContext.Verify(context => context.AddAsync(It.Is<User>(user => user.AuthorId == "userId"), new CancellationToken()), Times.Once()); // verifies that the context added the specific user, only one time
        }


        [Test]
        public async Task AddUserAsync_ThrowsArgumentNullException_GivenInvalidInput()
        {
            var users = A.ListOf<User>(5);
            _mockContext.Setup(context => context.Users).ReturnsDbSet(users);
            _mockContext.Setup(context => context.SaveChangesAsync(default)); // clears previous setups, returns void

            Should.Throw<ArgumentNullException>(async ()=> await _repository.AddUserAsync("userId", ""));
        }

        [Test]
        public async Task AddUserAsync_ThrowsDbUpdateException_GivenDatabaseError()
        {
            var users = A.ListOf<User>(5);
            _mockContext.Setup(context => context.Users).ReturnsDbSet(users);
            _mockContext.Setup(context => context.SaveChangesAsync(default)).Throws<DbUpdateException>(); // clears previous setups, returns void

            Should.Throw<DbUpdateException>(async () => await _repository.AddUserAsync("userId", "security-stamp"));
        }

        [Test]
        public async Task AddUserAsync_ThrowsInvalidOperationException_GivenExistingUser()
        {
            var users = A.ListOf<User>(5);
            users[0].AuthorId = "userId";
            _mockContext.Setup(context => context.Users).ReturnsDbSet(users);
            _mockContext.Setup(context => context.SaveChangesAsync(default)); // clears previous setups, returns void

            Should.Throw<InvalidOperationException>(async () => await _repository.AddUserAsync("userId", "security-stamp"));
        }
    }
}
