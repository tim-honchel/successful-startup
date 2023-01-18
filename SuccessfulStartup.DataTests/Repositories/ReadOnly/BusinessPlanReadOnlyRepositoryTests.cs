﻿using AutoMapper; // for IMapper
using Microsoft.EntityFrameworkCore; // for IDbContextFactory
using Moq; // for Mock
using Moq.EntityFrameworkCore;
using SuccessfulStartup.Data.Contexts;
using SuccessfulStartup.Data.Mapping;
using SuccessfulStartup.Data.Repositories.ReadOnly;
using SuccessfulStartup.Domain.Repositories.ReadOnly;
using System.Data;

namespace SuccessfulStartup.DataTests.Repositories.ReadOnly
{
    [TestFixture]
    internal class BusinessPlanReadOnlyRepositoryTests
    {
        private Mock<IDbContextFactory<AuthenticationDbContext>> _mockFactory;
        private IMapper _mapper;
        public IBusinessPlanReadOnlyRepository _repository;
        private Mock<AuthenticationDbContext> _mockConnection;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mockFactory = new Mock<IDbContextFactory<AuthenticationDbContext>>();
            _mapper = AllMappingProfiles.GetMapper();
            _repository = new BusinessPlanReadOnlyRepository(_mockFactory.Object, _mapper);
            _mockConnection = new Mock<AuthenticationDbContext>();
            _mockFactory.Setup(mockedFactory => mockedFactory.CreateDbContext()).Returns(_mockConnection.Object);
        }

        [Test]
        public void GetAllPlansByAuthorId_ReturnsMatchingPlans_GivenAuthorIDWithPlans()
        {
            _repository.GetAllPlansByAuthorIdAsync("authorIdWithMatches"); // TODO: Error because AuthenticationDbContext cannot be instantiated without options parameter
            Assert.Fail();
        }

        [Test]
        public void GetAllPlansByAuthorId_ReturnsEmptyList_GivenAuthorIdWithNoPlans()
        {
            Assert.Ignore();
        }

        [Test]
        public void GetAllPlansByAuthorId_ThrowsException_GivenNonexistentAuthorId()
        {
            Assert.Ignore();
        }

        [Test]
        public void GetAllPlansByAuthorId_ThrowsException_GivenInvalidAuthorId()
        {
            Assert.Ignore();
        }
    }
}
