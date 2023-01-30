using AutoMapper;
using SuccessfulStartup.Api.ViewModels;
using SuccessfulStartup.Data.Entities;

namespace SuccessfulStartup.Api.Mapping
{
    public class BusinessPlanViewModelMappingProfile : Profile // mapping profile configures a map between two entity types
    {
        public BusinessPlanViewModelMappingProfile()
        {
            AllowNullDestinationValues = true;
            CreateMap<BusinessPlanViewModel, BusinessPlan>().ReverseMap();
        }
    }
}
