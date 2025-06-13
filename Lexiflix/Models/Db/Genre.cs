using System.ComponentModel.DataAnnotations;

namespace Lexiflix.Models.Db
{
    public class Genre
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        // Navigation property
        public List<Movie> Movies { get; set; } = new List<Movie>();
        public List<Genre> Genres { get; set; } = new List<Genre>();
    }
}