using Lexiflix.Data.Db;
using Lexiflix.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace Lexiflix.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly MovieDbContext _db;

        public OrderServices(MovieDbContext db) 
        {
            _db = db;
        }

        public Order GetOrderWithDetails(int id)
        {
            return _db.Orders
                .Include(o => o.OrderRows)
                    .ThenInclude(or => or.Movie) 
                .Include(o => o.Customer)         
                .FirstOrDefault(o => o.Id == id);
        }
        public void CreateOrder(Order order)
        {
            // Make a copy of the OrderRows collection
            var orderRows = order.OrderRows.ToList();

            // Clear the navigation property to avoid modification issues
            order.OrderRows.Clear();

            _db.Orders.Add(order);
            _db.SaveChanges();

            foreach (var row in orderRows)
            {
                row.OrderId = order.Id;
                row.Id = 0; // Ensure ID is not set
                _db.OrderRows.Add(row);
            }

            _db.SaveChanges();
        }

        

        public List<Order> GetAllOrders()
        {
            var orderList = _db.Orders.Include(o => o.Customer).ToList();

            return orderList;
        }

        public Movie GetMovieById(int v)
        {
           return _db.Movies.FirstOrDefault(m => m.Id == v) ?? throw new Exception("Movie not found");
        }
       


    }
}