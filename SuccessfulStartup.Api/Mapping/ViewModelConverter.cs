using AutoMapper; // for IMapper
using SuccessfulStartup.Data.Entities;

namespace SuccessfulStartup.Api.Mapping
{
    public class ViewModelConverter // Overloaded method converts any type of API or data entity; removes the need of presentation layer to use mapper and access domain layer
    {
        private readonly IMapper _mapper; 

        public ViewModelConverter(IMapper mapper) // Automapper injected from configuration
        {
            _mapper = mapper;
        }

        public BusinessPlanViewModel Convert(BusinessPlan plan)
        {
            return _mapper.Map<BusinessPlanViewModel>(plan);
        }

        public BusinessPlan Convert(BusinessPlanViewModel plan)
        {
            return _mapper.Map<BusinessPlan>(plan);
        }

        public List<BusinessPlanViewModel> Convert(List<BusinessPlan> plans)
        {
            return _mapper.Map<List<BusinessPlanViewModel>>(plans);
        }

        public List<BusinessPlan> Convert(List<BusinessPlanViewModel> plans)
        {
            return _mapper.Map<List<BusinessPlan>>(plans);
        }

    }
}
