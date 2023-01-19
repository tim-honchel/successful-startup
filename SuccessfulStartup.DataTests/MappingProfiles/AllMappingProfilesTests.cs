using AutoMapper; // for IMapper
using GenFu; // for generating test objects
using Shouldly; // for assertion
using SuccessfulStartup.Data.Entities;
using SuccessfulStartup.Data.Mapping;
using SuccessfulStartup.Domain.Entities;

namespace SuccessfulStartup.DataTests.MappingProfiles
{
    [TestFixture]
    internal class AllMappingProfilesTests
    {
        private IMapper _mapper;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mapper = AllMappingProfiles.GetMapper();
        }

        [Test]
        public void Configuration_ContainsBusinessPlanMappingProfile()
        {
            
            Should.NotThrow( () => _mapper.Map<BusinessPlan>(A.New<BusinessPlanDomain>()) );
        }
    }
}
