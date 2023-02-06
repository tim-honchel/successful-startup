namespace SuccessfulStartup.Data.Contexts
{
    public class PlanDbContextFactory
    {
        public virtual PlanDbContext CreateDbContext()
        {
            return new PlanDbContext();
        }
    }
}
