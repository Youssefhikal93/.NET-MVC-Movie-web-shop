


//using System.ComponentModel.DataAnnotations;

//namespace Lexiflix.Models.ViewModels
//{

//    public class OrderVM
//    {
//        public int? CustomerId { get; set; }

//        public DateTime OrderDate { get; set; } = DateTime.Now;

//        public List<OrderRowVM> OrderRows { get; set; } = new();

//    }

//    public class OrderRowVM
//    {   
//        public int? MovieId { get; set; }
//        [Required(ErrorMessage = "Quantity is required")]
//        public int? Quantity { get; set; }


//        public decimal? Price { get; set; }
//    }
//}

using System.ComponentModel.DataAnnotations;
namespace Lexiflix.Models.ViewModels
{
    public class OrderVM
    {
        public int? Id { get; set; } // Added for edit functionality
        public int? CustomerId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public List<OrderRowVM> OrderRows { get; set; } = new();

        // Display properties for edit view (read-only)
        public string? CustomerEmail { get; set; }
        public string? CustomerName { get; set; }
        public string? DeliveryAddress { get; set; }
    }

    public class OrderRowVM
    {
        public int? Id { get; set; } // Added for edit functionality
        public int? MovieId { get; set; }
        [Required(ErrorMessage = "Quantity is required")]
        public int? Quantity { get; set; }

        public decimal? Price { get; set; }

        // Display property for edit view
        public string? MovieTitle { get; set; }
    }
}