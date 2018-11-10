using System.Collections.Generic;
using Fixer.Data;
using Fixer.Tmdb.Models;
using Fixer.Utils;
using Microsoft.Extensions.Logging;

namespace Fixer.Tmdb
{
    public class DbHelper : IDbHelper
    {
        private readonly ILogger<DbHelper> _logger;
        private readonly IDateProvider _dateProvider;
        private readonly MovieDbContext _dbContext;

        public DbHelper(ILogger<DbHelper> logger, MovieDbContext dbContext, IDateProvider dateProvider)
        {
            logger.LogDebug("Constructing DbHelper");

            _logger = logger;
            _dbContext = dbContext;
            _dateProvider = dateProvider;
        }

        public void AddOrUpdateMovies(IEnumerable<Movie> movies, Filter filter)
        {
            _logger.LogDebug("Updating/inserting movies");

            foreach (var movie in movies)
            {
                AddOrUpdate(movie, filter);
            }

            _dbContext.SaveChanges();
        }

        private void AddOrUpdate(Movie movie, Filter filter)
        {
            _logger.LogDebug($"Adding/updating movie. " +
                             $"Id='{movie.Id}', Title='{movie.Title}', filter='{filter}'");

            movie.LastUpdated = _dateProvider.Today;
            var existingMovie = _dbContext.Movies.Find(movie.Id);
            if (existingMovie == null)
            {
                _logger.LogDebug($"Existing record not found in DB. Creating.");

                movie.Filters = filter;
                _dbContext.Movies.Add(movie);
            }
            else
            {
                _logger.LogDebug($"Found existing record in DB. Updating.");

                movie.Filters = filter | existingMovie.Filters;
                _dbContext.Entry(existingMovie).CurrentValues.SetValues(movie);
            }
        }
    }
}