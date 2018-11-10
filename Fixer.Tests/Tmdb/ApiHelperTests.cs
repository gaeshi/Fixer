using System;
using Fixer.Tmdb;
using Fixer.Tmdb.Models;
using Fixer.Utils;
using Microsoft.Extensions.Options;
using Moq;
using RestSharp;
using Xunit;

namespace Fixer.Tests.Tmdb
{
    public class ApiHelperTests
    {
        private const string FakeBaseUrl = "https://tmdbfakeurl";
        private const string FakeApiKey = "fake key";

        [Fact]
        public void ApiHelper_NoTmdbApiKey_ThrowsApplicationException()
        {
            var restClient = Mock.Of<IRestClientWrapper>();
            var fakeOptions = Options.Create(new TmdbApiSettings());

            Assert.Throws<ApplicationException>(() => new ApiHelper(fakeOptions, restClient));
        }

        [Theory]
        [InlineData(Filter.Popular, "https://tmdbfakeurl/movie/popular")]
        [InlineData(Filter.Upcoming, "https://tmdbfakeurl/movie/upcoming")]
        [InlineData(Filter.TopRated, "https://tmdbfakeurl/movie/top_rated")]
        public void FetchMovies_VerifyRequestUrl(Filter filter, string expectedUrl)
        {
            var restClient = new Mock<IRestClientWrapper>();
            var apiHelper = CreateApiHelperWithFakeKey(restClient);

            apiHelper.FetchMovies(filter);

            VerifyRequestUrl(restClient, expectedUrl);
        }

        private static ApiHelper CreateApiHelperWithFakeKey(IMock<IRestClientWrapper> restClient)
        {
            var fakeOptions = Options.Create(new TmdbApiSettings
            {
                BaseUrl = FakeBaseUrl,
                ApiKey = FakeApiKey
            });
            
            var apiHelper = new ApiHelper(fakeOptions, restClient.Object);
            return apiHelper;
        }

        private static void VerifyRequestUrl(Mock<IRestClientWrapper> restClient, string expectedUrl)
        {
            restClient.Verify(
                c => c.ExecuteRequest<MovieResponse>(It.IsAny<IRestRequest>(), expectedUrl), Times.Once());
        }
    }
}