// Overloaded method converts any type of domain or data entity; removes the need of presentation layer to use mapper and access domain layer

using AutoMapper; // for IMapper
using SuccessfulStartup.Data.Entities;
using SuccessfulStartup.Domain.Entities;

namespace SuccessfulStartup.Data.Mapping
{
    public class EntityConverter
    {
        private IMapper _mapper; 

        public EntityConverter(IMapper mapper) // Autopapper injected from configuration
        {
            _mapper = mapper;
        }

        public BusinessPlan Convert(BusinessPlanDomain planDomain)
        {
            return _mapper.Map<BusinessPlan>(planDomain);
        }

        public BusinessPlanDomain Convert(BusinessPlan plan)
        {
            return _mapper.Map<BusinessPlanDomain>(plan);
        }

        public List<BusinessPlan> Convert(List<BusinessPlanDomain> plansDomain)
        {
            return _mapper.Map<List<BusinessPlan>>(plansDomain);
        }

        public List<BusinessPlanDomain> Convert(List<BusinessPlan> plans)
        {
            return _mapper.Map<List<BusinessPlanDomain>>(plans);
        }

    }
}
