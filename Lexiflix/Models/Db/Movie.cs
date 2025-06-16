using System.ComponentModel.DataAnnotations;

namespace Lexiflix.Models.Db
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
        public string Title { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Director cannot be longer than 100 characters.")]
        public string Director { get; set; }

        [Required]
        [Range(1900,2100)] public int ReleaseYear { get; set; }

        [Required]
        [Range(0, 1000, ErrorMessage = "Price must be between 0 and 2000.")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Url(ErrorMessage = "Please enter a valid URL.")]
        public string ImageUrl { get; set; }


        public string? Plot { get; set; }

        public string? Genre { get; set; }

        public int? Runtime { get; set; } 

        public string? Rating { get; set; } // PG-13, R, etc.

        public decimal? ImdbRating { get; set; }

        // Many-to-many relationship with Actor
        public ICollection<Actor> Actors { get; set; } = new List<Actor>();
        public ICollection<Genre> Genres { get; set; } = new List<Genre>();

    }
}
