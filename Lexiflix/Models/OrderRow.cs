namespace Lexiflix.Models
{
    public class OrderRow
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int MovieId { get; set; }
        public Decimal Price { get; set; }

        //public Order order { get; set; }
        //public Movie movie { get; set; }
    }
}
