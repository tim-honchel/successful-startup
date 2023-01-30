using Microsoft.AspNetCore.ApiAuthorization.IdentityServer; // for ApiAuthorizationDbContext
using Microsoft.EntityFrameworkCore; // for context members
using Shouldly; // for assertion
using SuccessfulStartup.Data.Authentication;
using SuccessfulStartup.Data.Contexts;

namespace SuccessfulStartup.DataTests.Contexts
{
    [TestFixture]
    internal class AuthenticationDbContextFactoryTests
    {
        private AuthenticationDbContextFactory _factory = new();
        private AuthenticationDbContext _contextWithArguments;
        private AuthenticationDbContext _contextWithoutArguments;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _contextWithArguments = _factory.CreateDbContext(new string[] { });
            _contextWithoutArguments = _factory.CreateDbContext();
        }

        [Test]
        public void GetsConnectionStringFromAppSettings()
        {
            AuthenticationDbContextFactory._connectionString.ShouldNotBeNullOrWhiteSpace();
        }

        [Test]
        public void CreateDbContextWithArguments_ReturnsAuthenticationDbContext()
        {
            _contextWithArguments.ShouldBeOfType<AuthenticationDbContext>();
        }

        [Test]
        public void CreateDbContextWithoutArguments_ReturnsAuthenticationDbContext()
        {
            _contextWithoutArguments.ShouldBeOfType<AuthenticationDbContext>();
        }

        [Test]
        public void CreateDbContextWithAndWithoutArguments_ReturnEquivalentAuthenticationDbContext() // the default method requires arguments; I created an overload with no arguments for simplicity
        {
            _contextWithArguments.ShouldSatisfyAllConditions(
                () => _contextWithArguments.Database.ShouldBeEquivalentTo(_contextWithoutArguments.Database),
                () => _contextWithArguments.Model.ShouldBeEquivalentTo(_contextWithoutArguments.Model),
                () => _contextWithArguments.BusinessPlans.ShouldBeEquivalentTo(_contextWithoutArguments.BusinessPlans),
                () => _contextWithArguments.Users.ShouldBeEquivalentTo(_contextWithoutArguments.Users)
                );    
        }

        [Test]
        public void CreateDbContext_ReturnsContextThatInheritsFromApiAuthorizationDbContext()
        {
            _contextWithoutArguments.ShouldBeAssignableTo<ApiAuthorizationDbContext<AppUser>>();
        }

        [Test]
        public void CreateDbContext_ReturnsContextWithSqlServerConnection()
        {
            _contextWithoutArguments.Database.IsSqlServer().ShouldBeTrue();
        }

    }
}
