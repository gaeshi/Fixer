### UI

filters for: 

* upcoming
* top rated
* popular

movies 

### Services

#### TmdbDataRefreshService

Periodically fetches list of movies

##### API

Base URL: `https://api.themoviedb.org/3/`

* [`GET /movie/top_rated`](https://developers.themoviedb.org/3/movies/top_rated)
* [`GET /movie/upcoming`](https://developers.themoviedb.org/3/movies/get-upcoming)
* [`GET /movie/popular`](https://developers.themoviedb.org/3/movies/upcoming)

Request example:

```csharp
var client = new RestClient($"{baseUrl}{topRated}?page=1&language=en-US&api_key=ak1234567890");
var request = new RestRequest(Method.GET);
IRestResponse response = client.Execute(request);
```

Parameters:
* **api_key (required)**
* language (optional)
* page (optional)
* region (optional)

Response JSON can be parsed using the following classes:

```csharp
    public class TmdbResponse
    {
        [JsonProperty("page")]
        public long Page { get; set; }

        [JsonProperty("total_results")]
        public long TotalResults { get; set; }

        [JsonProperty("total_pages")]
        public long TotalPages { get; set; }

        [JsonProperty("results")]
        public Result[] Results { get; set; }
    }

    public class Result
    {
        [JsonProperty("vote_count")]
        public long VoteCount { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("video")]
        public bool Video { get; set; }

        [JsonProperty("vote_average")]
        public double VoteAverage { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("popularity")]
        public double Popularity { get; set; }

        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }

        [JsonProperty("original_language")]
        public string OriginalLanguage { get; set; }

        [JsonProperty("original_title")]
        public string OriginalTitle { get; set; }

        [JsonProperty("genre_ids")]
        public long[] GenreIds { get; set; }

        [JsonProperty("backdrop_path")]
        public string BackdropPath { get; set; }

        [JsonProperty("adult")]
        public bool Adult { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("release_date")]
        public DateTimeOffset ReleaseDate { get; set; }
    }
```

The "Upcoming" API also contains `dates` field, e.g.: `"dates": { "maximum": "2018-12-05", "minimum": "2018-11-14" }`.

##### Sample responses

###### top_rated

```json5
{
  "page": 1,
  "total_results": 6430,
  "total_pages": 322,
  "results": [
    {
      "vote_count": 1903,
      "id": 19404,
      "video": false,
      "vote_average": 9.2,
      "title": "Dilwale Dulhania Le Jayenge",
      "popularity": 15.266,
      "poster_path": "/uC6TTUhPpQCmgldGyYveKRAu8JN.jpg",
      "original_language": "hi",
      "original_title": "दिलवाले दुल्हनिया ले जायेंगे",
      "genre_ids": [
        35,
        18,
        10749
      ],
      "backdrop_path": "/nl79FQ8xWZkhL3rDr1v2RFFR6J0.jpg",
      "adult": false,
      "overview": "Raj is a rich, carefree, happy-go-lucky second generation NRI. Simran is the daughter of Chaudhary Baldev Singh, who in spite of being an NRI is very strict about adherence to Indian values. Simran has left for India to be married to her childhood fiancé. Raj leaves for India with a mission at his hands, to claim his lady love under the noses of her whole family. Thus begins a saga.",
      "release_date": "1995-10-20"
    },
    // more results, total 20 per page
  ]
}
```

###### popular

```json5
{
  "page": 1,
  "total_results": 19825,
  "total_pages": 992,
  "results": [
    {
      "vote_count": 2012,
      "id": 335983,
      "video": false,
      "vote_average": 6.6,
      "title": "Venom",
      "popularity": 266.273,
      "poster_path": "/2uNW4WbgBXL25BAbXGLnLqX71Sw.jpg",
      "original_language": "en",
      "original_title": "Venom",
      "genre_ids": [
        878
      ],
      "backdrop_path": "/VuukZLgaCrho2Ar8Scl9HtV3yD.jpg",
      "adult": false,
      "overview": "When Eddie Brock acquires the powers of a symbiote, he will have to release his alter-ego \"Venom\" to save his life.",
      "release_date": "2018-10-03"
    },
    // more results, total 20 per page
  ]
}
```

###### upcoming

```json5
{
  "results": [
    {
      "vote_count": 525,
      "id": 424694,
      "video": false,
      "vote_average": 8.3,
      "title": "Bohemian Rhapsody",
      "popularity": 224.121,
      "poster_path": "/lHu1wtNaczFPGFDTrjCSzeLPTKN.jpg",
      "original_language": "en",
      "original_title": "Bohemian Rhapsody",
      "genre_ids": [
        18,
        10402
      ],
      "backdrop_path": "/pbXgLEYh8rlG2Km5IGZPnhcnuSz.jpg",
      "adult": false,
      "overview": "Singer Freddie Mercury, guitarist Brian May, drummer Roger Taylor and bass guitarist John Deacon take the music world by storm when they form the rock 'n' roll band Queen in 1970. Hit songs become instant classics. When Mercury's increasingly wild lifestyle starts to spiral out of control, Queen soon faces its greatest challenge yet – finding a way to keep the band together amid the success and excess.",
      "release_date": "2018-10-24"
    },
    // more results, total 20 per page
  ],
  "page": 1,
  "total_results": 288,
  "dates": {
    "maximum": "2018-12-05",
    "minimum": "2018-11-14"
  },
  "total_pages": 15
}
```