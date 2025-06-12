using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lexiflix.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        public int CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; }
        public List<OrderRow> OrderRows { get; internal set; }
    }

} 

