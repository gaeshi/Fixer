using System;
using Newtonsoft.Json;

namespace Fixer.Tmdb.Models
{
    public class Movie
    {
        [JsonProperty("id")] public long Id { get; set; }

        [JsonProperty("vote_count")] public long VoteCount { get; set; }

        [JsonProperty("vote_average")] public double VoteAverage { get; set; }

        [JsonProperty("title")] public string Title { get; set; }

        [JsonProperty("popularity")] public double Popularity { get; set; }

        [JsonProperty("poster_path")] public string PosterPath { get; set; }

        [JsonProperty("original_language")] public string OriginalLanguage { get; set; }

        [JsonProperty("original_title")] public string OriginalTitle { get; set; }

        [JsonProperty("backdrop_path")] public string BackdropPath { get; set; }

        [JsonProperty("adult")] public bool Adult { get; set; }

        [JsonProperty("overview")] public string Overview { get; set; }

        [JsonProperty("release_date")] public DateTime ReleaseDate { get; set; }

        #region Non-Json properties

        public DateTime LastUpdated { get; set; }
        public Filter Filters { get; set; }

        #endregion
    }
}