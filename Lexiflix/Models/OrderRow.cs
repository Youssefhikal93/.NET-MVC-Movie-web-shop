using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Lexiflix.Models
{
    public class OrderRow
    {
        [Key]
        public int Id { get; set; }

      

        [Required]
        [Range(0, 5000, ErrorMessage = "Price must be between 0 and 5000.")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required]
        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }

        [Required]
        [ForeignKey(nameof(Movie))]
        public int MovieId { get; set; }

        public Order Order { get; set; }
        public Movie Movie { get; set; }
        public int Quantity { get;  set; }
    }

}
