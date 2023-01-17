using AutoMapper;
using SuccessfulStartup.Data.Entities;
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
