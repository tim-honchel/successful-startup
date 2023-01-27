using AutoMapper; // for IMapper
using GenFu; // for generating mock data
using Shouldly; // for assertion
using SuccessfulStartup.Api.Mapping;
using SuccessfulStartup.Api.ViewModels;
using SuccessfulStartup.Data.Entities;

namespace SuccessfulStartup.ApiTests.Mapping
{
    [TestFixture]
    internal class AllViewModelsMappingProfilesTests
    {
        private IMapper _mapper;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mapper = AllViewModelsMappingProfiles.GetMapper();
        }

        [Test]
        public void Configuration_ContainsBusinessPlanMappingProfile()
        {

            Should.NotThrow(() => _mapper.Map<BusinessPlan>(A.New<BusinessPlanViewModel>()));
        }
    }
}
