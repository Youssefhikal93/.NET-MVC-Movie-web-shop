using Lexiflix.Data.Db;
using Lexiflix.Models;
using Lexiflix.Models.Db;
using Lexiflix.Models.ViewModels;
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

            return PaginatedList<OrderViewModel>.Create(query, pageIndex, pageSize); 
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


        public void DeleteOrder(int id)

        {
            var order = _db.Orders.FirstOrDefault(o => o.Id == id);
            if (order != null)
            {
                _db.Orders.Remove(order);
                _db.SaveChanges();
            }
        }

        public void UpdateOrder(OrderVM orderVM)
        {
            var existingOrder = _db.Orders
                .Include(o => o.OrderRows)
                .FirstOrDefault(o => o.Id == orderVM.Id);

            if (existingOrder == null)
                throw new Exception("Order not found");

            // Update order date if changed
            existingOrder.OrderDate = orderVM.OrderDate;
            _db.OrderRows.RemoveRange(existingOrder.OrderRows);

            // Add new order rows
            var validMovies = _db.Movies.Select(m => m.Id).ToHashSet();
            existingOrder.OrderRows = orderVM.OrderRows
                .Where(row => row.MovieId.HasValue &&
                             row.Quantity.HasValue &&
                             row.Price.HasValue &&
                             row.Quantity > 0 &&
                             validMovies.Contains(row.MovieId.Value))
                .Select(row => new OrderRow
                {
                    OrderId = existingOrder.Id,
                    MovieId = row.MovieId.Value,
                    Quantity = row.Quantity.Value,
                    Price = row.Price.Value
                }).ToList();

            _db.SaveChanges();
        }
    }
}