using Lexiflix.Models.Db;
using System.ComponentModel.DataAnnotations;

namespace Lexiflix.Models
{
    
        public class MovieUpdateVM
        {
            public int Id { get; set; }

            [MaxLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
            public string? Title { get; set; }

            [MaxLength(100, ErrorMessage = "Director cannot be longer than 100 characters.")]
            public string? Director { get; set; }

            [Range(1900, 2100)]
            public int? ReleaseYear { get; set; }

            [Range(0, 1000, ErrorMessage = "Price must be between 0 and 1000.")]
            public decimal? Price { get; set; }

            [Url(ErrorMessage = "Please enter a valid URL.")]
            public string? ImageUrl { get; set; }

            public string? Plot { get; set; }
            public string? Genre { get; set; }
            public int? Runtime { get; set; }
            public string? Rating { get; set; }

            [Range(0, 10)]
            public decimal? ImdbRating { get; set; }

            // For handling many-to-many relationships
            public List<int> SelectedActorIds { get; set; } = new List<int>();
            public List<int> SelectedGenreIds { get; set; } = new List<int>();

            // For display purposes
            public List<Actor> AvailableActors { get; set; } = new List<Actor>();
            public List<Genre> AvailableGenres { get; set; } = new List<Genre>();
        }
    
}
