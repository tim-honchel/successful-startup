using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration; // for ConfigurationBuilder
using Shouldly; // for assertion
using SuccessfulStartup.Data.Authentication;
using SuccessfulStartup.Data.Contexts;

namespace SuccessfulStartup.DataTests.Contexts
{
    [TestFixture]
    internal class AuthenticationDbContextFactoryTests
    {
        private AuthenticationDbContextFactory _factory;
        private AuthenticationDbContext _contextWithArguments;
        private AuthenticationDbContext _contextWithoutArguments;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _factory = new AuthenticationDbContextFactory();
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
        public void CreateDbContextWithAndWithoutArguments_ReturnEquivalentAuthenticationDbContext()
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
