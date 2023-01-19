using Shouldly;
using SuccessfulStartup.Data.Contexts;

namespace SuccessfulStartup.DataTests.Contexts
{
    [TestFixture]
    internal class AuthenticationDbContextTests
    {
        private AuthenticationDbContextFactory _factory;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _factory = new AuthenticationDbContextFactory();
        }

        [Test]
        public void Database_CanConnect()
        {
            using var context = _factory.CreateDbContext();
            context.Database.CanConnect().ShouldBeTrue();
        }
    }
}
