

namespace SuccessfulStartup.Domain.Entities
{
    public abstract class BusinessPlanAbstract // basic blueprint that all database models must follow
    {
        public abstract int Id { get; set; } // primary key
        public abstract string Name { get; set; }
        public abstract string? Description { get; set; }
        public abstract string AuthorId { get; set; } // foreign key that links to an Identity user

    }
}
