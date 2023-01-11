

namespace SuccessfulStartup.Domain.Entities
{
    public abstract class BusinessPlanAbstract // basic blueprint that all database models must follow
    {
        protected abstract int Id { get; set; }
        protected abstract string Name { get; set; }
        protected abstract string? Description { get; set; }
        protected abstract int AuthorId { get; set; }

    }
}
