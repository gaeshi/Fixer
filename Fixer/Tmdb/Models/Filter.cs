using System;

namespace Fixer.Tmdb.Models
{
    [Flags]
    public enum Filter
    {
        Popular = 1,
        TopRated = 2,
        Upcoming = 4
    }
}