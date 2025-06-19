using Lexiflix.Models.Db;

namespace Lexiflix.Models
{
    public class TopCustomerViewModel
    {
        public Customer Customer { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
    }
}
