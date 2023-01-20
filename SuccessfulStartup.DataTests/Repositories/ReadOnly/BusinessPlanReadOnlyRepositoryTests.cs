using AutoMapper; // for IMapper
using GenFu; // for populating mock database with "A"
using Microsoft.EntityFrameworkCore; // for DbContextOptionsBuilder
using Moq; // for Mock
using Moq.EntityFrameworkCore; // for ReturnsDbSet
using Shouldly; // for assertions
using SuccessfulStartup.Data.Authentication;
using SuccessfulStartup.Data.Contexts;
using SuccessfulStartup.Data.Entities;
using SuccessfulStartup.Data.Mapping;
using SuccessfulStartup.Data.Repositories.ReadOnly;
using SuccessfulStartup.Domain.Repositories.ReadOnly;
using System.Runtime.CompilerServices;

namespace SuccessfulStartup.DataTests.Repositories.ReadOnly
{
    [TestFixture]
    internal class BusinessPlanReadOnlyRepositoryTests
    {
        private Mock<AuthenticationDbContextFactory> _mockFactory;
        private IMapper _mapper;
        public IBusinessPlanReadOnlyRepository _repository;
        private Mock<AuthenticationDbContext> _mockContext;

        [OneTimeSetUp] // runs one time, prior to all the tests
        public void OneTimeSetup()
        {
            _mockFactory = new Mock<AuthenticationDbContextFactory>(); // mock factory so the real database is not used
            _mapper = AllMappingProfiles.GetMapper();
            _repository = new BusinessPlanReadOnlyRepository(_mockFactory.Object, _mapper);
            _mockContext = new Mock<AuthenticationDbContext>(new DbContextOptionsBuilder<AuthenticationDbContext>().Options,"dummyConnectionString"); //  fulfills required parameters
            _mockFactory.Setup(mockedFactory => mockedFactory.CreateDbContext()).Returns(_mockContext.Object); // factory will return the mock context instead of the real one
        }

        [Test]
        public async Task GetAllPlansByAuthorId_ReturnsMatchingPlans_GivenAuthorIDWithPlans()
        {
            A.Configure<BusinessPlan>().Fill(plan => plan.AuthorId, () => { return "authorIdWithMatches"; }); // all generated plans will have this AuthorId
            var plansMatchingAuthorId = A.ListOf<BusinessPlan>(5); // generates a list of business plans, 25 by default, but 5 in this case
            A.Configure<BusinessPlan>().Fill(plan => plan.AuthorId, () => { return "differentAuthorId"; }); // changes AuthorId of generated business plans
            var plansByOtherAuthor = A.ListOf<BusinessPlan>(5); // generates 5 more
            List<BusinessPlan> allPlans = plansMatchingAuthorId.Concat(plansByOtherAuthor).ToList(); // combines the two lists
            _mockContext.Setup(context => context.BusinessPlans).ReturnsDbSet(allPlans); // instead of returning the actual dataset, queries will return the combined list

            var plansReturned = _mapper.Map<List<BusinessPlan>>(await _repository.GetAllPlansByAuthorIdAsync("authorIdWithMatches"));

            plansReturned.ShouldBeEquivalentTo(plansMatchingAuthorId);
        }

        [Test]
        public async Task GetAllPlansByAuthorId_ReturnsEmptyList_GivenAuthorIdWithNoPlans()
        {
            A.Configure<BusinessPlan>().Fill(plan => plan.AuthorId, () => { return "differentAuthorId"; });
            var plansByOtherAuthor = A.ListOf<BusinessPlan>(5);
            _mockContext.Setup(context => context.BusinessPlans).ReturnsDbSet(plansByOtherAuthor);

            var plansReturned = _mapper.Map<List<BusinessPlan>>(await _repository.GetAllPlansByAuthorIdAsync("authorIdWithNoMatches"));

            plansReturned.ShouldBeEmpty();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("     ")]
        public async Task GetAllPlansByAuthorId_ThrowsArgumentNullException_GivenInvalidAuthorId(string? authorId) // authorId represents the different TestCases
        {
            Should.Throw<ArgumentNullException>(async () => await _repository.GetAllPlansByAuthorIdAsync(authorId));
        }



        [Test]
        public async Task GetPlanByIdAsync_ReturnsMatchingPlan_GivenExistingId()
        {
            A.Configure<BusinessPlan>().Fill(plan => plan.Id).WithinRange(1, 999999); // ensure plans have unique Ids
            var savedPlans = A.ListOf<BusinessPlan>(5);
            var matchingPlan = savedPlans[0];
            _mockContext.Setup(context => context.BusinessPlans).ReturnsDbSet(savedPlans);

            var returnedPlan = _mapper.Map<BusinessPlan>(await _repository.GetPlanByIdAsync(matchingPlan.Id));

            returnedPlan.ShouldBeEquivalentTo(matchingPlan);
        }

        [Test]
        public async Task GetPlanByIdAsync_ThrowsNullReferenceException_GivenNonexistentId()
        {
            var nonexistentId = 1;
            A.Configure<BusinessPlan>().Fill(plan => plan.Id).WithinRange(2, 10); // no Ids generated will be equal to 1
            var savedPlans = A.ListOf<BusinessPlan>(5);
            _mockContext.Setup(context => context.BusinessPlans).ReturnsDbSet(savedPlans);

            Should.Throw<NullReferenceException>(async () => await _repository.GetPlanByIdAsync(nonexistentId));
        }

        [TestCase(0)]
        public async Task GetPlanByIdAsync_ThrowsArgumentNullException_GivenInvalidParameter(int planId)
        {
            Should.Throw<ArgumentNullException>(async () => await _repository.GetPlanByIdAsync(planId));
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
            var savedUsers = A.ListOf<AppUser>(); // generates list of 25 Identity users

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
