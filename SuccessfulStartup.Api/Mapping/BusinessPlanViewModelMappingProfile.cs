﻿using AutoMapper;
using SuccessfulStartup.Api.ViewModels;
using SuccessfulStartup.Data.Entities;

namespace SuccessfulStartup.Api.Mapping
{
    internal class BusinessPlanViewModelMappingProfile : Profile
    {
        public BusinessPlanViewModelMappingProfile()
        {
            AllowNullDestinationValues = true;
            CreateMap<BusinessPlanViewModel, BusinessPlan>().ReverseMap();
        }
    }
}
