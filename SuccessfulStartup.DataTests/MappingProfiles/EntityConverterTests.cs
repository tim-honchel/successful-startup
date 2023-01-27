using GenFu; // for generating mock data
using Shouldly; // for assertion
using SuccessfulStartup.Data.Entities;
using SuccessfulStartup.Data.Mapping;
using SuccessfulStartup.Domain.Entities;

namespace SuccessfulStartup.DataTests.MappingProfiles
{
    internal class EntityConverterTests
    {
        private EntityConverter _converter = new(AllMappingProfiles.GetMapper());

        [Test]
        public void Convert_ReturnsPlan_GivenPlanDomain()
        {
            var planDomain = A.New<BusinessPlanDomain>();
            var conversion = _converter.Convert(planDomain);
            conversion.ShouldBeOfType<BusinessPlan>();
        }

        [Test]
        public void Convert_ReturnsPlanDomain_GivenPlan()
        {
            var plan = A.New<BusinessPlan>();
            var conversion = _converter.Convert(plan);
            conversion.ShouldBeOfType<BusinessPlanDomain>();
        }

        [Test]
        public void Convert_ReturnsListOfPlans_GivenListOfPlanDomain()
        {
            var plansDomain = A.ListOf<BusinessPlanDomain>(5);
            var conversion = _converter.Convert(plansDomain);
            conversion.ShouldBeOfType<List<BusinessPlan>>();
        }

        [Test]
        public void Convert_ReturnsListofPlanDomain_GivenListOfPlans()
        {
            var plans = A.ListOf<BusinessPlan>(5);
            var conversion = _converter.Convert(plans);
            conversion.ShouldBeOfType<List<BusinessPlanDomain>>();
        }
    }
}
