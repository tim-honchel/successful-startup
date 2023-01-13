using SuccessfulStartup.Data.Entities;
using SuccessfulStartup.Domain.Entities;

namespace SuccessfulStartup.Data
{
    public class MappingProfile // map data and domain objects TODO: use AutoMapper instead
    {
        public BusinessPlanDomain BusinessPlanDataToDomain(BusinessPlan plan)
        {
            var planDomain = new BusinessPlanDomain()
            {
                Id = plan.Id,
                Name = plan.Name,
                Description= plan.Description,
                AuthorId= plan.AuthorId
            };
            return planDomain;
        }

        public BusinessPlan BusinessPlanDomainToData(BusinessPlanDomain planDomain)
        {
            var plan = new BusinessPlan()
            {
                Id = planDomain.Id,
                Name = planDomain.Name,
                Description = planDomain.Description,
                AuthorId = planDomain.AuthorId
            };
            return plan;
        }

        public List<BusinessPlanDomain> ListBusinessPlanDataToDomain(List<BusinessPlan> plans)
        {
            var plansDomain = new List<BusinessPlanDomain>();
            foreach (var plan in plans)
            {
                plansDomain.Add(BusinessPlanDataToDomain(plan));
            }
            return plansDomain;         
        }

        public List<BusinessPlan> ListBusinessPlanDomainToData(List<BusinessPlanDomain> plansDomain)
        {
            var plans = new List<BusinessPlan>();
            foreach (var plan in plansDomain)
            {
                plans.Add(BusinessPlanDomainToData(plan));
            }
            return plans;
        }
    }
}
