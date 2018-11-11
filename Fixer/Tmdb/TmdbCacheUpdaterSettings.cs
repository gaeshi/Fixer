using Fixer.Utils;

namespace Fixer.Tmdb
{
    public class TmdbCacheUpdaterSettings
    {
        private int _maxPagesToFetch = 3;
        private const int MinValidPagesToFetch = 1;
        private const int MaxValidPagesToFetch = 1000;

        public int MaxPagesToFetch
        {
            get => _maxPagesToFetch;
            set => _maxPagesToFetch = GetValidMaxPageNumber(value);
        }

        private static int GetValidMaxPageNumber(int value)
        {
            return ValidationUtils.GetValueInBoundaries(value, min: MinValidPagesToFetch, max: MaxValidPagesToFetch);
        }
    }
}