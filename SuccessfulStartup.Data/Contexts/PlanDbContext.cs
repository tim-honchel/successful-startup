﻿using Microsoft.EntityFrameworkCore; // for DbContext, DbContextOptionsBuilder
using Microsoft.Extensions.Configuration; // for ConfiugrationBuilder
using SuccessfulStartup.Data.Entities;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SuccessfulStartup.DataTests")] // allows tests to access internal members

namespace SuccessfulStartup.Data.Contexts
{
    public class PlanDbContext : DbContext
    {
        private const string _connectionKey = "ConnectionStrings:BusinessPlanConnectionString";
        private const string _fileLocation = "appsettings.json";
        internal static string _connectionString; // switch to hard code when adding migration

        public virtual DbSet<BusinessPlan> BusinessPlans { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public PlanDbContext(bool testing = false)
        {
            _connectionString = testing ? "dummy" : new ConfigurationBuilder().AddJsonFile(_fileLocation).Build()[_connectionKey];
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(_connectionString);
        }
    }
}
