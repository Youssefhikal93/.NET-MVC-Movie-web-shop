using AspNetCoreGeneratedDocument;
using System.ComponentModel.DataAnnotations;

namespace Lexiflix.Models.Db
{
    public class Actor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        // Many-to-many relationship with Movie
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();

        
    }
}

