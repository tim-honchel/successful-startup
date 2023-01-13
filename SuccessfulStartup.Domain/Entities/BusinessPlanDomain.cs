namespace SuccessfulStartup.Domain.Entities
{
    public class BusinessPlanDomain // basic blueprint that all database models must follow
    {
        public int Id { get; set; } // primary key
        public string Name { get; set; }
        public string Description { get; set; }
        public string AuthorId { get; set; } // foreign key that links to an Identity user

    }
}
