using AutoMapper;
using SuccessfulStartup.Api.ViewModels;
using SuccessfulStartup.Data.Entities;

namespace SuccessfulStartup.Api.Mapping
{
    public class BusinessPlanViewModelMappingProfile : Profile
    {
        public BusinessPlanViewModelMappingProfile()
        {
            AllowNullDestinationValues = true;
            CreateMap<BusinessPlanViewModel, BusinessPlan>().ReverseMap();
        }
    }
}
