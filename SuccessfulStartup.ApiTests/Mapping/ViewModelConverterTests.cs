using GenFu; // for generating mock data
using Shouldly; // for assertion
using SuccessfulStartup.Api.Mapping;
using SuccessfulStartup.Api.ViewModels;
using SuccessfulStartup.Data.Entities;

namespace SuccessfulStartup.ApiTests.Mapping
{
    [TestFixture]
    internal class ViewModelConverterTests
    {
        private ViewModelConverter _converter = new(AllViewModelsMappingProfiles.GetMapper());

        [Test]
        public void Convert_ReturnsPlan_GivenPlanViewModel() 
        {
            var planViewModel = A.New<BusinessPlanViewModel>();
            var conversion = _converter.Convert(planViewModel);
            conversion.ShouldBeOfType<BusinessPlan>();
        }

        [Test]
        public void Convert_ReturnsPlanViewModel_GivenPlan()
        {
            var plan = A.New<BusinessPlan>();
            var conversion = _converter.Convert(plan);
            conversion.ShouldBeOfType<BusinessPlanViewModel>();
        }

        [Test]
        public void Convert_ReturnsListOfPlans_GivenListOfPlanViewModels()
        {
            var plansViewModel = A.ListOf<BusinessPlanViewModel>(5);
            var conversion = _converter.Convert(plansViewModel);
            conversion.ShouldBeOfType<List<BusinessPlan>>();
        }

        [Test]
        public void Convert_ReturnsListofPlanViewModels_GivenListOfPlans()
        {
            var plans = A.ListOf<BusinessPlan>(5);
            var conversion = _converter.Convert(plans);
            conversion.ShouldBeOfType<List<BusinessPlanViewModel>>();
        }
    }
}
