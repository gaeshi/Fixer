using System;
using System.Threading;

namespace Fixer.Tmdb
{
    public class FakeTmdbCacheUpdater : ITmdbCacheUpdater
    {
        public bool IsUpdateCacheCalled { get; private set; }
        
        public void UpdateCache(CancellationToken cancellationToken)
        {
            IsUpdateCacheCalled = true;
        }
    }
}