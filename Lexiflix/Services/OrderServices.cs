using Lexiflix.Data.Db;
using Lexiflix.Models;
using Lexiflix.Models.Db;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            order.OrderRows.Clear();

            _db.Orders.Add(order);
            _db.SaveChanges();

            foreach (var row in orderRows)
            {
                row.OrderId = order.Id;
                row.Id = 0; 
                _db.OrderRows.Add(row);
            }

            _db.SaveChanges();
        }

        public PaginatedList<OrderViewModel> GetAllOrders(string searchString, int pageIndex, int pageSize)
        {
            // Start with base query including related entities
            var query = _db.Orders
                .Include(o => o.Customer)
                .Select(o => new OrderViewModel
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    CustomerName = $"{o.Customer.FirstName} {o.Customer.LastName}",
                })
                .OrderByDescending(o => o.OrderDate)
                .AsQueryable();

            return PaginatedList<OrderViewModel>.Create(query, pageIndex, pageSize); ;
        }
       
       
        public Movie GetMovieById(int v)
        {
           return _db.Movies.FirstOrDefault(m => m.Id == v) ?? throw new Exception("Movie not found");
        }
       


    }
}