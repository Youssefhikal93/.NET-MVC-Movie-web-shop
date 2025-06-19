


using System.ComponentModel.DataAnnotations;

namespace Lexiflix.Models.ViewModels
{

    public class OrderVM
    {
        public int? CustomerId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public List<OrderRowVM> OrderRows { get; set; } = new();

    }

    public class OrderRowVM
    {   
        public int? MovieId { get; set; }
        [Required(ErrorMessage = "Quantity is required")]
        public int? Quantity { get; set; }
        

        public decimal? Price { get; set; }
    }
}