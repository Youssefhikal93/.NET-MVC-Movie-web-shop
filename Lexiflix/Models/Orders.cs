using System.ComponentModel.DataAnnotations;

namespace Lexiflix.Models
{
    public class Orders
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now; /*/*Default to current date*/
        [Required]
        public int CustomerId { get; set; }  
        //public Customer customer { get; set;}



    }

} 

