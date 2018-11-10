using Fixer.Tmdb.Models;

namespace Fixer.Tmdb
{
    public interface IApiHelper
    {
        MovieResponse FetchMovies(Filter filter, int page = 1);
    }
}