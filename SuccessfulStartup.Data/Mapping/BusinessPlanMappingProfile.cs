using AutoMapper; // for CreateMap
using SuccessfulStartup.Domain.Entities;

namespace SuccessfulStartup.Data.Mapping
{
    internal class BusinessPlanMappingProfile : Profile
    {
        public BusinessPlanMappingProfile()
        {
            AllowNullDestinationValues = true;
            CreateMap<BusinessPlan, BusinessPlanDomain>().ReverseMap();
        }
    }
}
