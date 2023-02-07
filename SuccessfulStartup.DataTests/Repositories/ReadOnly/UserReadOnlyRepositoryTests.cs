using GenFu;
using Moq;
using Moq.EntityFrameworkCore; 
using Shouldly;
using SuccessfulStartup.Data.Contexts;
using SuccessfulStartup.Data.Entities;
using SuccessfulStartup.Data.Repositories.ReadOnly;
using SuccessfulStartup.Domain.Repositories.ReadOnly;

namespace SuccessfulStartup.DataTests.Repositories.ReadOnly
{
    [TestFixture]
    internal class UserReadOnlyRepositoryTests
    {
        private Mock<PlanDbContextFactory> _mockFactory;
        private Mock<PlanDbContext> _mockContext;
        private IUserReadOnlyRepository _repository;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mockFactory = new Mock<PlanDbContextFactory>(); // mock factory so the real database is not used
            _mockContext = new Mock<PlanDbContext>(true); //  fulfills required parameters
            _mockFactory.Setup(mockedFactory => mockedFactory.CreateDbContext()).Returns(_mockContext.Object); // factory will return the mock context instead of the real one
            _repository = new UserReadOnlyRepository(_mockFactory.Object);
        }

        [Test]
        public async Task VerifyUser_ReturnsFalse_GivenUnmatchingPlanId()
        {
            var plans = A.ListOf<BusinessPlan>(5);
            var users = A.ListOf<User>(5);
            var unmatchingSecurityStamp = "no-match";
            _mockContext.Setup(context => context.BusinessPlans).ReturnsDbSet(plans);
            _mockContext.Setup(context => context.Users).ReturnsDbSet(users);

            var userVerified = await _repository.VerifyUserAsync(plans[0].Id, unmatchingSecurityStamp);

            userVerified.ShouldBeFalse();
        }

        [Test]
        public async Task VerifyUser_ReturnsFalse_GivenUnmatchingUserId()
        {
            var users = A.ListOf<User>(5);
            var unmatchingSecurityStamp = "no-match";
            _mockContext.Setup(context => context.Users).ReturnsDbSet(users);

            var userVerified = await _repository.VerifyUserAsync(users[0].AuthorId, unmatchingSecurityStamp);

            userVerified.ShouldBeFalse();
        }

        [Test]
        public async Task VerifyUser_ReturnsTrue_GivenMatchingPlanId()
        {
            var plans = A.ListOf<BusinessPlan>(5);
            var users = A.ListOf<User>(5);
            var matchingSecurityStamp = "match";
            plans[0].AuthorId = users[0].AuthorId;
            users[0].SecurityStamp = matchingSecurityStamp;
            _mockContext.Setup(context => context.BusinessPlans).ReturnsDbSet(plans);
            _mockContext.Setup(context => context.Users).ReturnsDbSet(users);

            var userVerified = await _repository.VerifyUserAsync(plans[0].Id, matchingSecurityStamp);

            userVerified.ShouldBeTrue();
        }

        [Test]
        public async Task VerifyUser_ReturnsTrue_GivenMatchingUserId()
        {
            var users = A.ListOf<User>(5);
            var matchingSecurityStamp = "match";
            users[0].SecurityStamp = matchingSecurityStamp;
            _mockContext.Setup(context => context.Users).ReturnsDbSet(users);

            var userVerified = await _repository.VerifyUserAsync(users[0].AuthorId, matchingSecurityStamp);

            userVerified.ShouldBeTrue();
        }

        [Test]
        public async Task VerifyUser_ThrowsArgumentNullException_GivenInvalidInput()
        {
            var users = A.ListOf<User>(5);
            _mockContext.Setup(context => context.Users).ReturnsDbSet(users);

            Should.Throw<ArgumentNullException>(async () => await _repository.VerifyUserAsync("userId", ""));
        }

        [Test]
        public async Task VerifyUser_ThrowsNullReferenceException_GivenNonexistenPlanId()
        {
            var plans = A.ListOf<BusinessPlan>(5);
            var users = A.ListOf<User>(5);
            var matchingSecurityStamp = "match";
            _mockContext.Setup(context => context.BusinessPlans).ReturnsDbSet(plans);
            _mockContext.Setup(context => context.Users).ReturnsDbSet(users);

            Should.Throw<NullReferenceException>(async () => await _repository.VerifyUserAsync(999999, "key"));
        }
    }
}
