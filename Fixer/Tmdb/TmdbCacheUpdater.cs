using System;
using System.Collections.Generic;
using Fixer.Tmdb.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fixer.Tmdb
{
    public class TmdbCacheUpdater : ITmdbCacheUpdater
    {
        private readonly ILogger<TmdbCacheUpdater> _logger;
        private readonly ApiHelper _apiHelper;
        private readonly DbHelper _dbHelper;
        private readonly int _maxPage;

        public TmdbCacheUpdater(IOptions<TmdbCacheUpdaterSettings> options, ILogger<TmdbCacheUpdater> logger, ApiHelper apiHelper, DbHelper dbHelper)
        {
            _logger = logger;
            _apiHelper = apiHelper;
            _dbHelper = dbHelper;
            _maxPage = options.Value.MaxPagesToFetch;
        }

        public void UpdateCache()
        {
            _logger.LogInformation("Updating cache");
            try
            {
                FetchAll();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Update failed: {ex}");
            }
        }

        private void FetchAll()
        {
            var totalPages = new Dictionary<Filter, int>();
            for (var page = 1; page <= _maxPage; page++)
            {
                _logger.LogInformation($"Processing page {page}");
                ProcessPage(totalPages, page);
            }
        }

        private void ProcessPage(IDictionary<Filter, int> totalPages, int page)
        {
            foreach (var filter in (Filter[]) Enum.GetValues(typeof(Filter)))
            {
                _logger.LogInformation($"Processing filter {filter}");
                if (AllPagesProcessed(totalPages, page, filter))
                {
                    continue;
                }

                var movieResponse = _apiHelper.FetchMovies(filter, page);
                totalPages[filter] = movieResponse.TotalPages;
                _dbHelper.AddOrUpdateMovies(movieResponse.Results, filter);
            }
        }

        private static bool AllPagesProcessed(IDictionary<Filter, int> totalPages, int currentPage, Filter filter)
        {
            return totalPages.ContainsKey(filter) && currentPage > totalPages[filter];
        }
    }
}