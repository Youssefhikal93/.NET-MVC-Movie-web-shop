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

        public void CreateOrder(Order order)
        {
            throw new NotImplementedException();
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