using Lexiflix.Data.Db;
using Lexiflix.Models.Db;
using Lexiflix.Models.ViewModels;
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
        





             public void AddOrderByAdmin(OrderVM ovm)
        {
            var validMovies = _db.Movies.Select(m => m.Id).ToHashSet();
            var order = new Order
            {

                CustomerId = ovm.CustomerId ?? 0,
                OrderDate = ovm.OrderDate,
                OrderRows = ovm.OrderRows
                .Where(row => row.MovieId.HasValue && validMovies.Contains(row.MovieId.Value)).Select(row => new OrderRow
                {

                    MovieId = row.MovieId ?? 0,
                    Quantity = row.Quantity ?? 0,
                    Price = row.Price ?? 0
                }).ToList()

            };

            _db.Orders.Add(order);
            _db.SaveChanges();

        }
       


    }
}