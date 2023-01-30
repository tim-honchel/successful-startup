using AutoMapper; // for IMapper
using GenFu; // for generating mock data
using Shouldly; // for assertion
using SuccessfulStartup.Data.Entities;
using SuccessfulStartup.Api.Mapping;
using SuccessfulStartup.Api.ViewModels;


namespace SuccessfulStartup.ApiTests.Mapping
{
    [TestFixture]
    internal class BusinessPlanViewModelMappingProfileTests
    {
        private IMapper _mapper = AllViewModelsMappingProfiles.GetMapper();
        private BusinessPlanViewModel _planViewModel;
        private BusinessPlan _planData;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _planViewModel = A.New<BusinessPlanViewModel>();
            _planData = A.New<BusinessPlan>();
        }

        [Test]
        public void CreateMapper_ReturnsMapperThatInheritsFromIMapper()
        {
            _mapper.ShouldBeAssignableTo<IMapper>();
        }

        [Test]
        public void Map_ReturnsEquivalentDataEntity_GivenDomainEntity()
        {
            var mappedPlanData = _mapper.Map<BusinessPlan>(_planViewModel);
            mappedPlanData.ShouldSatisfyAllConditions(
                () => mappedPlanData.Id.ShouldBe(_planViewModel.Id),
                () => mappedPlanData.Name.ShouldBe(_planViewModel.Name),
                () => mappedPlanData.Description.ShouldBe(_planViewModel.Description),
                () => mappedPlanData.AuthorId.ShouldBe(_planViewModel.AuthorId)
                );
        }

        [Test]
        public void Map_ReturnsEquivalentDomainEntity_GivenDataEntity()
        {
            var mappedPlanDomain = _mapper.Map<BusinessPlanViewModel>(_planData);
            mappedPlanDomain.ShouldSatisfyAllConditions(
                () => mappedPlanDomain.Id.ShouldBe(_planData.Id),
                () => mappedPlanDomain.Name.ShouldBe(_planData.Name),
                () => mappedPlanDomain.Description.ShouldBe(_planData.Description),
                () => mappedPlanDomain.AuthorId.ShouldBe(_planData.AuthorId)
                );
        }
    }
}

