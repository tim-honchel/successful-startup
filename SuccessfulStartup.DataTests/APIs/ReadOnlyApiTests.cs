namespace SuccessfulStartup.DataTests.APIs
{
    public class ReadOnlyApiTests
    {
        [SetUp]
        public void Setup()
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