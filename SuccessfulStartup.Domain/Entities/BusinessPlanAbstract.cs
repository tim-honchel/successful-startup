

namespace SuccessfulStartup.Domain.Entities
{
    internal abstract class BusinessPlanAbstract
    {
        protected abstract int Id { get; set; }
        protected abstract string Name { get; set; }
        protected abstract string Description { get; set; }

    }
}
