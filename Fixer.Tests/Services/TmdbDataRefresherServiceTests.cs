using System;
using System.Threading;
using System.Threading.Tasks;
using Fixer.Services;
using Fixer.Tmdb;
using Xunit;

namespace Fixer.Tests
{
    public class TmdbDataRefresherServiceTests
    {
        [Fact]
        public async Task StartingAndStoppingService_CallsUpdateCache()
        {
            var cacheUpdater = new FakeTmdbCacheUpdater();
            var service = new TmdbDataRefresherService(cacheUpdater);
            
            await service.StartAsync(CancellationToken.None);
            
            Assert.True(cacheUpdater.IsUpdateCacheCalled);
        }
    }
}