using System.Threading;

namespace Fixer.Tmdb
{
    public interface ITmdbCacheUpdater
    {
        void UpdateCache(CancellationToken cancellationToken);
    }
}