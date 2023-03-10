using Duende.IdentityServer.EntityFramework.Options; // for OperationalStoreOptions and TableConfiguration
using Microsoft.Extensions.Options; // for IOptions interface

namespace SuccessfulStartup.Presentation.Services
{
    internal class OperationalStoreOptionsMigrations : IOptions<OperationalStoreOptions> // options for persistence of grants, tokens, cache, etc.
    {
        public OperationalStoreOptions Value => new OperationalStoreOptions()
        {
            DeviceFlowCodes = new TableConfiguration("DeviceCodes"), // stores device and user codes
            EnableTokenCleanup = false, // determines whether stale entries will be automatically cleaned up from the database
            PersistedGrants = new TableConfiguration("PersistedGrants"), // stores persisting tokens
            TokenCleanupBatchSize = 100, // number of stale tokens to remove at a time
            TokenCleanupInterval = 3600, // number of seconds between connecting to database and cleaning stale tokens
        };
    }
}
