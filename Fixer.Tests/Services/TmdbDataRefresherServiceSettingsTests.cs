using Fixer.Services;
using Xunit;

namespace Fixer.Tests.Services
{
    public class TmdbDataRefresherServiceSettingsTests
    {
        [Theory]
        [InlineData(-1, 1)]
        [InlineData(0, 1)]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(100500, 100500)]
        public void UpdateTaskDelayMinutes_ReturnsValidNumber(int input, int expectedResult)
        {
            var settings = new TmdbDataRefresherServiceSettings {UpdateTaskDelayMinutes = input};

            var actual = settings.UpdateTaskDelayMinutes;

            Assert.Equal(expectedResult, actual);
        }
    }
}