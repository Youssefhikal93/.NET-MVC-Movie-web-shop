using Lexiflix.Data.Db;
using Lexiflix.Models;
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
            throw new NotImplementedException();
        }
       


    }
}