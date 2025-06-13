using System.Text.Json.Serialization;

namespace Lexiflix.Utils
{
    public class OmdbMovieResponse
    {
        [JsonPropertyName("Title")]
        public string Title { get; set; }

        [JsonPropertyName("Year")]
        public string Year { get; set; }

        [JsonPropertyName("Director")]
        public string Director { get; set; }

        [JsonPropertyName("Actors")]
        public string Actors { get; set; }

        [JsonPropertyName("Poster")]
        public string Poster { get; set; }

        [JsonPropertyName("Plot")]
        public string Plot { get; set; }

        [JsonPropertyName("Genre")]
        public string Genre { get; set; }

        [JsonPropertyName("Runtime")]
        public string Runtime { get; set; }

        [JsonPropertyName("Rated")]
        public string Rated { get; set; }

        [JsonPropertyName("imdbRating")]
        public string ImdbRating { get; set; }

        [JsonPropertyName("Error")]
        public string Error { get; set; }
    }
}