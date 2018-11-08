using Fixer.Tmdb;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using Xunit;

namespace Fixer.Tests
{
    public class StartupTests
    {
        [Fact]
        public void Startup_Constructor_AssignsConfiguration()
        {
            var mockConfiguration = new Mock<IConfiguration>();
            
            var startup = new Startup(mockConfiguration.Object);
            
            Assert.Equal(mockConfiguration.Object, startup.Configuration);
        }

        [Fact]
        public void ConfigureServices_AddsCacheUpdater()
        {
            var serviceProvider = ConfigureServicesWithMocks();

            var tmdbCacheUpdater = serviceProvider.GetService<ITmdbCacheUpdater>();

            Assert.NotNull(tmdbCacheUpdater);
        }

        [Fact]
        public void ConfigureServices_AddsTmdbDataRefresherService()
        {
            var serviceProvider = ConfigureServicesWithMocks();

            var tmdbDataRefresherService = serviceProvider.GetService<IHostedService>();

            Assert.NotNull(tmdbDataRefresherService);
        }

        private static ServiceProvider ConfigureServicesWithMocks()
        {
            var mockConfiguration = new Mock<IConfiguration>();
            var startup = new Startup(mockConfiguration.Object);

            var services = new ServiceCollection();
            startup.ConfigureServices(services);

            return services.BuildServiceProvider();
        }
    }
}