using NUnit.Framework;
using static SatellEarthAPI.Application.IntegrationTests.Testing;

namespace SatellEarthAPI.Application.IntegrationTests
{
    [TestFixture]
    public abstract class BaseTestFixture
    {
        [SetUp]
        public async Task TestSetUp()
        {
            await ResetState();
        }
    }
}