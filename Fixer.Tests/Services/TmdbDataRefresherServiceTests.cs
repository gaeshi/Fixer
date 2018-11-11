using System;
using System.Threading;
using System.Threading.Tasks;
using Fixer.Services;
using Fixer.Tmdb;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Fixer.Tests.Services
{
    public class TmdbDataRefresherServiceTests
    {
        private static IOptions<TmdbDataRefresherServiceSettings> FakeOptions
            => Options.Create(new TmdbDataRefresherServiceSettings {UpdateTaskDelayMinutes = 1});

        private static ILogger<TmdbDataRefresherService> FakeLogger
            => Mock.Of<ILogger<TmdbDataRefresherService>>();

        [Fact]
        public async Task StartingAndStoppingService_CallsUpdateCache()
        {
            var services = new ServiceCollection();
            services.AddTransient<ITmdbCacheUpdater, FakeTmdbCacheUpdater>();

            var service = new TmdbDataRefresherService(FakeOptions, FakeLogger, services.BuildServiceProvider());

            Assert.False(FakeTmdbCacheUpdater.IsUpdateCacheCalled);
            await service.StartAsync(CancellationToken.None);
            Assert.True(FakeTmdbCacheUpdater.IsUpdateCacheCalled);
        }

        [Fact]
        public void Dispose_DisposesServiceScope()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            var mockServiceScope = new Mock<IServiceScope>();
            mockServiceScope.Setup(x => x.ServiceProvider).Returns(mockServiceProvider.Object);

            var mockServiceScopeFactory = new Mock<IServiceScopeFactory>();
            mockServiceScopeFactory
                .Setup(x => x.CreateScope())
                .Returns(mockServiceScope.Object);
            mockServiceProvider
                .Setup(x => x.GetService(typeof(IServiceScopeFactory)))
                .Returns(mockServiceScopeFactory.Object);

            // Act
            var service = new TmdbDataRefresherService(FakeOptions, FakeLogger, mockServiceProvider.Object);
            service.Dispose();

            mockServiceScope.Verify(ss => ss.Dispose(), Times.Once);
        }

        [Fact]
        public void UpdateCache_Exception_DoesNotRethrow()
        {
            var mockTmdbCacheUpdater = new Mock<ITmdbCacheUpdater>();
            mockTmdbCacheUpdater
                .Setup(tcu => tcu.UpdateCache())
                .Throws<ApplicationException>();

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider
                .Setup(sp => sp.GetService(typeof(ITmdbCacheUpdater)))
                .Returns(mockTmdbCacheUpdater.Object);
            var mockServiceScope = new Mock<IServiceScope>();
            mockServiceScope.Setup(x => x.ServiceProvider).Returns(mockServiceProvider.Object);

            var mockServiceScopeFactory = new Mock<IServiceScopeFactory>();
            mockServiceScopeFactory
                .Setup(x => x.CreateScope())
                .Returns(mockServiceScope.Object);
            mockServiceProvider
                .Setup(x => x.GetService(typeof(IServiceScopeFactory)))
                .Returns(mockServiceScopeFactory.Object);

            // Act
            var service = new TmdbDataRefresherService(FakeOptions, FakeLogger, mockServiceProvider.Object);
            service.StartAsync(CancellationToken.None);

            mockTmdbCacheUpdater.Verify(tcu => tcu.UpdateCache(), Times.Once);
        }

        public class FakeTmdbCacheUpdater : ITmdbCacheUpdater
        {
            public static bool IsUpdateCacheCalled { get; private set; }

            public void UpdateCache()
            {
                IsUpdateCacheCalled = true;
            }
        }
    }
}