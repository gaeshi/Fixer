using System;
using System.Collections.Generic;
using Fixer.Tmdb.Models;
using Fixer.Utils;
using Microsoft.Extensions.Options;
using RestSharp;

namespace Fixer.Tmdb
{
    public class ApiHelper : IApiHelper
    {
        private readonly IRestClientWrapper _restClient;
        private readonly TmdbApiSettings _settings;

        private static IDictionary<Filter, string> EndpointMethodUrl { get; } = new Dictionary<Filter, string>()
        {
            {Filter.Popular, "/movie/popular"},
            {Filter.TopRated, "/movie/top_rated"},
            {Filter.Upcoming, "/movie/upcoming"}
        };

        public ApiHelper(IOptions<TmdbApiSettings> settings, IRestClientWrapper restClient)
        {
            if (string.IsNullOrEmpty(settings.Value.ApiKey))
            {
                throw new ApplicationException("Please provide api key for TheMovieDB API.");
            }

            _settings = settings.Value;
            _restClient = restClient;
        }

        public MovieResponse FetchMovies(Filter filter, int page = 1)
        {
            var request = new RestRequest(Method.GET);
            request.AddQueryParameter(_settings.ApiKeyRequestParamName, _settings.ApiKey);
            request.AddQueryParameter(_settings.PageRequestParamName, page.ToString());

            var url = $"{_settings.BaseUrl}{EndpointMethodUrl[filter]}";
            
            return _restClient.ExecuteRequest<MovieResponse>(request, url);
        }
    }
}