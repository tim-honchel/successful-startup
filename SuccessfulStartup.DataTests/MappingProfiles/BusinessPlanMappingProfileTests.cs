using AutoMapper; // for MapperConfiguration
using GenFu;
using Shouldly;
using SuccessfulStartup.Data.Entities;
using SuccessfulStartup.Data.Mapping;
using SuccessfulStartup.Domain.Entities;



namespace SuccessfulStartup.DataTests.MappingProfiles
{
    [TestFixture]
    internal class BusinessPlanMappingProfileTests
    {
        private MapperConfiguration _businessPlanConfiguration;
        private IMapper _mapper;
        private BusinessPlanDomain _planDomain;
        private BusinessPlan _planData;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _businessPlanConfiguration = new MapperConfiguration(configuration => configuration.AddProfile<BusinessPlanMappingProfile>());
            _mapper = _businessPlanConfiguration.CreateMapper();
            _planDomain = A.New<BusinessPlanDomain>();
            _planData = A.New<BusinessPlan>();
        }

        [Test]
        public void MapperConfiguration_IsValid()
        {
            _businessPlanConfiguration.AssertConfigurationIsValid();
        }

        [Test]
        public void CreateMapper_ReturnsMapperThatInheritsFromIMapper()
        {
            _mapper.ShouldBeAssignableTo<IMapper>();
        }

        [Test]
        public void Map_ReturnsEquivalentDataEntity_GivenDomainEntity()
        {
            var mappedPlanData = _mapper.Map<BusinessPlan>(_planDomain);
            mappedPlanData.ShouldSatisfyAllConditions(
                () => mappedPlanData.Id.ShouldBe(_planDomain.Id),
                () => mappedPlanData.Name.ShouldBe(_planDomain.Name),
                () => mappedPlanData.Description.ShouldBe(_planDomain.Description),
                () => mappedPlanData.AuthorId.ShouldBe(_planDomain.AuthorId)
                );
        }

        [Test]
        public void Map_ReturnsEquivalentDomainEntity_GivenDataEntity()
        {
            var mappedPlanDomain = _mapper.Map<BusinessPlanDomain>(_planData);
            mappedPlanDomain.ShouldSatisfyAllConditions(
                () => mappedPlanDomain.Id.ShouldBe(_planData.Id),
                () => mappedPlanDomain.Name.ShouldBe(_planData.Name),
                () => mappedPlanDomain.Description.ShouldBe(_planData.Description),
                () => mappedPlanDomain.AuthorId.ShouldBe(_planData.AuthorId)
                );
        }
    }
}
