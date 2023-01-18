using AutoMapper; // for IMapper
using Microsoft.EntityFrameworkCore; // for DbContextOptionsBuilder
using Moq; // for Mock
using SuccessfulStartup.Data.Contexts;
using SuccessfulStartup.Data.Mapping;
using SuccessfulStartup.Data.Repositories.ReadOnly;
using SuccessfulStartup.Domain.Repositories.ReadOnly;

namespace SuccessfulStartup.DataTests.Repositories.ReadOnly
{
    [TestFixture]
    internal class BusinessPlanReadOnlyRepositoryTests
    {
        private Mock<AuthenticationDbContextFactory> _mockFactory;
        private IMapper _mapper;
        public IBusinessPlanReadOnlyRepository _repository;
        private Mock<AuthenticationDbContext> _mockContext;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mockFactory = new Mock<AuthenticationDbContextFactory>();
            _mapper = AllMappingProfiles.GetMapper();
            _repository = new BusinessPlanReadOnlyRepository(_mockFactory.Object, _mapper);
            _mockContext = new Mock<AuthenticationDbContext>(new DbContextOptionsBuilder<AuthenticationDbContext>().Options,"dummyConnectionString");
            _mockFactory.Setup(mockedFactory => mockedFactory.CreateDbContext(new string[] { })).Returns(_mockContext.Object);
        }

        [Test]
        public void GetAllPlansByAuthorId_ReturnsMatchingPlans_GivenAuthorIDWithPlans()
        {
            _repository.GetAllPlansByAuthorIdAsync("authorIdWithMatches");
            Assert.Fail();
        }

        [Test]
        public void GetAllPlansByAuthorId_ReturnsEmptyList_GivenAuthorIdWithNoPlans()
        {
            Assert.Ignore();
        }

        [Test]
        public void GetAllPlansByAuthorId_ThrowsException_GivenNonexistentAuthorId()
        {
            Assert.Ignore();
        }

        [Test]
        public void GetAllPlansByAuthorId_ThrowsException_GivenInvalidAuthorId()
        {
            Assert.Ignore();
        }
    }
}
