﻿using Microsoft.EntityFrameworkCore; // for DbContextOptionsBuilder
using Microsoft.EntityFrameworkCore.Design; // for IDesignTimeDbContextFactory interface
using Microsoft.Extensions.Configuration; // for accessing connection strings

namespace SuccessfulStartup.Data.Contexts
{
    public class AuthenticationDbContextFactory : IDesignTimeDbContextFactory<AuthenticationDbContext>
    {
        private const string connectionKey = "ConnectionStrings:IdentityConnectionString";
        private const string fileLocation = "appsettings.json";
        private static string connectionString = new ConfigurationBuilder().AddJsonFile(fileLocation).Build()[connectionKey];

        public virtual AuthenticationDbContext CreateDbContext(string[] args) // required by interface
        {            
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder<AuthenticationDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new AuthenticationDbContext(optionsBuilder.Options);
        }

        public virtual AuthenticationDbContext CreateDbContext() // overload is preferable to avoid having to pass "new string[] {}" every time context is created
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder<AuthenticationDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new AuthenticationDbContext(optionsBuilder.Options);
        }
    }
}
