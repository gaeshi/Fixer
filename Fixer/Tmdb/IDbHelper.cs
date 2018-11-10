using System.Collections.Generic;
using Fixer.Tmdb.Models;

namespace Fixer.Tmdb
{
    public interface IDbHelper
    {
        void AddOrUpdateMovies(IEnumerable<Movie> movies, Filter filter);
    }
}