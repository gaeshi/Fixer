using Fixer.Services;
using Fixer.Tmdb;
using Xunit;

namespace Fixer.Tests.Tmdb
{
    public class TmdbCacheUpdaterSettingsTests
    {
        [Theory]
        [InlineData(-1, 1)]
        [InlineData(0, 1)]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(999, 999)]
        [InlineData(1000, 1000)]
        [InlineData(1001, 1000)]
        public void MaxPagesToFetch_ReturnsValidNumber(int input, int expectedResult)
        {
            var settings = new TmdbCacheUpdaterSettings {MaxPagesToFetch = input};

            var actual = settings.MaxPagesToFetch;

            Assert.Equal(expectedResult, actual);
        }
    }
}