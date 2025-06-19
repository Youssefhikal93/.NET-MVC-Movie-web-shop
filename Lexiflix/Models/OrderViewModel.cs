using Lexiflix.Models.Db;

namespace Lexiflix.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
    }
}
