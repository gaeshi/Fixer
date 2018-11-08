using System;
using System.Threading;
using System.Threading.Tasks;
using Fixer.Tmdb;
using Microsoft.Extensions.Hosting;

namespace Fixer.Services
{
    public class TmdbDataRefresherService : BackgroundService
    {
        private readonly ITmdbCacheUpdater _cacheUpdater;

        public TmdbDataRefresherService(ITmdbCacheUpdater cacheUpdater)
        {
            _cacheUpdater = cacheUpdater;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _cacheUpdater.UpdateCache(stoppingToken);
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}