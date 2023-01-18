using AutoMapper;
using Moq; // for Mock
using SuccessfulStartup.Data.Contexts;

namespace SuccessfulStartup.DataTests.APIs
{
    [TestFixture]
    internal class ReadOnlyApiTests
    {


        [OneTimeSetUp]
        public void OneTimeSetup()
        {

        }

        [Test]
        public void GetAllPlansByAuthorId_ReturnsMatchingPlans_GivenAuthorIDWithPlans()
        {
            Assert.Ignore();
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