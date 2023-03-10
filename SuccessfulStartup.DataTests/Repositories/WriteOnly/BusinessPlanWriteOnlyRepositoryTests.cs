using AutoMapper; // for IMapper
using GenFu; // for generating mock data
using Microsoft.EntityFrameworkCore; // for DbContextOptionsBuilder, DbUpdateException
using Moq; // for Mock, Setup
using Shouldly; // for assertioon
using SuccessfulStartup.Data.Contexts;
using SuccessfulStartup.Data.Entities;
using SuccessfulStartup.Data.Mapping;
using SuccessfulStartup.Data.Repositories.WriteOnly;
using SuccessfulStartup.Domain.Entities;
using SuccessfulStartup.Domain.Repositories.WriteOnly;

namespace SuccessfulStartup.DataTests.Repositories.WriteOnly
{
    public class BusinessPlanWriteOnlyRepositoryTests
    {
        private readonly IMapper _mapper = AllMappingProfiles.GetMapper();
        private Mock<PlanDbContextFactory> _mockFactory;
        private IBusinessPlanWriteOnlyRepository _repository;
        private Mock<PlanDbContext> _mockContext;

        [OneTimeSetUp] // runs one time, prior to all the tests
        public void OneTimeSetup()
        {

            _mockFactory = new Mock<PlanDbContextFactory>(); // mock factory so the real database is not used
            _repository = new BusinessPlanWriteOnlyRepository(_mockFactory.Object, _mapper);
            _mockContext = new Mock<PlanDbContext>(true); //  fulfills required parameters
            _mockFactory.Setup(mockedFactory => mockedFactory.CreateDbContext()).Returns(_mockContext.Object); // factory will return the mock context instead of the real one
        }

        [Test]
        public async Task DeletePlanAsync_RemovesPlanFromDatabase_GivenExistingId()
        {
            var planToDelete = A.New<BusinessPlanDomain>();
            await _repository.DeletePlanAsync(planToDelete);

            _mockContext.Verify(context => context.Remove<BusinessPlan>(It.Is<BusinessPlan>(plan => plan.Id == planToDelete.Id)), Times.Once()); // verifies that the context deleted the specific business plan, only one time
        }

        [Test]
        public async Task DeletePlanAsync_ThrowsDbUpdateException_GivenSaveChangesError() // from error removing plan (i.e. does not exist)
        {
            var planToDelete = A.New<BusinessPlanDomain>();
            _mockContext.Setup(context => context.SaveChanges()).Throws<DbUpdateException>();
            
            Should.Throw<DbUpdateException>( async () => await _repository.DeletePlanAsync(planToDelete));
        }

        public static IEnumerable<BusinessPlanDomain> ProvideCasesOfInvalidBusinessPlans() // for use with TestCaseSource
        {
            yield return new BusinessPlanDomain();
            yield return new BusinessPlanDomain() { Id = 0, Name = "", Description = "valid description", AuthorId = "valid id" };
            yield return new BusinessPlanDomain() { Id = 0, Name = "valid name", Description = "    ", AuthorId = null };
            yield return new BusinessPlanDomain() { Id = 0, Name = "valid name", Description = "valid description", AuthorId = null };
        }

        [Test]
        public async Task SaveNewPlanAsync_ReturnsIdOfNewPlan_GivenSuccessfulSave()
        {
            var planSaved = A.New<BusinessPlan>();
            _mockContext.Setup(context => context.SaveChangesAsync(default)); // clears previous setups, returns void

            var returnedId = await _repository.SaveNewPlanAsync(_mapper.Map<BusinessPlanDomain>(planSaved));

            returnedId.ShouldBe(planSaved.Id);
        }

        [TestCase(null)]
        [TestCaseSource(nameof(ProvideCasesOfInvalidBusinessPlans))]
        public async Task SaveNewPlanAsync_ThrowsArgumentNullException_GivenInvalidPlan(BusinessPlanDomain plan)
        {
            Should.Throw<ArgumentNullException>(async () => await _repository.SaveNewPlanAsync(plan));
        }

        [Test]
        public async Task SaveNewPlanAsync_ThrowsDbUpdateException_GivenSaveChangesError() // from error removing plan (i.e. does not exist)
        {
            var planToSave = A.New<BusinessPlanDomain>();

            _mockContext.Setup(context => context.SaveChangesAsync(default)).Throws<DbUpdateException>(); // default keyword is necessary to avoid error

            Should.Throw<DbUpdateException>(async () => await _repository.SaveNewPlanAsync(planToSave));
        }

        [Test]
        public async Task SaveNewPlanAsync_WritesPlanToDatabase_GivenValidPlan()
        {
            var planSaved = A.New<BusinessPlan>();
            _mockContext.Setup(context => context.SaveChangesAsync(default)); // clears previous setups, returns void

            await _repository.SaveNewPlanAsync(_mapper.Map<BusinessPlanDomain>(planSaved));

            _mockContext.Verify(context => context.AddAsync(It.Is<BusinessPlan>(plan => plan.Id == planSaved.Id), new CancellationToken()), Times.Once()); // verifies that the context added the specific business plan, only one time
        }

        [Test]
        public async Task UpdatePlanAsync_SavesChangesToPlan_GivenExistingPlan()
        {
            var updatedPlan = A.New<BusinessPlanDomain>();
            _mockContext.Setup(context => context.SaveChanges()); // clears previous setups, returns void

            await _repository.UpdatePlanAsync(updatedPlan);

            _mockContext.Verify(context => context.Update<BusinessPlan>(It.Is<BusinessPlan>(plan => plan.Id == updatedPlan.Id)), Times.Once()); // verifies that the context updated the specific business plan, only one time
        }

        [Test]
        public async Task UpdatePlanAsync_ThrowsDbUpdateException_GivenSaveChangesError() // from error removing plan (i.e. does not exist)
        {
            var planToDelete = A.New<BusinessPlanDomain>();
            _mockContext.Setup(context => context.SaveChanges()).Throws<DbUpdateException>();

            Should.Throw<DbUpdateException>(async () => await _repository.UpdatePlanAsync(planToDelete));
        }

    }
}
