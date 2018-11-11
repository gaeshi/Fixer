using System;
using System.Threading;
using System.Threading.Tasks;
using Fixer.Tmdb;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fixer.Services
{
    public class TmdbDataRefresherService : BackgroundService
    {
        private readonly IServiceScope _serviceScope;
        private readonly ILogger<TmdbDataRefresherService> _logger;
        private readonly TimeSpan _updateTaskDelay;

        public TmdbDataRefresherService(
            IOptions<TmdbDataRefresherServiceSettings> options, ILogger<TmdbDataRefresherService> logger, IServiceProvider services)
        {
            _logger = logger;
            _serviceScope = services.CreateScope();
            _updateTaskDelay = TimeSpan.FromMinutes(options.Value.UpdateTaskDelayMinutes);
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Triggering UpdateCache");
                try
                {
                    UpdateCache();
                    _logger.LogInformation("Cache update finished successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error updating cache: {ex}");
                }

                await Task.Delay(_updateTaskDelay, stoppingToken);
            }
        }

        private void UpdateCache()
        {
            var cacheUpdater = _serviceScope.ServiceProvider.GetRequiredService<ITmdbCacheUpdater>();
            cacheUpdater.UpdateCache();
        }

        public override void Dispose()
        {
            _serviceScope?.Dispose();
            base.Dispose();
        }
    }
}