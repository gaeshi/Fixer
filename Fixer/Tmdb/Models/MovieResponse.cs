using System.Collections.Generic;
using Newtonsoft.Json;

namespace Fixer.Tmdb.Models
{
    public class MovieResponse
    {
        [JsonProperty("page")]
        public long Page { get; set; }

        [JsonProperty("total_results")]
        public long TotalResults { get; set; }

        [JsonProperty("total_pages")]
        public long TotalPages { get; set; }

        [JsonProperty("results")]
        public List<Movie> Results { get; set; }
    }
}